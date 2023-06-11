using GameRecomendations.Shared;

namespace GameRecomendations.RecomendationSystem.Contracts;

public interface IDataProcessor<T>
{
    Task<T> ProcessDataAsync(IEnumerable<VideoGame> data);

    T GetProcessedData();
}
