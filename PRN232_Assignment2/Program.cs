using Assignment2_PRN232.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Repository;
using Repository.Models;
using Service;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
        options.JsonSerializerOptions.MaxDepth = 32;
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("MyPolicy",
    builder =>
    {
        builder.WithOrigins("https://localhost:7044")
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

builder.Services.AddScoped(typeof(GenericRepository<>));

builder.Services.AddScoped<AccountService>();
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<NewsArticleService>();
builder.Services.AddScoped<TagService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("MyPolicy");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
