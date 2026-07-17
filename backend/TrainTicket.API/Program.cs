using TrainTicket.API.Data;
using TrainTicket.API.Models;
using TrainTicket.API.Repositories;
using TrainTicket.API.Services;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

builder.Services.AddSingleton<IRepository<Booking>, MemoryRepository<Booking>>();

builder.Services.AddSingleton<IRepository<Schedule>, MemoryRepository<Schedule>>();

builder.Services.AddSingleton<IRepository<SpecialRequest>, MemoryRepository<SpecialRequest>>();

builder.Services.AddSingleton<BookingService>();

builder.Services.AddSingleton<ScheduleService>();

builder.Services.AddSingleton<SpecialRequestService>();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var bookingRepo = scope.ServiceProvider.GetRequiredService<IRepository<Booking>>();
    var scheduleRepo = scope.ServiceProvider.GetRequiredService<IRepository<Schedule>>();
    var requestRepo = scope.ServiceProvider.GetRequiredService<IRepository<SpecialRequest>>();

    SeedData.Initialize(bookingRepo, scheduleRepo, requestRepo);
}


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.MapControllers();

app.Run();