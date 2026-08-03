using Microsoft.EntityFrameworkCore;
using PredictionService.API.Data;
using PredictionService.API.Services;


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


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<AppDbContext>(
    options =>
        options.UseSqlServer(
            builder.Configuration
            .GetConnectionString("DefaultConnection")));


builder.Services.AddScoped<PredictionManagementService>();
builder.Services.AddScoped<ChatbotService>();


var app = builder.Build();


if(app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowReact");

app.UseHttpsRedirection();

app.MapControllers();


app.Run();