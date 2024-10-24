using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RPSSL.Infrastructure.Migrations;

/// <inheritdoc />
public partial class InitialMigration : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.EnsureSchema(
            name: "Game");

        migrationBuilder.EnsureSchema(
            name: "Identity");

        migrationBuilder.CreateTable(
            name: "Players",
            schema: "Identity",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Email = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: false),
                Username = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Players", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "GameSessions",
            schema: "Game",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                PlayerOneId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                PlayerTwoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                GameType = table.Column<int>(type: "int", nullable: false),
                StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                EndDate = table.Column<DateTime>(type: "datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_GameSessions", x => x.Id);
                table.ForeignKey(
                    name: "FK_GameSessions_Players_PlayerOneId",
                    column: x => x.PlayerOneId,
                    principalSchema: "Identity",
                    principalTable: "Players",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_GameSessions_Players_PlayerTwoId",
                    column: x => x.PlayerTwoId,
                    principalSchema: "Identity",
                    principalTable: "Players",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateTable(
            name: "GameRounds",
            schema: "Game",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                GameSessionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                PlayerOneChoice = table.Column<int>(type: "int", nullable: false),
                PlayerTwoChoice = table.Column<int>(type: "int", nullable: false),
                PlayedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_GameRounds", x => x.Id);
                table.ForeignKey(
                    name: "FK_GameRounds_GameSessions_GameSessionId",
                    column: x => x.GameSessionId,
                    principalSchema: "Game",
                    principalTable: "GameSessions",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_GameRounds_GameSessionId",
            schema: "Game",
            table: "GameRounds",
            column: "GameSessionId");

        migrationBuilder.CreateIndex(
            name: "IX_GameSessions_PlayerOneId",
            schema: "Game",
            table: "GameSessions",
            column: "PlayerOneId");

        migrationBuilder.CreateIndex(
            name: "IX_GameSessions_PlayerTwoId",
            schema: "Game",
            table: "GameSessions",
            column: "PlayerTwoId");

        migrationBuilder.CreateIndex(
            name: "IX_Players_Email",
            schema: "Identity",
            table: "Players",
            column: "Email",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Players_Username",
            schema: "Identity",
            table: "Players",
            column: "Username",
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "GameRounds",
            schema: "Game");

        migrationBuilder.DropTable(
            name: "GameSessions",
            schema: "Game");

        migrationBuilder.DropTable(
            name: "Players",
            schema: "Identity");
    }
}
