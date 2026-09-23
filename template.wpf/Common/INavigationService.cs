namespace template.wpf.Common;

public interface INavigationService
{
    BaseViewModel? CurrentView { get; }
    bool CanGoBack { get; }
    event Action? CurrentViewChanged;

    void NavigateTo<TViewModel>(object? parameter = null) where TViewModel : BaseViewModel;
    Task NavigateToAsync<TViewModel>(object? parameter = null, CancellationToken cancellationToken = default) where TViewModel : BaseViewModel;
    void GoBack();
    Task GoBackAsync(CancellationToken cancellationToken = default);
    void ClearCache();
}
