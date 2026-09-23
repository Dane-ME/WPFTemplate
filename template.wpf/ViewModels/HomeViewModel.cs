using template.Application.UseCases;
using template.wpf.Common;

namespace template.wpf.ViewModels;

public class HomeViewModel : BaseViewModel, INavigationAware
{
    private readonly ILoginUseCase _loginUseCase;
    private string _username = "DemoUser";
    private string _statusMessage = string.Empty;

    public string Username
    {
        get => _username;
        set => SetProperty(ref _username, value);
    }

    public string StatusMessage
    {
        get => _statusMessage;
        set => SetProperty(ref _statusMessage, value);
    }

    public RelayCommand LoginCommand { get; }

    public HomeViewModel(ILoginUseCase loginUseCase)
    {
        _loginUseCase = loginUseCase ?? throw new ArgumentNullException(nameof(loginUseCase));
        LoginCommand = new RelayCommand(_ => ExecuteLogin());
    }

    private void ExecuteLogin()
    {
        var result = _loginUseCase.Execute(Username, "password123");
        StatusMessage = result.IsSuccess ? result.Value! : result.Error!;
    }

    public void OnNavigatedTo(object? parameter)
    {
        if (parameter is string message)
        {
            StatusMessage = message;
        }
    }

    public bool CanNavigateFrom() => true;
}
