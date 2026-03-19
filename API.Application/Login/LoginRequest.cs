using API.Shared.Resultado;
using MediatR;

namespace API.Application.Login;

public record LoginRequest : IRequest<Result<LoginResponse>>
{
}
