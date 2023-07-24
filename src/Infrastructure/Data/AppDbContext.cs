using System.Reflection;
using Application.Common.Interfaces;
using Domain.Stories.Entities;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Infrastructure.Data; 

public class AppDbContext : DbContext, IAppDbContext {

    public DbSet<Story> Stories { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        Log.Logger.Information("OnModelCreating");
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}