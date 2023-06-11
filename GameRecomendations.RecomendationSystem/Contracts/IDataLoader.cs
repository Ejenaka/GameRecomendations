using GameRecomendations.Shared;

namespace GameRecomendations.RecomendationSystem.Contracts
{
    public interface IDataLoader
    {
        Task<List<VideoGame>> LoadDataAsync(string source);

        List<string> LoadVideoGamesTags(IEnumerable<VideoGame> videoGames);

        List<VideoGame> GetLoadedData();
    }
}
