using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace StudentWProject.Migrations
{
    public partial class SecondMigr : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AmbionRefId",
                table: "Student",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Ambion",
                columns: table => new
                {
                    AmbionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AmbionName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ambion", x => x.AmbionId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Student_AmbionRefId",
                table: "Student",
                column: "AmbionRefId");

            migrationBuilder.AddForeignKey(
                name: "FK_Student_Ambion_AmbionRefId",
                table: "Student",
                column: "AmbionRefId",
                principalTable: "Ambion",
                principalColumn: "AmbionId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Student_Ambion_AmbionRefId",
                table: "Student");

            migrationBuilder.DropTable(
                name: "Ambion");

            migrationBuilder.DropIndex(
                name: "IX_Student_AmbionRefId",
                table: "Student");

            migrationBuilder.DropColumn(
                name: "AmbionRefId",
                table: "Student");
        }
    }
}
