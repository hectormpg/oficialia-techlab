using Microsoft.UI.Xaml.Controls;
using Spike001.WinUI.ViewModels;

namespace Spike001.WinUI.Views;

public sealed partial class PlatformPage : Page
{
    public PlatformPage(PlatformViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
