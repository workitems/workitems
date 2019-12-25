using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Violet.WorkItems.Provider.SqlServer.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WorkItems",
                columns: table => new
                {
                    ProjectCode = table.Column<string>(nullable: false),
                    Id = table.Column<string>(nullable: false),
                    WorkItemType = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkItems", x => new { x.ProjectCode, x.Id });
                });

            migrationBuilder.CreateTable(
                name: "LogEntry",
                columns: table => new
                {
                    Date = table.Column<DateTimeOffset>(nullable: false),
                    ProjectCode = table.Column<string>(nullable: false),
                    Id = table.Column<string>(nullable: false),
                    User = table.Column<string>(nullable: true),
                    Comment = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogEntry", x => new { x.ProjectCode, x.Id, x.Date });
                    table.ForeignKey(
                        name: "FK_LogEntry_WorkItems_ProjectCode_Id",
                        columns: x => new { x.ProjectCode, x.Id },
                        principalTable: "WorkItems",
                        principalColumns: new[] { "ProjectCode", "Id" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Property",
                columns: table => new
                {
                    Name = table.Column<string>(nullable: false),
                    ProjectCode = table.Column<string>(nullable: false),
                    Id = table.Column<string>(nullable: false),
                    DataType = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Property", x => new { x.ProjectCode, x.Id, x.Name });
                    table.ForeignKey(
                        name: "FK_Property_WorkItems_ProjectCode_Id",
                        columns: x => new { x.ProjectCode, x.Id },
                        principalTable: "WorkItems",
                        principalColumns: new[] { "ProjectCode", "Id" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PropertyChange",
                columns: table => new
                {
                    Name = table.Column<string>(nullable: false),
                    ProjectCode = table.Column<string>(nullable: false),
                    Id = table.Column<string>(nullable: false),
                    Date = table.Column<DateTimeOffset>(nullable: false),
                    OldValue = table.Column<string>(nullable: true),
                    NewValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertyChange", x => new { x.ProjectCode, x.Id, x.Date, x.Name });
                    table.ForeignKey(
                        name: "FK_PropertyChange_LogEntry_ProjectCode_Id_Date",
                        columns: x => new { x.ProjectCode, x.Id, x.Date },
                        principalTable: "LogEntry",
                        principalColumns: new[] { "ProjectCode", "Id", "Date" },
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Property");

            migrationBuilder.DropTable(
                name: "PropertyChange");

            migrationBuilder.DropTable(
                name: "LogEntry");

            migrationBuilder.DropTable(
                name: "WorkItems");
        }
    }
}
