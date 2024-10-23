using MediatR;
using RPSSL.Domain.Abstraction;

namespace RPSSL.Application.Abstractions.Messaging;

public interface ICommand<TResponse> : IRequest<Result<TResponse>>, IBaseCommand
{ }
