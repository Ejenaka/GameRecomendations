using GameRecommendations.Services.Contracts;
using GameRecommendations.Views.Pages;

namespace GameRecommendations.ViewModels;

public class NavigationBarViewModel : ViewModelBase
{
    private readonly INavigationService _navigationService;
    private int _selectedPageIndex;

    public NavigationBarViewModel(INavigationService navigationService)
    {
        _navigationService = navigationService;
    }

    public int SelectedPageIndex
    {
        get => _selectedPageIndex;
        set
        {
            SetProperty(ref _selectedPageIndex, value);

            switch (value)
            {
                case 0:
                    _navigationService.ChangePage(new VideoGamesPage());
                    break;
                case 1:
                    _navigationService.ChangePage(new RecommendedVideoGamesPage());
                    break;
                default:
                    break;
            }
        }
    }
}
