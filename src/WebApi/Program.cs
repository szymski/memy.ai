using System.Reflection;
using Application;
using Application.Stories.Queries;
using Domain.Stories.Interfaces;
using Infrastructure;
using Infrastructure.Data;
using Infrastructure.Data.Stories;
using MediatR;
using Microsoft.AspNetCore.Mvc.Filters;
using Presentation;
using Serilog;
using WebApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options => {
    options.Filters.Add<FluentValidationExceptionFilter>();
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new()
    {
        Title = "MemyAi",
        Version = "v1",
    });
    
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration)
    .AddPresentation();

builder.Host
    .UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MemyAi v1"));

    await app.InitializeDb();

    app.UseCors(policyBuilder => {
        policyBuilder.AllowAnyOrigin();
    });
}

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using var scope = app.Services.CreateScope();
scope.ServiceProvider.GetRequiredService<IStoryPresetStore>();

// using var scope = app.Services.CreateScope();
// var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
// await mediator.Send(new GetStoryQuery(2));

app.Run();