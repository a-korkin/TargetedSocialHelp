namespace Application.Models.Helpers;

public class ResourceParameters
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public string Filter { get; set; } = string.Empty;
    public string Sort { get; set; } = string.Empty;
}