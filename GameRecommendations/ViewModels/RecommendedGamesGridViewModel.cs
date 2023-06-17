using GameRecomendations.RecomendationSystem.Contracts;
using GameRecommendations.Services.Contracts;
using GameRecommendations.Views.Pages;
using System.Linq;

namespace GameRecommendations.ViewModels;

public class RecommendedGamesGridViewModel : GamesGridViewModelBase
{
    public RecommendedGamesGridViewModel(IUrlImageLoader urlImageLoader, IDataLoader dataLoader, IRecommender recommender, INavigationService navigationService)
        : base(urlImageLoader)
    {
        navigationService.OnPageChanged += async (page) =>
        {
            if (page is RecommendedVideoGamesPage)
            {
                var likedVideoGames = dataLoader.GetLoadedData().Where(g => g.IsLiked);

                var recommendedGames = await recommender.RecommendVideoGamesAsync(likedVideoGames);

                _videoGamesToView = recommendedGames.Where(g => !g.IsLiked).ToList();

                await LoadVideoGamesPageAsync(1);
            }
        };
    }
}
