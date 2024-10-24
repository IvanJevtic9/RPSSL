using System.Data;
using RPSSL.Domain.Game;
using RPSSL.Domain.Players;
using RPSSL.Domain.GameFlow;
using RPSSL.Domain.Abstraction;
using Microsoft.AspNetCore.Http;
using RPSSL.Application.Extensions;
using Microsoft.EntityFrameworkCore;
using RPSSL.Application.GameFlow.Shared;
using RPSSL.Application.Abstractions.Data;
using RPSSL.Application.Abstractions.Messaging;

namespace RPSSL.Application.GameFlow.GetPlayHistoryDetails;

internal sealed record GetPlayHistoryHandler : IQueryHandler<GetPlayHistoryQuery, IReadOnlyList<GameSessionResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GetPlayHistoryHandler(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Result<IReadOnlyList<GameSessionResponse>>> Handle(GetPlayHistoryQuery request, CancellationToken cancellationToken)
    {
        var userIdentifier = _httpContextAccessor.HttpContext?.User?.GetUserIdentifier();

        var gameSessions = await _unitOfWork.GameSessions
            .AsNoTracking()
            .Include(x => x.Rounds)
            .Where(x =>
                x.PlayerOneId == userIdentifier ||
                x.PlayerTwoId == userIdentifier)
            .ToListAsync(cancellationToken);

        var gameSessionsResponse = await MapToResponse(gameSessions, userIdentifier);

        return new Result<IReadOnlyList<GameSessionResponse>>(gameSessionsResponse);
    }

    private async Task<IReadOnlyList<GameSessionResponse>> MapToResponse(IEnumerable<GameSession>? gameSessions, Guid? playerId)
    {
        if (gameSessions is null)
        {
            return Array.Empty<GameSessionResponse>();
        }

        var gameSessionsResponse = new List<GameSessionResponse>();
        var playerDictionary = new Dictionary<Guid, PlayerResponse>();
        var username = _httpContextAccessor.HttpContext?.User?.GetUsername();

        foreach (var gameSession in gameSessions)
        {
            var gameSessionState = GameSessionStateFactory.ToModel(gameSession);

            var otherPlayerId = gameSessionState.PlayerOneId == playerId
                ? gameSessionState.PlayerTwoId
                : gameSessionState.PlayerOneId;

            var playerResponse = await GetPlayerResponseAsync(otherPlayerId, playerDictionary);

            var gameRoundResponse = gameSessionState.Rounds
                .Select(round => new GameRoundResponse
                {
                    PlayerOneChoice = round.PlayerOneChoice.ToString().ToLower(),
                    PlayerTwoChoice = round.PlayerTwoChoice.ToString().ToLower(),
                    Result = round.Result.ToString(),
                    PlayedDate = round.PlayedDate
                }).ToList();

            gameSessionsResponse.Add(new GameSessionResponse
            {
                Id = gameSessionState.SessionId,
                Status = gameSessionState.Status,
                GameType = gameSessionState.GameType.ToString(),
                PlayerOne = Username.GetOrDefault(username),
                PlayerTwo = Username.GetOrDefault(playerResponse?.Username, true),
                PlayerOneTotalRoundWins = gameSessionState.PlayerOneRoundWins,
                PlayerTwoTotalRoundWins = gameSessionState.PlayerTwoRoundWins,
                StartDate = gameSessionState.StartDate,
                EndDate = gameSessionState.EndDate,
                GameRounds = gameRoundResponse
            });
        }

        return gameSessionsResponse;
    }

    private async Task<PlayerResponse?> GetPlayerResponseAsync(Guid? otherPlayerId, Dictionary<Guid, PlayerResponse> playerDictionary)
    {
        var foundInDictionary = playerDictionary.TryGetValue(otherPlayerId ?? Guid.Empty, out var playerResponse);

        if (foundInDictionary || !otherPlayerId.HasValue)
        {
            return playerResponse;
        }

        var player = await _unitOfWork.Players
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == otherPlayerId);

        if (player is not null)
        {
            playerResponse = new PlayerResponse
            {
                Id = player.Id,
                Username = player.Username.Value,
                Email = player.Email.Value
            };

            playerDictionary[otherPlayerId.Value] = playerResponse;
        }

        return playerResponse;
    }
}
