namespace Application.Models.Helpers;

public class ResourceParameters
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public string Search { get; set; } = string.Empty;
    public string Ordered { get; set; } = string.Empty;
}