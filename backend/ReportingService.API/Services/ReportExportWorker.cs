using System.Collections.Concurrent;
using CsvHelper;
using System.Globalization;
using ReportingService.API.Models;
using ReportingService.API.Services;

namespace ReportingService.API.Services;

public class ReportExportWorker : BackgroundService
{
    private readonly ConcurrentQueue<ReportExportJob> jobs = new();

    private readonly ConcurrentDictionary<Guid, ReportExportJob> jobResults = new();

    private readonly IServiceScopeFactory scopeFactory;


    public ReportExportWorker(
        IServiceScopeFactory scopeFactory)
    {
        this.scopeFactory = scopeFactory;
    }



    public Guid QueueExport()
    {
        var job = new ReportExportJob
        {
            JobId = Guid.NewGuid(),

            Status = "Queued",

            CreatedAt = DateTime.UtcNow
        };


        jobs.Enqueue(job);

        jobResults.TryAdd(
            job.JobId,
            job);


        return job.JobId;
    }




    public ReportExportJob? GetStatus(Guid id)
    {
        jobResults.TryGetValue(
            id,
            out var job);

        return job;
    }




    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {

        while (!stoppingToken.IsCancellationRequested)
        {

            if (jobs.TryDequeue(out var job))
            {
                await ProcessExport(job);
            }


            await Task.Delay(
                1000,
                stoppingToken);
        }

    }





    private async Task ProcessExport(
        ReportExportJob job)
    {

        try
        {

            job.Status = "Processing";


            using var scope =
                scopeFactory.CreateScope();


            var reportService =
                scope.ServiceProvider
                .GetRequiredService<ReportService>();


            var bookings =
                reportService.GetBookingReport(
                    DateTime.UtcNow.AddDays(-30),
                    DateTime.UtcNow,
                    null,
                    null);



            Directory.CreateDirectory(
                "Reports");


            var fileName =
                $"booking-report-{job.JobId}.csv";


            var filePath =
                Path.Combine(
                    "Reports",
                    fileName);



            await using var stream =
                new FileStream(
                    filePath,
                    FileMode.Create,
                    FileAccess.Write,
                    FileShare.None,
                    4096,
                    true);



            await using var writer =
                new StreamWriter(stream);



            using var csv =
                new CsvWriter(
                    writer,
                    CultureInfo.InvariantCulture);



            csv.WriteRecords(
                bookings.Select(b => new
                {

                    BookingId = b.Id,

                    Reference = b.BookingReference,


                    Route =
                    $"{b.Route.DepartureStation} → {b.Route.DestinationStation}",


                    TravelDate =
                    b.Schedule.TravelDate
                    .ToShortDateString(),


                    Departure =
                    b.Schedule.DepartureTime,


                    Arrival =
                    b.Schedule.ArrivalTime,


                    Seat =
                    b.SeatNumber,


                    Price =
                    b.TicketPrice,


                    Type =
                    b.BookingType.ToString(),


                    Requests =
                    string.Join(", ",
                    b.SpecialRequests
                    .Select(x => x.Description))

                }));


            await writer.FlushAsync();



            job.FilePath = filePath;

            job.Status = "Completed";

            job.CompletedAt =
                DateTime.UtcNow;

        }
        catch(Exception ex)
        {

            job.Status =
                "Failed: " + ex.Message;

        }

    }
    public string? GetFilePath(Guid id)
{
    if (jobResults.TryGetValue(id, out var job))
    {
        if (job.Status == "Completed")
        {
            return job.FilePath;
        }
    }

    return null;
}
}