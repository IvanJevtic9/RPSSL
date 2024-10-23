using RPSSL.Application.Abstractions.Messaging;
using RPSSL.Application.GameFlow.Shared;

namespace RPSSL.Application.GameFlow.PlayRoundAgainstComputer;

public sealed record PlayRoundAgainstComputerCommand(int ChoiceId) : ICommand<PlayerVsComputerRoundOutcomeResponse>;
