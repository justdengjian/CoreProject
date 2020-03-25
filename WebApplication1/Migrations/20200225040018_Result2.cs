using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication1.Migrations
{
    public partial class Result2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TypeId",
                table: "Results",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ResultModel",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StuName = table.Column<string>(maxLength: 10, nullable: false),
                    Title = table.Column<string>(maxLength: 100, nullable: false),
                    Description = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResultModel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ResultTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResultTypes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Results_TypeId",
                table: "Results",
                column: "TypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Results_ResultTypes_TypeId",
                table: "Results",
                column: "TypeId",
                principalTable: "ResultTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Results_ResultTypes_TypeId",
                table: "Results");

            migrationBuilder.DropTable(
                name: "ResultModel");

            migrationBuilder.DropTable(
                name: "ResultTypes");

            migrationBuilder.DropIndex(
                name: "IX_Results_TypeId",
                table: "Results");

            migrationBuilder.DropColumn(
                name: "TypeId",
                table: "Results");
        }
    }
}
