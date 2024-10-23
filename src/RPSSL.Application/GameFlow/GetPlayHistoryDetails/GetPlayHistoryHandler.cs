using Dapper;
using System.Data;
using RPSSL.Domain.GameFlow;
using RPSSL.Domain.Abstraction;
using Microsoft.AspNetCore.Http;
using RPSSL.Application.Extensions;
using RPSSL.Application.Game.Factories;
using RPSSL.Application.GameFlow.Shared;
using RPSSL.Application.Abstractions.Data;
using RPSSL.Application.Abstractions.Messaging;
using RPSSL.Domain.Players;

namespace RPSSL.Application.GameFlow.GetPlayHistoryDetails;

internal sealed record GetPlayHistoryHandler : IQueryHandler<GetPlayHistoryQuery, IReadOnlyList<GameSessionResponse>>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public GetPlayHistoryHandler(IHttpContextAccessor httpContextAccessor, ISqlConnectionFactory sqlConnectionFactory)
    {
        _httpContextAccessor = httpContextAccessor;
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<Result<IReadOnlyList<GameSessionResponse>>> Handle(GetPlayHistoryQuery request, CancellationToken cancellationToken)
    {
        var userIdentifier = _httpContextAccessor.HttpContext?.User?.GetUserIdentifier();

        const string sqlQuery = """
            SELECT 
            	gs.Id,
            	gs.PlayerOneId,
            	gs.PlayerTwoId,
            	gs.GameType,
            	gs.StartDate,
            	gs.EndDate,
            	gr.Id as RoundId,
            	gr.GameSessionId,
            	gr.PlayerOneChoice,
            	gr.PlayerTwoChoice,
            	gr.PlayedDate
            FROM 
            	dbo.GameSessions AS gs WITH (NOLOCK)
            	JOIN dbo.GameRounds as gr WITH (NOLOCK)
            	ON gs.Id = gr.GameSessionId
            WHERE 
            	PlayerOneId = @UserIdentifier OR
            	PlayerTwoId = @UserIdentifier
            """;

        using var connection = _sqlConnectionFactory.CreateConnection();

        var gameSessionDictionary = new Dictionary<Guid, GameSession>();

        var gameSessions = await connection.QueryAsync<GameSession, GameRound, GameSession>(
            sqlQuery,
            (gameSession, gameRound) =>
            {
                if (!gameSessionDictionary.TryGetValue(gameSession.Id, out var gameSessionEntry))
                {
                    gameSessionEntry = gameSession;
                    gameSessionDictionary.Add(gameSession.Id, gameSessionEntry);
                }

                gameSessionEntry.GameRounds.Add(gameRound);

                return gameSessionEntry;
            },
            new { UserIdentifier = userIdentifier },
            splitOn: "RoundId"
        );

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
                    GameSessionId = round.GameSessionId,
                    PlayerOneChoice = round.PlayerOneChoice.ToString().ToLower(),
                    PlayerTwoChoice = round.PlayerTwoChoice.ToString().ToLower(),
                    Result = round.Result.ToString(),
                    PlayedDate = round.PlayedDate
                }).ToList();

            gameSessionsResponse.Add(new GameSessionResponse
            {
                Id = gameSessionState.SessionId,
                PlayerOne = Username.GetOrDefault(username),
                PlayerTwo = Username.GetOrDefault(playerResponse?.Username, true),
                GameType = gameSessionState.GameType.ToString(),
                StartDate = gameSessionState.StartDate,
                EndDate = gameSessionState.EndDate,
                PlayerOneTotalRoundWins = gameSessionState.PlayerOneRoundWins,
                PlayerTwoTotalRoundWins = gameSessionState.PlayerTwoRoundWins,
                Status = gameSessionState.Status,
                GameRounds = gameRoundResponse
            });
        }

        return gameSessionsResponse;
    }

    private async Task<PlayerResponse?> GetPlayerResponseAsync(Guid? otherPlayerId, Dictionary<Guid, PlayerResponse> playerDictionary)
    {
        if (!playerDictionary.TryGetValue(otherPlayerId ?? Guid.Empty, out var playerResponse) && otherPlayerId.HasValue)
        {
            const string sqlQuery = """
                SELECT
                    Id,
                    Username,
                    Email
                FROM 
                    Players
                WHERE 
                    Id = @Id
                """;

            using var connection = _sqlConnectionFactory.CreateConnection();

            playerResponse = await connection.QuerySingleOrDefaultAsync<PlayerResponse?>(sqlQuery, new { Id = otherPlayerId });

            if (playerResponse is not null)
            {
                playerDictionary[otherPlayerId.Value] = playerResponse;
            }
        }

        return playerResponse;
    }
}
