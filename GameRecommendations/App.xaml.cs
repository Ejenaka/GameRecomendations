using GameRecomendations.RecomendationSystem.Contracts;
using GameRecomendations.RecomendationSystem.Services;
using GameRecommendations.Helpers;
using GameRecommendations.Services;
using GameRecommendations.Services.Contracts;
using GameRecommendations.ViewModels;
using GameRecommendations.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Windows;
using Microsoft.Extensions.Configuration;

namespace GameRecommendations;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public static IHost AppHost { get; private set; }

    public App()
    {
        AppHost = Host.CreateDefaultBuilder()
            .ConfigureServices((contex, services) =>
            {
                // Add configuration to DI
                services.AddSingleton(contex.Configuration);
                
                services.AddSingleton<MainWindow>();

                // ViewModels
                services.AddSingleton<MainViewModel>();
                services.AddSingleton<NavigationBarViewModel>();
                services.AddSingleton<GamesGridViewModel>();
                services.AddSingleton<RecommendedGamesGridViewModel>();

                // Services
                services.AddSingleton<INavigationService, NavigationService>();
                services.AddSingleton<IDataLoader, FileDataLoader>();
                services.AddSingleton<IDataProcessor<string[][]>, DataProcessor>();
                services.AddSingleton<IRecommender, VideoGamesRecommender>();
                services.AddSingleton<IUrlImageLoader, UrlImageLoader>();
            })
            .Build();

        ViewModelLocator.Init(AppHost.Services);
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        await AppHost.StartAsync();

        var mainWindow = AppHost.Services.GetRequiredService<MainWindow>();
        mainWindow.Show();

        base.OnStartup(e);
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        await AppHost.StopAsync();

        base.OnExit(e);
    }
}
