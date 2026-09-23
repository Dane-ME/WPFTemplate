using template.wpf.Common;

namespace template.wpf.ViewModels;

public class MainViewModel : BaseViewModel
{
    private readonly INavigationService _navigationService;

    public BaseViewModel? CurrentView => _navigationService.CurrentView;
    public bool CanGoBack => _navigationService.CanGoBack;

    public RelayCommand NavigateHomeCommand { get; }
    public RelayCommand NavigateSettingsCommand { get; }
    public RelayCommand GoBackCommand { get; }

    public MainViewModel(INavigationService navigationService)
    {
        _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
        _navigationService.CurrentViewChanged += OnCurrentViewChanged;

        NavigateHomeCommand = new RelayCommand(_ => _navigationService.NavigateTo<HomeViewModel>());
        NavigateSettingsCommand = new RelayCommand(_ => _navigationService.NavigateTo<SettingsViewModel>());
        GoBackCommand = new RelayCommand(_ => _navigationService.GoBack(), _ => _navigationService.CanGoBack);
    }

    private void OnCurrentViewChanged()
    {
        OnPropertyChanged(nameof(CurrentView));
        OnPropertyChanged(nameof(CanGoBack));
    }
}
