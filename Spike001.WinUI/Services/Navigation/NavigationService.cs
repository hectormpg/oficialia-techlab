using System;
using Microsoft.UI.Xaml.Controls;
using Spike001.WinUI.Models;
using Spike001.WinUI.Views;

namespace Spike001.WinUI.Services.Navigation;

public sealed class NavigationService : INavigationService
{
    private readonly Func<HomePage> _homePageFactory;
    private readonly Func<PlatformPage> _platformPageFactory;
    private readonly Func<TestsPage> _testsPageFactory;
    private Frame? _frame;

    public NavigationService(
        Func<HomePage> homePageFactory,
        Func<PlatformPage> platformPageFactory,
        Func<TestsPage> testsPageFactory)
    {
        _homePageFactory = homePageFactory;
        _platformPageFactory = platformPageFactory;
        _testsPageFactory = testsPageFactory;
    }

    public void Attach(Frame frame)
    {
        _frame = frame;
    }

    public void Navigate(NavigationDestination destination)
    {
        if (_frame is null)
        {
            throw new InvalidOperationException("NavigationService must be attached to a Frame before navigation.");
        }

        _frame.Content = destination switch
        {
            NavigationDestination.Home => _homePageFactory(),
            NavigationDestination.Platform => _platformPageFactory(),
            NavigationDestination.Tests => _testsPageFactory(),
            _ => throw new ArgumentOutOfRangeException(nameof(destination), destination, "Unknown navigation destination.")
        };
    }
}
