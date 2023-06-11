using GameRecommendations.Services.Contracts;
using System;
using System.Windows.Controls;

namespace GameRecommendations.Services;

public class NavigationService : INavigationService
{
    public event Action<Page>? OnPageChanged;

    public void ChangePage(Page page) => OnPageChanged?.Invoke(page);
}
