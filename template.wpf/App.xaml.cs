using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using template.Application.Common.Interfaces;
using template.Application.UseCases;
using template.Infrastructure.Services;
using template.wpf.Common;
using template.wpf.ViewModels;

namespace template.wpf;

public partial class App : System.Windows.Application
{
    private ServiceProvider? _serviceProvider;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var services = new ServiceCollection();

        services.AddSingleton<ICurrentUserService, CurrentUserService>();
        services.AddTransient<ILoginUseCase, LoginUseCase>();

        services.AddTransient<HomeViewModel>();
        services.AddTransient<SettingsViewModel>();
        services.AddSingleton<MainViewModel>();

        services.AddSingleton<Func<Type, BaseViewModel>>(sp => type => (BaseViewModel)sp.GetRequiredService(type));
        services.AddSingleton<INavigationService, NavigationService>(sp =>
            new NavigationService(sp.GetRequiredService<Func<Type, BaseViewModel>>(), maxCacheSize: 5));

        services.AddSingleton<MainWindow>(sp => new MainWindow
        {
            DataContext = sp.GetRequiredService<MainViewModel>()
        });

        _serviceProvider = services.BuildServiceProvider();

        var navService = _serviceProvider.GetRequiredService<INavigationService>();
        navService.NavigateTo<HomeViewModel>();

        var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
        mainWindow.Show();
    }

    protected override void OnExit(ExitEventArgs e)
    {
        _serviceProvider?.Dispose();
        base.OnExit(e);
    }
}
