using Microsoft.EntityFrameworkCore;
using ParkyAPI.Data;
using ParkyAPI.Repository;
using ParkyAPI.Repository.IRepository;
using AutoMapper;
using System.Reflection;
using System.IO;
using ParkyAPI.ParkyMapper;
using System;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options=>
                          options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddControllers();

builder.Services.AddScoped<INationalParkRepository, NationalParkRepository>();
builder.Services.AddScoped<ITrailRepository, TrailRepository>();



builder.Services.AddAutoMapper(typeof(ParkyMappings));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    options =>
    {
        options.SwaggerDoc("ParkyOpenAPISpecNP",
            new Microsoft.OpenApi.Models.OpenApiInfo()
            {
                Title = "Parky API (National Park)",
                Version = "1"
            });
        options.SwaggerDoc("ParkyOpenAPISpecTrails",
            new Microsoft.OpenApi.Models.OpenApiInfo()
            {
                Title = "Parky API Trails",
                Version = "1"
            });

        var xmlCommentFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlCommentFullPath=Path.Combine(AppContext.BaseDirectory,xmlCommentFile);
        options.IncludeXmlComments(xmlCommentFullPath);


    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
    //options.SwaggerEndpoint(@"./swagger/v1/swagger.json", "v1");
        options.SwaggerEndpoint("/swagger/ParkyOpenAPISpecNP/swagger.json", "parky api np");
        options.SwaggerEndpoint("/swagger/ParkyOpenAPISpecTrails/swagger.json", "parky api trails");

        //options.RoutePrefix = "";

    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
