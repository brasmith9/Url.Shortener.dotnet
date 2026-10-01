using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Url.Shortner.Dtos;
using Url.Shortner.Entity;
using Url.Shortner.Extensions;
using Url.Shortner.Models;
using Url.Shortner.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IUrlShortenerService, UrlShortenerService>();
builder.Services.AddDbContextPool<AppDbContext>(options =>
{
    options
        .UseNpgsql(NpgsqlDataSource.Create(builder.Configuration.GetConnectionString("DefaultConnection")!))
        .UseSnakeCaseNamingConvention();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.ApplyMigrations();
app.UseHttpsRedirection();

app.UseExceptionHandler(exceptionHandler =>
{
    exceptionHandler.Run(async context =>
    {
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/json";
        var ex = context.Features.Get<IExceptionHandlerFeature>();
        if (ex != null)
        {
            var error = ex.Error;
            await context.Response.WriteAsync(error.Message);
        }

        await context.Response.WriteAsync("An error occured during processing");
    });
});

app.AddApiEndpoints();

app.Run();