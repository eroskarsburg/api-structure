using API.Shared.Erros;
using API.Shared.Responses;
using API.Shared.Resultado;
using Microsoft.AspNetCore.Mvc;

namespace API.Structure.OutputPort;

public class OutPutPortPresenter<T> : IOutputPort<T>
{
    private readonly ControllerBase _controller;
    private readonly string _instanciaBase;

    public OutPutPortPresenter(ControllerBase controller)
    {
        _controller = controller;
        _instanciaBase = controller.HttpContext.Request.Path;
    }

    public IActionResult Success(T data) => _controller.Ok(data);

    public IActionResult BadRequest(IEnumerable<Error> erros)
    {
        var errosPorCampo = erros
            .Where(e => e.Campo != null)
            .GroupBy(e => e.Campo!)
            .ToDictionary(g => g.Key, g => g.Select(e => e.Mensagem).ToArray());
        var problema = new CustomProblemResponse
        {
            Tipo = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            Titulo = "Requisicao invalida",
            Status = 400,
            Detail = string.Join("; ", erros.Select(e => e.Mensagem)),
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
            Titulo = erro.Codigo,
            Status = 400,
            Detail = erro.Mensagem,
            Instancia = _instanciaBase,
            Codigo = erro.Codigo
        };
        return _controller.BadRequest(problema);
    }

    public IActionResult Responder(Result<T> result)
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
