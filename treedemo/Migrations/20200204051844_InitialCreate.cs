using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace treedemo.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AAAAATest_AgentTree",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Ancestor = table.Column<string>(nullable: false, defaultValue: ""),
                    Descendant = table.Column<string>(nullable: false, defaultValue: ""),
                    Distance = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AAAAATest_AgentTree", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AAAAATest_AgentTree_Ancestor_Descendant_Distance",
                table: "AAAAATest_AgentTree",
                columns: new[] { "Ancestor", "Descendant", "Distance" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AAAAATest_AgentTree");
        }
    }
}
