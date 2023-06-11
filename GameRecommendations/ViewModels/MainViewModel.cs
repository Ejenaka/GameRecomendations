using GameRecommendations.Services.Contracts;
using GameRecommendations.Views.Pages;
using System.Windows.Controls;

namespace GameRecommendations.ViewModels;

public class MainViewModel : ViewModelBase
{
    public MainViewModel(INavigationService navigationService)
    {
        navigationService.OnPageChanged += (page) => CurrentPage = page;

        navigationService.ChangePage(new VideoGamesPage());
    }

    public Page CurrentPage { get; set; }
}