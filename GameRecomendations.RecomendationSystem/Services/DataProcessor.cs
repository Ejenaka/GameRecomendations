using Accord.MachineLearning;
using GameRecomendations.RecomendationSystem.Contracts;
using GameRecomendations.Shared;

namespace GameRecomendations.RecomendationSystem.Services;

public class DataProcessor : IDataProcessor<string[][]>
{
    private string[][]? _processedData; 

    public async Task<string[][]> ProcessDataAsync(IEnumerable<VideoGame> data)
    {
        var stopWords = await File.ReadAllLinesAsync("Data/english stopwords.txt");
        var tokenizedDescriptionsList = data.Select(x => x.Description.Tokenize().ToList()).ToList();

        Parallel.ForEach(tokenizedDescriptionsList, description => description.RemoveAll(s => stopWords.Contains(s)));

        _processedData = tokenizedDescriptionsList.Select(x => x.ToArray()).ToArray();

        return _processedData;
    }

    public string[][] GetProcessedData()
    {
        return _processedData ?? throw new InvalidOperationException("Data has not been processed.");
    }
}
