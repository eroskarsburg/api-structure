using API.Shared.Errors;
using API.Shared.Result;
using Microsoft.AspNetCore.Mvc;

namespace API.Structure.OutputPort;

public interface IOutputPort<T>
{
    IActionResult Success(T data);
    IActionResult BadRequest(IEnumerable<Error> errors);
    IActionResult Fail(Error error);
    IActionResult Responder(Result<T> result);
}
