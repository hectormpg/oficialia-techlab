using Microsoft.UI.Xaml.Controls;
using Spike001.WinUI.ViewModels;

namespace Spike001.WinUI.Views;

public sealed partial class HomePage : Page
{
    public HomePage(HomeViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
