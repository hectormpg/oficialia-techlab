using Microsoft.UI.Xaml;
using Spike001.WinUI.Models;
using Spike001.WinUI.Services.Navigation;
using Spike001.WinUI.ViewModels;

namespace Spike001.WinUI;

public sealed partial class MainWindow : Window
{
    private readonly NavigationService _navigationService;

    public MainWindow(NavigationService navigationService, MainViewModel viewModel)
    {
        _navigationService = navigationService;
        InitializeComponent();
        RootLayout.DataContext = viewModel;
        _navigationService.Attach(ContentFrame);
        _navigationService.Navigate(NavigationDestination.Home);
    }
}
