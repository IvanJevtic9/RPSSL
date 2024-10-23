using MediatR;
using RPSSL.Domain.Abstraction;

namespace RPSSL.Application.Abstractions.Messaging;

public interface ICommandHandler<TCommand, TResponse> : IRequestHandler<TCommand, Result<TResponse>>
    where TCommand : ICommand<TResponse>
{ }
