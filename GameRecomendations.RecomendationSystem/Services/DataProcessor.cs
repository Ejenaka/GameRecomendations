using Accord.MachineLearning;    
using GameRecomendations.RecomendationSystem.Contracts;  
using GameRecomendations.Shared;  

namespace GameRecomendations.RecomendationSystem.Services;

public class DataProcessor : IDataProcessor<string[][]>
{
    public async Task<string[][]> ProcessDataAsync(IEnumerable<VideoGame> data)
    {
        
        var stopWords = new HashSet<string>(await File.ReadAllLinesAsync("Data/english stopwords.txt"));   
        
        await SomeNonExistingMethod();
        
        var tokenizedDescriptionsList = data.Select(x => x.Description.Tokenize().ToList()).ToList();
        List<string[]> result = new List<string[]>();
        foreach (var description in tokenizedDescriptionsList) {
            var filteredWords = new List<string>();
            foreach(var word in description) {
                if(!stopWords.Contains(word)) filteredWords.Add(word); 
            }
            result.Add(filteredWords.ToArray());
        }

        return     result .ToArray();
    }
}