using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

using TrainTicket.API.Data;
using TrainTicket.API.Models;
using TrainTicket.API.Repositories;
using TrainTicket.API.Services;


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

        options.JsonSerializerOptions.ReferenceHandler =
            ReferenceHandler.IgnoreCycles;
    });


builder.Services.AddDbContext<AppDbContext>(
    options =>
    {
        options.UseSqlite(
            "Data Source=TrainTicket.db"
        );
    });


// Repository registrations

builder.Services.AddScoped<IRepository<Schedule>, EFRepository<Schedule>>();

builder.Services.AddScoped<IRepository<SpecialRequest>, EFRepository<SpecialRequest>>();

builder.Services.AddScoped<BookingRepository>();


// Services
builder.Services.AddScoped<BookingService>();

builder.Services.AddScoped<ScheduleService>();

builder.Services.AddScoped<SpecialRequestService>();

builder.Services.AddScoped<RecurringBookingService>();

builder.Services.AddScoped<ReportService>();

builder.Services.AddScoped<ExportService>();

builder.Services.AddScoped<ChatbotService>();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();


var app = builder.Build();

app.UseCors("AllowReact");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();