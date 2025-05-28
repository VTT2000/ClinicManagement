public class SearchServiceParaclinicalSelectedVM
{
    public int ServiceID { get; set; }
    public string? ServiceName { get; set; }
    public List<SearchServiceParaclinicalSelectedVM> ServiceChildren { get; set; } = new List<SearchServiceParaclinicalSelectedVM>();
}