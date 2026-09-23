namespace template.wpf.Common;

public interface INavigationAware
{
    void OnNavigatedTo(object? parameter);
    Task OnNavigatedToAsync(object? parameter, CancellationToken cancellationToken) => Task.CompletedTask;
    bool CanNavigateFrom();
    Task<bool> CanNavigateFromAsync() => Task.FromResult(CanNavigateFrom());
}
