using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

using BookingService.API.Data;
using BookingService.API.Models;

using BookingService.API.Repositories;
using BookingService.API.Services;


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


builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"));
});


// Repository registrations

builder.Services.AddScoped<BookingRepository>();


// Services
builder.Services.AddScoped<BookingManagementService>();

builder.Services.AddScoped<ScheduleService>();

builder.Services.AddScoped<SpecialRequestService>();

builder.Services.AddScoped<RecurringBookingService>();

builder.Services.AddScoped<ExportService>();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    context.Database.Migrate();

    DataSeeder.Seed(context);
}

app.UseCors("AllowReact");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();