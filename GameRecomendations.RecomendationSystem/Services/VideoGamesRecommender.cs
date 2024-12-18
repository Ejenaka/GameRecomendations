﻿using Accord.MachineLearning;
using Accord.Math.Distances;
using GameRecomendations.RecomendationSystem.Contracts;
using GameRecomendations.Shared;
using System.Collections.Concurrent;

namespace GameRecomendations.RecomendationSystem.Services;

public class VideoGamesRecommender : IRecommender
{
    private readonly IDataLoader _dataLoader;
    private readonly IDataProcessor<string[][]> _dataProcessor;
    private List<VideoGame> _videoGames;
    private double[][]? _tfIdfMatrix;

    public VideoGamesRecommender(IDataLoader dataLoader, IDataProcessor<string[][]> dataProcessor)
    {
        _dataLoader = dataLoader;
        _dataProcessor = dataProcessor;
    }

    public async Task<List<VideoGame>> RecommendVideoGamesAsync(IEnumerable<VideoGame> videoGames)
    {
        _videoGames = _dataLoader.GetLoadedData();

        var tfIdfMatrix = await GetOrCalculateTfIdfMatrixAsync();

        var similarityScoresVectors = CalculateSimilarityScoresVectors(tfIdfMatrix, videoGames).ToList();
        var averagedSimilarityScores = CalculateAverageSimilarityScoresVector(similarityScoresVectors);

        foreach (var (gameIndex, similarityScore) in averagedSimilarityScores)
        {
            _videoGames[gameIndex].RecommendationScore = (int)similarityScore;
        }

        return _videoGames.OrderByDescending(g => g.RecommendationScore).ToList();
    }

    private async Task<double[][]> GetOrCalculateTfIdfMatrixAsync()
    {
        if (_tfIdfMatrix != null)
        {
            return _tfIdfMatrix;
        }

        var processedData = await _dataProcessor.ProcessDataAsync(_videoGames);

        var tfidf = new TFIDF
        {
            Tf = TermFrequency.Log,
            Idf = InverseDocumentFrequency.Smooth,
        };

        tfidf.Learn(processedData);

        _tfIdfMatrix = tfidf.Transform(processedData);

        return _tfIdfMatrix;
    }

    private IEnumerable<List<(int Index, double RecommendationScore)>> CalculateSimilarityScoresVectors(double[][] tfIdfMatrix, IEnumerable<VideoGame> likedVideoGames)
    {
        var likedVideoGamesNames = likedVideoGames.Select(g => g.Name);
        var likedVideoGamesIndexes = likedVideoGamesNames.Select(name => _videoGames.FindIndex(g => g.Name == name));

        var cosine = new Cosine();

        foreach (var (likedVideGame, likedVideGameIndex) in likedVideoGames.Zip(likedVideoGamesIndexes, Tuple.Create))
        {
            var gameTfIdfMatrixRow = tfIdfMatrix[likedVideGameIndex];
            var gameSimilarityScores = new ConcurrentBag<(int, double)>();

            for (int i = 0; i < tfIdfMatrix.GetLength(0); i++)
            {
                var tfIdfRow = tfIdfMatrix[i];
                gameSimilarityScores.Add((i, cosine.Similarity(gameTfIdfMatrixRow, tfIdfRow)));
            }

            yield return gameSimilarityScores.ToList();
        }
    }

    private IEnumerable<(int Index, double RecommendationScore)> CalculateAverageSimilarityScoresVector(List<List<(int Index, double RecommendationScore)>> similatiryVectors)
    {
        for (var i = 0; i < similatiryVectors[0].Count; i++)
        {
            var gameIndex = similatiryVectors[0][i].Index;

            var averageScore = 0.0;

            for (var j = 0; j < similatiryVectors.Count; j++)
            {
                averageScore += similatiryVectors[j][i].RecommendationScore;
            } 

            averageScore /= similatiryVectors.Count;

            averageScore = Math.Round(averageScore * 100);

            yield return (gameIndex, averageScore);
        }
    }
}
