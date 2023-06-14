using GameRecomendations.RecomendationSystem.Contracts;
using GameRecommendations.Services.Contracts;
using GameRecommendations.Views.Pages;
using System.Linq;

namespace GameRecommendations.ViewModels;

public class RecommendedGamesGridViewModel : GamesGridViewModelBase
{
    private readonly IDataLoader _dataLoader;

    public RecommendedGamesGridViewModel(IUrlImageLoader urlImageLoader, IDataLoader dataLoader, IRecommender recommender, INavigationService navigationService)
        : base(urlImageLoader)
    {
        navigationService.OnPageChanged += async (page) =>
        {
            if (page is RecommendedVideoGamesPage)
            {
                var likedVideoGames = dataLoader.GetLoadedData().Where(g => g.IsLiked);

                _videoGamesToView = await recommender.RecommendVideoGamesAsync(likedVideoGames);

                await LoadVideoGamesPageAsync(1);
            }
        };
    }
}
