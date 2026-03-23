using Bank.Shared.Errors;
using Bank.Shared.Result;
using Microsoft.AspNetCore.Mvc;

namespace Bank.API.OutputPort;

public interface IOutputPort<T>
{
    IActionResult Success(T data);
    IActionResult BadRequest(IEnumerable<Error> errors);
    IActionResult Fail(Error error);
    IActionResult Responder(Result<T> result);
}
