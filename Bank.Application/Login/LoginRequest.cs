using Bank.Shared.Result;
using MediatR;

namespace Bank.Application.Login;

public record LoginRequest : IRequest<Result<LoginResponse>>
{
}
