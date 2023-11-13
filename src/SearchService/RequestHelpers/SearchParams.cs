namespace SearchService;

public class SearchParams
{
    public string? SearchTerm { get; set; }
    public uint PageNumber { get; set; } = 1;
    public uint PageSize { get; set; } = 4;
    public string? Seller { get; set; }
    public string? Winner { get; set; }
    public string? OrderBy { get; set; }
    public string? FilterBy { get; set; }
}
