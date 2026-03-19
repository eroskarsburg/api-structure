using API.Shared.Erros;
using API.Shared.Resultado;
using MediatR;

namespace API.Application.Login;

public class LoginHandler : IRequestHandler<LoginRequest, Result<LoginResponse>>
{
    public Task<Result<LoginResponse>> Handle(LoginRequest request, CancellationToken cancellationToken)
    {
        return Task.FromResult<Result<LoginResponse>>(
            Result<LoginResponse>.Success(
                new LoginResponse("token-exemplo", DateTime.UtcNow.AddHours(1))));
    }
}
