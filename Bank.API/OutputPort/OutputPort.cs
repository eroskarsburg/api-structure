using Bank.Shared.Errors;
using Bank.Shared.Responses;
using Bank.Shared.Result;
using Microsoft.AspNetCore.Mvc;

namespace Bank.API.OutputPort;

public class OutputPort<T>(ControllerBase controller) : IOutputPort<T>
{
    private readonly ControllerBase _controller = controller;
    private readonly string _instanciaBase = controller.HttpContext.Request.Path;

    public IActionResult Success(T data) => _controller.Ok(data);

    public IActionResult BadRequest(IEnumerable<Error> erros)
    {
        var errosPorCampo = erros
            .Where(e => e.Field != null)
            .GroupBy(e => e.Field!)
            .ToDictionary(g => g.Key, g => g.Select(e => e.Message).ToArray());
        var problema = new CustomProblemResponse
        {
            Tipo = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            Titulo = "Requisicao invalida",
            Status = 400,
            Detail = string.Join("; ", erros.Select(e => e.Message)),
            Instancia = _instanciaBase,
            Erros = errosPorCampo.Count > 0 ? errosPorCampo : null
        };
        return _controller.BadRequest(problema);
    }

    public IActionResult Fail(Error erro)
    {
        var problema = new CustomProblemResponse
        {
            Tipo = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            Titulo = erro.Code,
            Status = 400,
            Detail = erro.Message,
            Instancia = _instanciaBase,
            Codigo = erro.Code
        };
        return _controller.BadRequest(problema);
    }

    public IActionResult Response(Result<T> result)
    {
        return result switch
        {
            Success<T> s => Success(s.Value),
            Fail<T> f => Fail(f.Erro),
            BadRequest<T> r => BadRequest(r.Erros),
            _ => _controller.StatusCode(500)
        };
    }
}
