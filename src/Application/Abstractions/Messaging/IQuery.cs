using MediatR;

namespace Application.Abstractions.Messaging; 

public interface IQuery<out TResult> : IRequest<TResult> {
    
}