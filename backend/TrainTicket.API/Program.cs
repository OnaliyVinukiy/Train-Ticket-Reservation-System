using TrainTicket.API.Models;
using TrainTicket.API.Repositories;
using TrainTicket.API.Services;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

builder.Services.AddSingleton<IRepository<Booking>, MemoryRepository<Booking>>();

builder.Services.AddSingleton<BookingService>();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();


var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.MapControllers();

app.Run();