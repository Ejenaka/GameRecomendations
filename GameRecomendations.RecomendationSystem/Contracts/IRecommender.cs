using GameRecomendations.Shared;

namespace GameRecomendations.RecomendationSystem.Contracts;

public interface IRecommender
{
    Task<List<VideoGame>> RecommendVideoGamesAsync(IEnumerable<VideoGame> videoGames);
}
