using CommunityToolkit.Mvvm.Input;
using Spike001.WinUI.Models;
using Spike001.WinUI.Services.Navigation;

namespace Spike001.WinUI.ViewModels;

public sealed partial class MainViewModel
{
    private readonly INavigationService _navigationService;

    public MainViewModel(INavigationService navigationService)
    {
        _navigationService = navigationService;
    }

    [RelayCommand]
    private void NavigateHome()
    {
        _navigationService.Navigate(NavigationDestination.Home);
    }

    [RelayCommand]
    private void NavigatePlatform()
    {
        _navigationService.Navigate(NavigationDestination.Platform);
    }

    [RelayCommand]
    private void NavigateTests()
    {
        _navigationService.Navigate(NavigationDestination.Tests);
    }
}
