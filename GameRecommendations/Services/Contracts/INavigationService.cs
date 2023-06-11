using System;
using System.Windows.Controls;

namespace GameRecommendations.Services.Contracts;

public interface INavigationService
{
    event Action<Page> OnPageChanged;

    void ChangePage(Page page);
}
