using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eVote360Pro.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class TestMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Citizens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdentificationNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Citizens", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Elections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ActivationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FinishedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Elections", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ElectivePositions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ElectivePositions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PartidosPoliticos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Acronym = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LogoUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartidosPoliticos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CodigosVerificacion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CitizenId = table.Column<int>(type: "int", nullable: false),
                    ElectionId = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: false),
                    GeneratedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsUsed = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CitizenId1 = table.Column<int>(type: "int", nullable: true),
                    ElectionId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CodigosVerificacion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CodigosVerificacion_Citizens_CitizenId",
                        column: x => x.CitizenId,
                        principalTable: "Citizens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CodigosVerificacion_Citizens_CitizenId1",
                        column: x => x.CitizenId1,
                        principalTable: "Citizens",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CodigosVerificacion_Elections_ElectionId",
                        column: x => x.ElectionId,
                        principalTable: "Elections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CodigosVerificacion_Elections_ElectionId1",
                        column: x => x.ElectionId1,
                        principalTable: "Elections",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "VotosCiudadanos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CitizenId = table.Column<int>(type: "int", nullable: false),
                    ElectionId = table.Column<int>(type: "int", nullable: false),
                    VotedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CitizenId1 = table.Column<int>(type: "int", nullable: true),
                    ElectionId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VotosCiudadanos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VotosCiudadanos_Citizens_CitizenId",
                        column: x => x.CitizenId,
                        principalTable: "Citizens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VotosCiudadanos_Citizens_CitizenId1",
                        column: x => x.CitizenId1,
                        principalTable: "Citizens",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_VotosCiudadanos_Elections_ElectionId",
                        column: x => x.ElectionId,
                        principalTable: "Elections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VotosCiudadanos_Elections_ElectionId1",
                        column: x => x.ElectionId1,
                        principalTable: "Elections",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CandidatePositions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CandidateId = table.Column<int>(type: "int", nullable: false),
                    ElectivePositionId = table.Column<int>(type: "int", nullable: false),
                    PoliticalPartyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CandidatePositions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CandidatePositions_ElectivePositions_ElectivePositionId",
                        column: x => x.ElectivePositionId,
                        principalTable: "ElectivePositions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Alianzas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Party1Id = table.Column<int>(type: "int", nullable: false),
                    Party2Id = table.Column<int>(type: "int", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alianzas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Alianzas_PartidosPoliticos_Party1Id",
                        column: x => x.Party1Id,
                        principalTable: "PartidosPoliticos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Alianzas_PartidosPoliticos_Party2Id",
                        column: x => x.Party2Id,
                        principalTable: "PartidosPoliticos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Candidatos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PhotoUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    ParticipatedInElection = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    PoliticalPartyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Candidatos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Candidatos_PartidosPoliticos_PoliticalPartyId",
                        column: x => x.PoliticalPartyId,
                        principalTable: "PartidosPoliticos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SolicitudesAlianzas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicantPartyId = table.Column<int>(type: "int", nullable: false),
                    ReceiverPartyId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    ResponseDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SolicitudesAlianzas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SolicitudesAlianzas_PartidosPoliticos_ApplicantPartyId",
                        column: x => x.ApplicantPartyId,
                        principalTable: "PartidosPoliticos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SolicitudesAlianzas_PartidosPoliticos_ReceiverPartyId",
                        column: x => x.ReceiverPartyId,
                        principalTable: "PartidosPoliticos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AsignacionesDirigentes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    PoliticalPartyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AsignacionesDirigentes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AsignacionesDirigentes_PartidosPoliticos_PoliticalPartyId",
                        column: x => x.PoliticalPartyId,
                        principalTable: "PartidosPoliticos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AsignacionesDirigentes_Usuarios_UserId",
                        column: x => x.UserId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Votos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ElectionId = table.Column<int>(type: "int", nullable: false),
                    ElectivePositionId = table.Column<int>(type: "int", nullable: false),
                    CandidateId = table.Column<int>(type: "int", nullable: true),
                    ElectionId1 = table.Column<int>(type: "int", nullable: true),
                    ElectivePositionId1 = table.Column<int>(type: "int", nullable: true),
                    CandidateId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Votos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Votos_Candidatos_CandidateId",
                        column: x => x.CandidateId,
                        principalTable: "Candidatos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Votos_Candidatos_CandidateId1",
                        column: x => x.CandidateId1,
                        principalTable: "Candidatos",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Votos_Elections_ElectionId",
                        column: x => x.ElectionId,
                        principalTable: "Elections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Votos_Elections_ElectionId1",
                        column: x => x.ElectionId1,
                        principalTable: "Elections",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Votos_ElectivePositions_ElectivePositionId",
                        column: x => x.ElectivePositionId,
                        principalTable: "ElectivePositions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Votos_ElectivePositions_ElectivePositionId1",
                        column: x => x.ElectivePositionId1,
                        principalTable: "ElectivePositions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Alianzas_Party1Id",
                table: "Alianzas",
                column: "Party1Id");

            migrationBuilder.CreateIndex(
                name: "IX_Alianzas_Party2Id",
                table: "Alianzas",
                column: "Party2Id");

            migrationBuilder.CreateIndex(
                name: "IX_AsignacionesDirigentes_PoliticalPartyId",
                table: "AsignacionesDirigentes",
                column: "PoliticalPartyId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AsignacionesDirigentes_UserId",
                table: "AsignacionesDirigentes",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CandidatePositions_ElectivePositionId",
                table: "CandidatePositions",
                column: "ElectivePositionId");

            migrationBuilder.CreateIndex(
                name: "IX_Candidatos_PoliticalPartyId",
                table: "Candidatos",
                column: "PoliticalPartyId");

            migrationBuilder.CreateIndex(
                name: "IX_CodigosVerificacion_CitizenId_ElectionId",
                table: "CodigosVerificacion",
                columns: new[] { "CitizenId", "ElectionId" });

            migrationBuilder.CreateIndex(
                name: "IX_CodigosVerificacion_CitizenId1",
                table: "CodigosVerificacion",
                column: "CitizenId1");

            migrationBuilder.CreateIndex(
                name: "IX_CodigosVerificacion_ElectionId",
                table: "CodigosVerificacion",
                column: "ElectionId");

            migrationBuilder.CreateIndex(
                name: "IX_CodigosVerificacion_ElectionId1",
                table: "CodigosVerificacion",
                column: "ElectionId1");

            migrationBuilder.CreateIndex(
                name: "IX_PartidosPoliticos_Acronym",
                table: "PartidosPoliticos",
                column: "Acronym",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PartidosPoliticos_Name",
                table: "PartidosPoliticos",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SolicitudesAlianzas_ApplicantPartyId",
                table: "SolicitudesAlianzas",
                column: "ApplicantPartyId");

            migrationBuilder.CreateIndex(
                name: "IX_SolicitudesAlianzas_ReceiverPartyId",
                table: "SolicitudesAlianzas",
                column: "ReceiverPartyId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Email",
                table: "Usuarios",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_UserName",
                table: "Usuarios",
                column: "UserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Votos_CandidateId",
                table: "Votos",
                column: "CandidateId");

            migrationBuilder.CreateIndex(
                name: "IX_Votos_CandidateId1",
                table: "Votos",
                column: "CandidateId1");

            migrationBuilder.CreateIndex(
                name: "IX_Votos_ElectionId",
                table: "Votos",
                column: "ElectionId");

            migrationBuilder.CreateIndex(
                name: "IX_Votos_ElectionId1",
                table: "Votos",
                column: "ElectionId1");

            migrationBuilder.CreateIndex(
                name: "IX_Votos_ElectivePositionId",
                table: "Votos",
                column: "ElectivePositionId");

            migrationBuilder.CreateIndex(
                name: "IX_Votos_ElectivePositionId1",
                table: "Votos",
                column: "ElectivePositionId1");

            migrationBuilder.CreateIndex(
                name: "IX_VotosCiudadanos_CitizenId_ElectionId",
                table: "VotosCiudadanos",
                columns: new[] { "CitizenId", "ElectionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VotosCiudadanos_CitizenId1",
                table: "VotosCiudadanos",
                column: "CitizenId1");

            migrationBuilder.CreateIndex(
                name: "IX_VotosCiudadanos_ElectionId",
                table: "VotosCiudadanos",
                column: "ElectionId");

            migrationBuilder.CreateIndex(
                name: "IX_VotosCiudadanos_ElectionId1",
                table: "VotosCiudadanos",
                column: "ElectionId1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Alianzas");

            migrationBuilder.DropTable(
                name: "AsignacionesDirigentes");

            migrationBuilder.DropTable(
                name: "CandidatePositions");

            migrationBuilder.DropTable(
                name: "CodigosVerificacion");

            migrationBuilder.DropTable(
                name: "SolicitudesAlianzas");

            migrationBuilder.DropTable(
                name: "Votos");

            migrationBuilder.DropTable(
                name: "VotosCiudadanos");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Candidatos");

            migrationBuilder.DropTable(
                name: "ElectivePositions");

            migrationBuilder.DropTable(
                name: "Citizens");

            migrationBuilder.DropTable(
                name: "Elections");

            migrationBuilder.DropTable(
                name: "PartidosPoliticos");
        }
    }
}
