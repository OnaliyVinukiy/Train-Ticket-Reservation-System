using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TrainTicket.API.Data;

#nullable disable

namespace TrainTicket.API.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "10.0.10");

            modelBuilder.Entity("TrainTicket.API.Models.Booking", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("BookingReference")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("BookingType")
                        .HasColumnType("INTEGER");

                    b.Property<int>("RouteId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("RouteId1")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ScheduleId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("ScheduleId1")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SeatNumber")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<decimal>("TicketPrice")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("RouteId");

                    b.HasIndex("RouteId1");

                    b.HasIndex("ScheduleId");

                    b.HasIndex("ScheduleId1");

                    b.ToTable("Bookings");
                });

            modelBuilder.Entity("TrainTicket.API.Models.Route", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("DepartureStation")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("DestinationStation")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Routes");
                });

            modelBuilder.Entity("TrainTicket.API.Models.Schedule", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<TimeSpan>("ArrivalTime")
                        .HasColumnType("TEXT");

                    b.Property<TimeSpan>("DepartureTime")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("TravelDate")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Schedules");
                });

            modelBuilder.Entity("TrainTicket.API.Models.SpecialRequest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("BookingId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("BookingId");

                    b.ToTable("SpecialRequests");
                });

            modelBuilder.Entity("TrainTicket.API.Models.Booking", b =>
                {
                    b.HasOne("TrainTicket.API.Models.Route", "Route")
                        .WithMany()
                        .HasForeignKey("RouteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TrainTicket.API.Models.Route", null)
                        .WithMany("Bookings")
                        .HasForeignKey("RouteId1");

                    b.HasOne("TrainTicket.API.Models.Schedule", "Schedule")
                        .WithMany()
                        .HasForeignKey("ScheduleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TrainTicket.API.Models.Schedule", null)
                        .WithMany("Bookings")
                        .HasForeignKey("ScheduleId1");

                    b.Navigation("Route");

                    b.Navigation("Schedule");
                });

            modelBuilder.Entity("TrainTicket.API.Models.SpecialRequest", b =>
                {
                    b.HasOne("TrainTicket.API.Models.Booking", "Booking")
                        .WithMany("SpecialRequests")
                        .HasForeignKey("BookingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Booking");
                });

            modelBuilder.Entity("TrainTicket.API.Models.Booking", b =>
                {
                    b.Navigation("SpecialRequests");
                });

            modelBuilder.Entity("TrainTicket.API.Models.Route", b =>
                {
                    b.Navigation("Bookings");
                });

            modelBuilder.Entity("TrainTicket.API.Models.Schedule", b =>
                {
                    b.Navigation("Bookings");
                });
        }
    }
}
