using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFCore6_Activity0201.DBLibrary.Migrations
{
    public partial class EncryptionMigration_Step2_BackupData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                UPDATE [HumanResources].[Employee]
                    SET [NationalIDNumberBackup] = [NationalIDNumber]
                    ,[JobTitleBackup] = [JobTitle]
                    ,[BirthDateBackup] = [BirthDate]
                    ,[MaritalStatusBackup] = [MaritalStatus]
                    ,[GenderBackup] = [Gender]
                    ,[HireDateBackup] = [HireDate]
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
