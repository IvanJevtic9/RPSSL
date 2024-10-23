using RPSSL.Application.Abstractions.Messaging;

namespace RPSSL.Application.GameFlow.GetPlayHistoryDetails;

public sealed record GetPlayHistoryQuery : IQuery<IReadOnlyList<GameSessionResponse>>;
