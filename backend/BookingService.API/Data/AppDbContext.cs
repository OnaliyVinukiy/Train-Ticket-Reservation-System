using Microsoft.EntityFrameworkCore;
using TrainTicket.API.Models;
using Route = TrainTicket.API.Models.Route;


namespace TrainTicket.API.Data;


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



    protected override void OnModelCreating(
        ModelBuilder modelBuilder
    )
    {

        modelBuilder.Entity<Booking>()
            .HasOne(b => b.Route)
            .WithMany()
            .HasForeignKey(b => b.RouteId)
            .OnDelete(DeleteBehavior.Cascade);



        modelBuilder.Entity<Booking>()
            .HasOne(b => b.Schedule)
            .WithMany()
            .HasForeignKey(b => b.ScheduleId)
            .OnDelete(DeleteBehavior.Cascade);



        modelBuilder.Entity<SpecialRequest>()
            .HasOne(r => r.Booking)
            .WithMany(b => b.SpecialRequests)
            .HasForeignKey(r => r.BookingId)
            .OnDelete(DeleteBehavior.Cascade);

    }

}