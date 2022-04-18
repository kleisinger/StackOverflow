using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StackOverload.Data.Migrations
{
    public partial class addedtagQuestionstocontext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TagQuestion_Questions_QuestionId",
                table: "TagQuestion");

            migrationBuilder.DropForeignKey(
                name: "FK_TagQuestion_Tags_TagId",
                table: "TagQuestion");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TagQuestion",
                table: "TagQuestion");

            migrationBuilder.RenameTable(
                name: "TagQuestion",
                newName: "TagQuestions");

            migrationBuilder.RenameIndex(
                name: "IX_TagQuestion_TagId",
                table: "TagQuestions",
                newName: "IX_TagQuestions_TagId");

            migrationBuilder.RenameIndex(
                name: "IX_TagQuestion_QuestionId",
                table: "TagQuestions",
                newName: "IX_TagQuestions_QuestionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TagQuestions",
                table: "TagQuestions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TagQuestions_Questions_QuestionId",
                table: "TagQuestions",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TagQuestions_Tags_TagId",
                table: "TagQuestions",
                column: "TagId",
                principalTable: "Tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TagQuestions_Questions_QuestionId",
                table: "TagQuestions");

            migrationBuilder.DropForeignKey(
                name: "FK_TagQuestions_Tags_TagId",
                table: "TagQuestions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TagQuestions",
                table: "TagQuestions");

            migrationBuilder.RenameTable(
                name: "TagQuestions",
                newName: "TagQuestion");

            migrationBuilder.RenameIndex(
                name: "IX_TagQuestions_TagId",
                table: "TagQuestion",
                newName: "IX_TagQuestion_TagId");

            migrationBuilder.RenameIndex(
                name: "IX_TagQuestions_QuestionId",
                table: "TagQuestion",
                newName: "IX_TagQuestion_QuestionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TagQuestion",
                table: "TagQuestion",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TagQuestion_Questions_QuestionId",
                table: "TagQuestion",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TagQuestion_Tags_TagId",
                table: "TagQuestion",
                column: "TagId",
                principalTable: "Tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
