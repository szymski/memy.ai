using System.Data.Common;
using System.Reflection;
using Application.Common.Interfaces;
using Domain.Auth.Entities;
using Domain.Stories.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Serilog;

namespace Infrastructure.Data;

public class AppDbContext : IdentityDbContext<User, IdentityRole<int>, int>, IAppDbContext {

    public DbSet<Story> Stories { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override async void OnModelCreating(ModelBuilder mb)
    {
        Log.Logger.Information("OnModelCreating");
        base.OnModelCreating(mb);
        mb.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        mb.Entity<Story>()
            .HasOne(s => s.User)
            .WithMany(u => u.Stories)
            .IsRequired();
    }

    public IDbContextTransaction BeginTransaction()
    {
        return Database.BeginTransaction();
    }

    public IDbContextTransaction UseTransaction(DbTransaction? transaction)
    {
        return Database.UseTransaction(transaction)!;
    }
}