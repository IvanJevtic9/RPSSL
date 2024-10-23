using RPSSL.Application.GameFlow.Shared;
using RPSSL.Application.Abstractions.Messaging;

namespace RPSSL.Application.GameFlow.GetAvailableChoices;

public sealed record GetAvailableChoicesQuery : IQuery<IReadOnlyList<ChoiceResponse>>;
