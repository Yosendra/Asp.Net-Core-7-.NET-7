using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    public partial class InsertPerson_StoredProcedure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string sqlStatement = @"
CREATE PROCEDURE [dbo].[InsertPerson](
    @Id uniqueidentifier,
    @Name nvarchar(40),
    @Email nvarchar(40),
    @DateOfBirth datetime2(7),
    @Gender nvarchar(10),
    @Address nvarchar(200),
    @ReceiveNewsLetters bit,
    @CountryId uniqueidentifier
)
AS BEGIN
    INSERT INTO [dbo].[Persons] 
    VALUES(@Id, @Name, @Email, @DateOfBirth, @Gender, @Address, @ReceiveNewsLetters, @CountryId)
END
            ";
            migrationBuilder.Sql(sqlStatement);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string sqlStatement = @"
DROP PROCEDURE [dbo].[InsertPerson]
            ";
            migrationBuilder.Sql(sqlStatement);
        }
    }
}
