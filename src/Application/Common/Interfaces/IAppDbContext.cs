using System.Data.Common;
using Domain.Auth.Entities;
using Domain.Credits.Entities;
using Domain.Stories.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Application.Common.Interfaces;

public interface IAppDbContext {
    DbSet<User> Users { get; }
    DbSet<Story> Stories { get; }
    DbSet<CreditEvent> CreditEvents { get; }
    
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    public IDbContextTransaction BeginTransaction();
    public IDbContextTransaction UseTransaction(DbTransaction? transaction);
}