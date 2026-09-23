using template.Domain.Common;
using template.Application.Common.Interfaces;

namespace template.Application.UseCases;

public interface ILoginUseCase
{
    Result<string> Execute(string username, string password);
}

public class LoginUseCase : ILoginUseCase
{
    public Result<string> Execute(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            return Result<string>.Failure("Username and password are required.");
        }

        return Result<string>.Success($"Welcome, {username}!");
    }
}
