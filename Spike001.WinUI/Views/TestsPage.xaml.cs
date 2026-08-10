using Microsoft.UI.Xaml.Controls;
using Spike001.WinUI.ViewModels;

namespace Spike001.WinUI.Views;

public sealed partial class TestsPage : Page
{
    public TestsPage(TestsViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
