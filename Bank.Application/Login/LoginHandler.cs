using Bank.Shared.Erros;
using Bank.Shared.Resultado;
using MediatR;

namespace Bank.Application.Login;

public class LoginHandler : IRequestHandler<LoginRequest, Result<LoginResponse>>
{
    public Task<Result<LoginResponse>> Handle(LoginRequest request, CancellationToken cancellationToken)
    {
        return Task.FromResult<Result<LoginResponse>>(
            Result<LoginResponse>.Success(
                new LoginResponse("token-exemplo", DateTime.UtcNow.AddHours(1))));
    }
}
