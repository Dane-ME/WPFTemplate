using template.Application.Common.Interfaces;

namespace template.Infrastructure.Services;

public class CurrentUserService : ICurrentUserService
{
    public string? UserName { get; private set; } = "Guest";
    public bool IsAuthenticated => !string.IsNullOrEmpty(UserName) && UserName != "Guest";

    public void SetUser(string username)
    {
        UserName = username;
    }
}
