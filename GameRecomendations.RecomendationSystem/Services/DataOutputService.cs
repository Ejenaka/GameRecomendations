using GameRecomendations.RecomendationSystem.Contracts;
using GameRecomendations.Shared;

namespace GameRecomendations.RecomendationSystem.Services;

public class DataOutputService : IDataOutput
{
    private readonly IDataLoader _dataLoader;

    public List<VideoGame> OutputData()
    {
        throw new NotImplementedException();
    }
}
