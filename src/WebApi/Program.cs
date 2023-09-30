using System.Reflection;
using Application;
using Domain.Auth.Entities;
using Domain.Stories.Interfaces;
using Infrastructure;
using Infrastructure.Data;
using Mapster;
using Mapster.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Presentation;
using Serilog;
using WebApi.Auth.Services;
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

builder.Services.AddScoped<CurrentUserAccessor>();

builder.Services.AddAuthentication(o => {
        o.DefaultAuthenticateScheme = IdentityConstants.BearerScheme;
    })
    .AddBearerToken(IdentityConstants.BearerScheme);

builder.Services.AddAuthorizationBuilder()
    .AddDefaultPolicy("", new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .AddAuthenticationSchemes(IdentityConstants.BearerScheme)
        .Build());

builder.Services
    .AddIdentity<User, IdentityRole<int>>(o => {
        o.User.RequireUniqueEmail = true;
        o.Password.RequireNonAlphanumeric = false;
        o.Password.RequireUppercase = false;
    })
    .AddEntityFrameworkStores<AppDbContext>()
    .AddApiEndpoints();

builder.Services.AddMapster();

var typeAdapterConfig = TypeAdapterConfig.GlobalSettings;
var assembly = Assembly.GetExecutingAssembly();
typeAdapterConfig.ScanInheritedTypes(assembly);

builder.Host
    .UseSerilog((
        context,
        configuration
    ) => configuration.ReadFrom.Configuration(context.Configuration));

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

app.UseAuthentication();
app.UseAuthorization();

app.MapGroup("api/auth").MapIdentityApi<User>();

app.MapControllers();

using var scope = app.Services.CreateScope();
scope.ServiceProvider.GetRequiredService<IStoryPresetStore>();

// using var scope = app.Services.CreateScope();
// var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
// await mediator.Send(new GetStoryQuery(2));

app.Run();