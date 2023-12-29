using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolAPI.Migrations
{
    /// <inheritdoc />
    public partial class seedingdata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string userId, roleId;
            userId = Guid.NewGuid().ToString();
            roleId = Guid.NewGuid().ToString();

            migrationBuilder.InsertData(
               table: "AspNetRoles",
               columns: new[] { "Id", "Name", "NormalizedName" },
               values: new object[,]
               {
                    { roleId ,"Admin","ADMIN" },
               }
           );

            migrationBuilder.InsertData(
             table: "AspNetUsers",
                columns: new[] { "Id", "UserName", "Email","EmailConfirmed", "NormalizedUserName", "NormalizedEmail",
                            "PasswordHash","PhoneNumber" , "PhoneNumberConfirmed" ,"TwoFactorEnabled","LockoutEnabled","AccessFailedCount"},
                values:new object[,] {
                    { userId ,"admin","admin@gmail.com",false,"ADMIN","ADMIN@GMAIL.COM","mmm123@!",
                        "01143385496",false,false,true,0}
                }
                );

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId" },
                values: new object[,]
                {
                    { userId , roleId }
                }
                ); 
            
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
