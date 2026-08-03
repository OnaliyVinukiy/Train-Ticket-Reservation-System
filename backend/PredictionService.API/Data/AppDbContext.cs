using Microsoft.EntityFrameworkCore;
using PredictionService.API.Models;
using Route = PredictionService.API.Models.Route;

namespace PredictionService.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(
        DbContextOptions<AppDbContext> options)
        : base(options)
    {

    }


    public DbSet<Booking> Bookings { get; set; }

    public DbSet<Route> Routes { get; set; }

    public DbSet<Schedule> Schedules { get; set; }

    public DbSet<SpecialRequest> SpecialRequests { get; set; }


    protected override void OnModelCreating(
        ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<Booking>()
            .HasOne(b => b.Route)
            .WithMany()
            .HasForeignKey(b => b.RouteId);


        modelBuilder.Entity<Booking>()
            .HasOne(b => b.Schedule)
            .WithMany()
            .HasForeignKey(b => b.ScheduleId);


        modelBuilder.Entity<SpecialRequest>()
            .HasOne(x => x.Booking)
            .WithMany(x => x.SpecialRequests)
            .HasForeignKey(x => x.BookingId);

    }
}