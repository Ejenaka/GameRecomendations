using craftersmine.SteamGridDBNet;
using craftersmine.SteamGridDBNet.Exceptions;
using GameRecommendations.Services.Contracts;
using System.Linq;
using System.Threading.Tasks;

namespace GameRecommendations.Services;

public class UrlImageLoader : IUrlImageLoader
{
    private readonly SteamGridDb _steamGridDb;
    private readonly string _notFoundImageUrl = "https://static.vecteezy.com/system/resources/previews/005/720/408/original/crossed-image-icon-picture-not-available-delete-picture-symbol-free-vector.jpg";

    public UrlImageLoader()
    {
        _steamGridDb = new SteamGridDb("6cc986b4bbc47f5e792b5d5597a6883c");
    }

    public async Task<string> GetImageUrlAsync(int appId)
    {
        try
        {
            var game = await _steamGridDb.GetGameBySteamIdAsync(appId);
            var grid = (await _steamGridDb.GetGridsByGameIdAsync(game.Id, dimensions: SteamGridDbDimensions.W600H900)).FirstOrDefault();

            return grid?.FullImageUrl ?? _notFoundImageUrl;
        }
        catch (SteamGridDbException)
        {
            return _notFoundImageUrl;
        }
    }
}
