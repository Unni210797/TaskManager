using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Task_Management_System.Migrations
{
    /// <inheritdoc />
    public partial class Procedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string procedure = @"Create procedure GetFilteredTaskByName
                                @Name nvarchar(100) 
                                as
                                Begin
                                select * from TaskInfos 
                                where UserName=@Name
                                End";
            migrationBuilder.Sql(procedure);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string procedure = @"Drop procedure GetFilteredTaskByName";
            migrationBuilder.Sql(procedure);
        }
    }
}
