namespace Bank.Shared.Errors;

public class Error
{
    public string Code { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string? Field { get; set; }

    public static Error Create(string code, string message, string? field = null) =>
        new() { Code = code, Message = message, Field = field };
}
