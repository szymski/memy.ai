using Application.Common.Interfaces;
using MediatR;

namespace Application.Abstractions.Messaging;

public interface IDbRequestHandler {
     IAppDbContext Context { get; set; }
}

public abstract class DbRequestHandler : IDbRequestHandler {
     public IAppDbContext Context { get; set; }
}