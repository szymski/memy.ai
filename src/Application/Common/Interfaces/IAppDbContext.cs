using Domain.Stories.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces;

public interface IAppDbContext {
    DbSet<Story> Stories { get; }
}