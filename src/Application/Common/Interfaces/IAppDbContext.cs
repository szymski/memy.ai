using Domain.Auth.Entities;
using Domain.Stories.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces;

public interface IAppDbContext {
    DbSet<User> Users { get; }
    DbSet<Story> Stories { get; }
    
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}