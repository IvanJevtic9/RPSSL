using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RPSSL.Infrastructure.Migrations;

/// <inheritdoc />
public partial class InitialMigration : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Players",
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
                    principalTable: "Players",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_GameSessions_Players_PlayerTwoId",
                    column: x => x.PlayerTwoId,
                    principalTable: "Players",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateTable(
            name: "GameRounds",
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
                    principalTable: "GameSessions",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_GameRounds_GameSessionId",
            table: "GameRounds",
            column: "GameSessionId");

        migrationBuilder.CreateIndex(
            name: "IX_GameSessions_PlayerOneId",
            table: "GameSessions",
            column: "PlayerOneId");

        migrationBuilder.CreateIndex(
            name: "IX_GameSessions_PlayerTwoId",
            table: "GameSessions",
            column: "PlayerTwoId");

        migrationBuilder.CreateIndex(
            name: "IX_Players_Email",
            table: "Players",
            column: "Email",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Players_Username",
            table: "Players",
            column: "Username",
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "GameRounds");

        migrationBuilder.DropTable(
            name: "GameSessions");

        migrationBuilder.DropTable(
            name: "Players");
    }
}
