using API.Application.Login;
using API.Shared.Responses;
using API.Structure.OutputPort;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Structure.Controllers;

[ApiController]
[Route("[controller]")]
[AllowAnonymous]
public class LoginController(IMediator mediator) : ControllerBase
{
    [HttpGet(Name = "Login")]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(CustomProblemResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var port = new OutPutPortPresenter<LoginResponse>(this);
        var result = await mediator.Send(new LoginRequest(), cancellationToken);
        return port.Responder(result);
    }
}
