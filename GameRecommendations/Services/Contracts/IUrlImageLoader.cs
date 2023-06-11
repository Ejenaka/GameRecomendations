using System.Threading.Tasks;

namespace GameRecommendations.Services.Contracts;
public interface IUrlImageLoader
{
    Task<string> GetImageUrlAsync(int appId);
}
