namespace Bank.Shared.Errors;

public class Error
{
    public string Codigo { get; set; } = string.Empty;
    public string Mensagem { get; set; } = string.Empty;
    public string? Campo { get; set; }

    public static Error Criar(string codigo, string mensagem, string? campo = null) =>
        new() { Codigo = codigo, Mensagem = mensagem, Campo = campo };
}
