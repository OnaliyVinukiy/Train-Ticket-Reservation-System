namespace ReportingService.API.Models;

public class ReportExportJob
{
    public Guid JobId { get; set; }

    public string Status { get; set; } = "Queued";

    public string? FilePath { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? CompletedAt { get; set; }
}