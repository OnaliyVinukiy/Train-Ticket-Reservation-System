using Microsoft.EntityFrameworkCore;
using BookingService.API.Models;
using Route = BookingService.API.Models.Route;


namespace BookingService.API.Data;


public class AppDbContext : DbContext
{

    public AppDbContext(
        DbContextOptions<AppDbContext> options
    ) : base(options)
    {

    }


    public DbSet<Booking> Bookings { get; set; }

    public DbSet<Route> Routes { get; set; }

    public DbSet<Schedule> Schedules { get; set; }

    public DbSet<SpecialRequest> SpecialRequests { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Booking>()
            .HasOne(b => b.Route)
            .WithMany(r => r.Bookings)
            .HasForeignKey(b => b.RouteId);

        modelBuilder.Entity<Booking>()
            .HasOne(b => b.Schedule)
            .WithMany(s => s.Bookings)
            .HasForeignKey(b => b.ScheduleId);

        modelBuilder.Entity<SpecialRequest>()
            .HasOne(s => s.Booking)
            .WithMany(b => b.SpecialRequests)
            .HasForeignKey(s => s.BookingId);

        modelBuilder.Entity<Booking>()
            .Property(b => b.TicketPrice)
            .HasPrecision(18, 2);
    }
}