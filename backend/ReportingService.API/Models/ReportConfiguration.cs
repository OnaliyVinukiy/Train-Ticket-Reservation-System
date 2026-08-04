namespace ReportingService.API.Models;


public class ReportConfiguration
{
    public string CompanyName { get; set; } = "";

    public string Currency { get; set; } = "";

    public string DefaultExportFormat { get; set; } = "";

    public List<string> Reports { get; set; } = new();
}