using CommunityToolkit.Mvvm.Input;
using GameRecomendations.RecomendationSystem.Contracts;
using GameRecomendations.Shared;
using GameRecommendations.Services.Contracts;
using GameRecommendations.Views.Pages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GameRecommendations.ViewModels;

public class GamesGridViewModel : GamesGridViewModelBase
{
    private readonly IDataLoader _dataLoader;
    private string _searchQuery;
    private string _filterQuery;
    private bool _viewLiked = false;
    private List<VideoGame> _allVideoGames;
    private ObservableCollection<string> _allTags;
    private ObservableCollection<string> _selectedTags = new();

    public GamesGridViewModel(IDataLoader dataLoader, IUrlImageLoader urlImageLoader, INavigationService navigationService)
        : base(urlImageLoader)
    {
        _dataLoader = dataLoader;

        AddFilterCommand = new RelayCommand(async () => await AddFilterAsync(FilterQuery), () => !string.IsNullOrEmpty(FilterQuery));
        RemoveFilterCommand = new RelayCommand<string>(async (filter) => await RemoveFilterAsync(filter));
        ToggleViewLikedVideoGamesCommand = new RelayCommand(async () => await ToggleViewLikedVideoGamesAsync());
        SearchVideoGamesCommand = new RelayCommand(async () =>
        {
            if (string.IsNullOrEmpty(SearchQuery))
            {
                await ClearSearchAsync();
            }
            else
            {
                await TrySearchAndApplyFiltersAsync();
            }
        });


        navigationService.OnPageChanged += async (page) =>
        {
            if (page is VideoGamesPage)
            {
                if (_allVideoGames == null)
                {
                    await LoadGamesAsync();

                    _videoGamesToView = _allVideoGames;

                    await LoadVideoGamesPageAsync(1);

                    AllTags = new ObservableCollection<string>(dataLoader.LoadVideoGamesTags(_allVideoGames));
                }
            }
        };
    }

    public ObservableCollection<string> AllTags
    {
        get => _allTags;
        set => SetProperty(ref _allTags, value);
    }

    public ObservableCollection<string> SelectedTags
    {
        get => _selectedTags;
        set => SetProperty(ref _selectedTags, value);
    }

    public string SearchQuery
    {
        get => _searchQuery;
        set => SetProperty(ref _searchQuery, value);
    }

    public string FilterQuery
    {
        get => _filterQuery;
        set
        {
            SetProperty(ref _filterQuery, value);

            ((RelayCommand)AddFilterCommand).NotifyCanExecuteChanged();
        }
    }

    public bool ViewLikedVideoGames
    {
        get => _viewLiked;
        set => SetProperty(ref _viewLiked, value);
    }

    public ICommand SearchVideoGamesCommand { get; }

    public ICommand ClearSearchCommand { get; }

    public ICommand AddFilterCommand { get; }

    public ICommand RemoveFilterCommand { get; }

    public ICommand ToggleViewLikedVideoGamesCommand { get; }

    private async Task LoadGamesAsync()
    {
        var sourceFile = "./Data/games.csv";

        _allVideoGames = await _dataLoader.LoadDataAsync(sourceFile);

        TotalPages = CalculateTotalPages(_allVideoGames.Count);
    }

    private async Task TrySearchAndApplyFiltersAsync()
    {
        if (!string.IsNullOrEmpty(SearchQuery))
        {
            var searchedGames = _allVideoGames.Where(g => g.Name.Contains(SearchQuery, StringComparison.InvariantCultureIgnoreCase));

            _videoGamesToView = ApplyFilters(searchedGames).ToList();
        }
        else
        {
            _videoGamesToView = ApplyFilters(_allVideoGames).ToList();
        }

        await LoadVideoGamesPageAsync(1);
    }

    private IEnumerable<VideoGame> ApplyFilters(IEnumerable<VideoGame> videoGames)
    {
        if (ViewLikedVideoGames)
        {
            videoGames = videoGames.Where(g => g.IsLiked);
        }

        if (SelectedTags.Count > 0)
        {
            videoGames = videoGames.Where(g => SelectedTags.All(tag => g.PopularTags.Contains(tag)));
        }

        return videoGames;
    }

    private async Task ClearSearchAsync()
    {
        _videoGamesToView = ApplyFilters(_allVideoGames).ToList();

        await LoadVideoGamesPageAsync(1);
    }

    private async Task AddFilterAsync(string filter)
    {
        SelectedTags.Add(filter);
        AllTags.Remove(filter);

        _videoGamesToView = _videoGamesToView.Where(g => SelectedTags.All(tag => g.PopularTags.Contains(tag))).ToList();

        await LoadVideoGamesPageAsync(1);
    }

    private async Task RemoveFilterAsync(string filter)
    {
        SelectedTags.Remove(filter);
        AllTags.Add(filter);
        AllTags = new ObservableCollection<string>(AllTags.OrderBy(x => x));

        await TrySearchAndApplyFiltersAsync();
    }

    private async Task ToggleViewLikedVideoGamesAsync()
    {
        if (ViewLikedVideoGames)
        {
            _videoGamesToView = _videoGamesToView.Where(g => g.IsLiked).ToList();

            await LoadVideoGamesPageAsync(1);
        }
        else
        {
            await TrySearchAndApplyFiltersAsync();
        }
    }
}
