using System.Text.Json.Serialization;

namespace ReportingService.API.Models;

public class Route
{
    public int Id { get; set; }

    public string DepartureStation { get; set; } = "";

    public string DestinationStation { get; set; } = "";

}