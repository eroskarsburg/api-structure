using Bank.Shared.Result;
using MediatR;

namespace Bank.Application.Login;

public class LoginHandler : IRequestHandler<LoginRequest, Result<LoginResponse>>
{
    public Task<Result<LoginResponse>> Handle(LoginRequest request, CancellationToken cancellationToken)
    {
        return Task.FromResult(Result<LoginResponse>.Success(new LoginResponse("token-exemplo", DateTime.UtcNow.AddHours(1))));
    }
}
