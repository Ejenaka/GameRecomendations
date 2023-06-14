using CommunityToolkit.Mvvm.Input;
using GameRecomendations.Shared;
using GameRecommendations.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GameRecommendations.ViewModels;

public abstract class GamesGridViewModelBase : ViewModelBase
{
    private readonly IUrlImageLoader _urlImageLoader;
    private readonly int _itemsPerPage = 24;
    private int _totalPages;
    private int _currentPage;

    protected List<VideoGame> _videoGamesToView;
    protected ObservableCollection<VideoGame> _pagedVideoGames;

    protected GamesGridViewModelBase(IUrlImageLoader urlImageLoader)
    {
        _urlImageLoader = urlImageLoader;

        NextPageCommand = new RelayCommand(async () => await NextPageAsync(), () => CurrentPage < TotalPages);
        PreviousPageCommand = new RelayCommand(async () => await PreviousPageAsync(), () => CurrentPage > 1);
    }

    public ObservableCollection<VideoGame> PagedVideoGames
    {
        get => _pagedVideoGames;
        set => SetProperty(ref _pagedVideoGames, value);
    }

    public int CurrentPage
    {
        get => _currentPage;
        set => SetProperty(ref _currentPage, value);
    }

    public int TotalPages
    {
        get => _totalPages;
        set
        {
            SetProperty(ref _totalPages, value);

            ((RelayCommand)NextPageCommand).NotifyCanExecuteChanged();
            ((RelayCommand)PreviousPageCommand).NotifyCanExecuteChanged();
        }
    }

    public ICommand NextPageCommand { get; }

    public ICommand PreviousPageCommand { get; }

    protected int CalculateTotalPages(int itemsCount) => (int)Math.Ceiling(itemsCount / (double)_itemsPerPage);

    protected async Task LoadVideoGamesPageAsync(int page)
    {
        CurrentPage = page;

        var toSkip = (_currentPage - 1) * _itemsPerPage;
        var toTake = _videoGamesToView.Count - toSkip < _itemsPerPage ? _videoGamesToView.Count - toSkip : _itemsPerPage;

        var pagedVideoGames = _videoGamesToView.GetRange(toSkip, toTake);

        var loadImagesTasks = pagedVideoGames.Select(async game =>
        {
            if (string.IsNullOrEmpty(game.ImageUrl))
            {
                game.ImageUrl = await _urlImageLoader.GetImageUrlAsync(game.AppId);
            }
        });

        await Task.WhenAll(loadImagesTasks);

        TotalPages = CalculateTotalPages(_videoGamesToView.Count);
        PagedVideoGames = new ObservableCollection<VideoGame>(pagedVideoGames);
    }

    private async Task NextPageAsync()
    {
        await LoadVideoGamesPageAsync(CurrentPage + 1);
    }

    private async Task PreviousPageAsync()
    {
        if (CurrentPage > 1)
        {
            await LoadVideoGamesPageAsync(CurrentPage - 1);
        }
    }
}
