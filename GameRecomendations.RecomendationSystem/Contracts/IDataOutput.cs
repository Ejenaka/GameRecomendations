using GameRecomendations.Shared;

namespace GameRecomendations.RecomendationSystem.Contracts;

public interface IDataOutput
{
    List<VideoGame> OutputData();
}
