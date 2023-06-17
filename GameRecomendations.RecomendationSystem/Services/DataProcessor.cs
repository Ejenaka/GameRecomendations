using Accord.MachineLearning;
using GameRecomendations.RecomendationSystem.Contracts;
using GameRecomendations.Shared;

namespace GameRecomendations.RecomendationSystem.Services;

public class DataProcessor : IDataProcessor<string[][]>
{
    public async Task<string[][]> ProcessDataAsync(IEnumerable<VideoGame> data)
    {
        var stopWords = new HashSet<string>(await File.ReadAllLinesAsync("Data/english stopwords.txt"));
        var tokenizedDescriptionsList = data.Select(x => x.Description.Tokenize().ToList()).ToList();

        var processedData = tokenizedDescriptionsList.AsParallel().Select(description =>
            description.Where(word => !stopWords.Contains(word)).ToArray()
        ).ToArray();

        return processedData;
    }
}
