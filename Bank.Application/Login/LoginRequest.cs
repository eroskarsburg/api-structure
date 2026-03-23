using Bank.Shared.Resultado;
using MediatR;

namespace Bank.Application.Login;

public record LoginRequest : IRequest<Result<LoginResponse>>
{
}
