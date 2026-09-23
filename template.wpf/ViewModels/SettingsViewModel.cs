using template.wpf.Common;

namespace template.wpf.ViewModels;

public class SettingsViewModel : BaseViewModel, INavigationAware
{
    private bool _darkMode = true;
    private int _cacheLimit = 5;

    public bool DarkMode
    {
        get => _darkMode;
        set => SetProperty(ref _darkMode, value);
    }

    public int CacheLimit
    {
        get => _cacheLimit;
        set => SetProperty(ref _cacheLimit, value);
    }

    public void OnNavigatedTo(object? parameter)
    {
    }

    public bool CanNavigateFrom() => true;
}
