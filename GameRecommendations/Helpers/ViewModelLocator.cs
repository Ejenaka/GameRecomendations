using GameRecommendations.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace GameRecommendations.Helpers;

public class ViewModelLocator
{
    private static IServiceProvider _serviceProvider;

    public static void Init(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public MainViewModel MainViewModel { get => _serviceProvider.GetRequiredService<MainViewModel>(); }
    public NavigationBarViewModel NavigationBarViewModel { get => _serviceProvider.GetRequiredService<NavigationBarViewModel>(); }
    public GamesGridViewModel GamesGridViewModel { get => _serviceProvider.GetRequiredService<GamesGridViewModel>(); }
    public RecommendedGamesGridViewModel RecommendedGamesGridViewModel { get => _serviceProvider.GetRequiredService<RecommendedGamesGridViewModel>(); }
}
