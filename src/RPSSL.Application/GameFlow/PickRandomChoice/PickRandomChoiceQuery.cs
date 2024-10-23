using RPSSL.Application.GameFlow.Shared;
using RPSSL.Application.Abstractions.Messaging;

namespace RPSSL.Application.GameFlow.PickRandomChoice;

public sealed record PickRandomChoiceQuery : IQuery<ChoiceResponse>;
