using TrainTicket.API.Data;
using TrainTicket.API.Models;
using TrainTicket.API.Repositories;
using TrainTicket.API.Services;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReact",
        policy =>
        {
            policy
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});


builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(
            new JsonStringEnumConverter()
        );
    });

builder.Services.AddSingleton<IRepository<Booking>, MemoryRepository<Booking>>();

builder.Services.AddSingleton<IRepository<Schedule>, MemoryRepository<Schedule>>();

builder.Services.AddSingleton<IRepository<SpecialRequest>, MemoryRepository<SpecialRequest>>();

builder.Services.AddSingleton<BookingService>();

builder.Services.AddSingleton<ScheduleService>();

builder.Services.AddSingleton<SpecialRequestService>();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors("AllowReact");

using (var scope = app.Services.CreateScope())
{
    var bookingRepository =
        scope.ServiceProvider
        .GetRequiredService<IRepository<Booking>>();


    var scheduleRepository =
        scope.ServiceProvider
        .GetRequiredService<IRepository<Schedule>>();


    var requestRepository =
        scope.ServiceProvider
        .GetRequiredService<IRepository<SpecialRequest>>();


    SeedData.Initialize(
        bookingRepository,
        scheduleRepository,
        requestRepository);
}


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.MapControllers();

app.Run();