namespace API.Shared.Responses;

public class CustomProblemResponse
{
    public string Tipo { get; set; } = string.Empty;
    public string Titulo { get; set; } = string.Empty;
    public int Status { get; set; }
    public string Detail { get; set; } = string.Empty;
    public string Instancia { get; set; } = string.Empty;
    public Dictionary<string, string[]>? Erros { get; set; }
    public string? Codigo { get; set; }
}
