using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Spike001.WinUI.Services.Navigation;
using Spike001.WinUI.ViewModels;
using Spike001.WinUI.Views;

namespace Spike001.WinUI;

public partial class App : Application
{
    private readonly IServiceProvider _serviceProvider;
    private Window? _window;

    public App()
    {
        InitializeComponent();
        _serviceProvider = ConfigureServices();
    }

    protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
    {
        _window = _serviceProvider.GetRequiredService<MainWindow>();
        _window.Activate();
    }

    private static IServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();

        services.AddSingleton<HomeViewModel>();
        services.AddSingleton<PlatformViewModel>();
        services.AddSingleton<TestsViewModel>();
        services.AddSingleton<MainViewModel>();

        services.AddTransient<HomePage>();
        services.AddTransient<PlatformPage>();
        services.AddTransient<TestsPage>();
        services.AddSingleton<Func<HomePage>>(serviceProvider =>
            () => serviceProvider.GetRequiredService<HomePage>());
        services.AddSingleton<Func<PlatformPage>>(serviceProvider =>
            () => serviceProvider.GetRequiredService<PlatformPage>());
        services.AddSingleton<Func<TestsPage>>(serviceProvider =>
            () => serviceProvider.GetRequiredService<TestsPage>());

        services.AddSingleton<NavigationService>();
        services.AddSingleton<INavigationService>(serviceProvider =>
            serviceProvider.GetRequiredService<NavigationService>());
        services.AddSingleton<MainWindow>();

        return services.BuildServiceProvider();
    }
}
