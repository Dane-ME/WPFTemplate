namespace template.Application.Common.Interfaces;

public interface ICurrentUserService
{
    string? UserName { get; }
    bool IsAuthenticated { get; }
}
