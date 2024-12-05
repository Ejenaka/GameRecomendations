using craftersmine.SteamGridDBNet;
using craftersmine.SteamGridDBNet.Exceptions;
using GameRecommendations.Services.Contracts;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace GameRecommendations.Services;

public class UrlImageLoader : IUrlImageLoader
{
    private readonly SteamGridDb _steamGridDb;

    public UrlImageLoader(IConfiguration configuration)
    {
        _steamGridDb = new SteamGridDb(configuration["SteamGridDbApiKey"]);
    }

    public async Task<string> GetImageUrlAsync(int appId)
    {
        try
        {
            var game = await _steamGridDb.GetGameBySteamIdAsync(appId);
            var grid = (await _steamGridDb.GetGridsByGameIdAsync(game.Id, dimensions: SteamGridDbDimensions.W600H900)).FirstOrDefault();

            return grid?.FullImageUrl ?? Resources.ImageNotFoundUrl;
        }
        catch (SteamGridDbException)
        {
            return Resources.ImageNotFoundUrl;
        }
    }
}
