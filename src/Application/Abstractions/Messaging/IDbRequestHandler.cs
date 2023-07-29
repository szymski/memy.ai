using Application.Common.Interfaces;
using MediatR;

namespace Application.Abstractions.Messaging;

public interface IDbRequestHandler {
    IAppDbContext Context { get; set; }
}