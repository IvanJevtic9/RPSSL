using MediatR;
using RPSSL.Domain.Abstraction;

namespace RPSSL.Application.Abstractions.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{ }
