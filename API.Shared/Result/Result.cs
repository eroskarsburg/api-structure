using API.Shared.Errors;

namespace API.Shared.Result;

public abstract class Result<T>
{
    public static Result<T> Success(T value) => new Success<T>(value);
    public static Result<T> Fail(Error error) => new Fail<T>(error);
    public static Result<T> BadRequest(IEnumerable<Error> errors) => new BadRequest<T>(errors);
}

public class Success<T>(T value) : Result<T>
{
    public T Value { get; } = value;
}

public class Fail<T>(Error erro) : Result<T>
{
    public Error Erro { get; } = erro;
}

public class BadRequest<T>(IEnumerable<Error> erros) : Result<T>
{
    public IReadOnlyList<Error> Erros { get; } = erros.ToList();
}
