using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SafeSpaceAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddUsuarioAgendamentoRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UsuarioSSId",
                table: "Agendamento",
                type: "RAW(16)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Agendamento_UsuarioSSId",
                table: "Agendamento",
                column: "UsuarioSSId");

            migrationBuilder.AddForeignKey(
                name: "FK_Agendamento_UsuarioSSId",
                table: "Agendamento",
                column: "UsuarioSSId",
                principalTable: "UsuarioSS",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Agendamento_UsuarioSSId",
                table: "Agendamento");

            migrationBuilder.DropIndex(
                name: "IX_Agendamento_UsuarioSSId",
                table: "Agendamento");

            migrationBuilder.DropColumn(
                name: "UsuarioSSId",
                table: "Agendamento");
        }
    }
}
