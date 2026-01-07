using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VehicalApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RideBookings",
                columns: table => new
                {
                    BookingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    VehicleId = table.Column<int>(type: "int", nullable: false),
                    ShowroomId = table.Column<int>(type: "int", nullable: false),
                    BookingDate = table.Column<DateOnly>(type: "date", nullable: false),
                    SlotHour = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    BookingCreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RideBookings", x => x.BookingId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: false),
                    Role = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false, collation: "Latin1_General_CS_AS"),
                    createdAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    RefreshToken = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: true),
                    RefreshTokenExpiry = table.Column<DateTime>(type: "datetime", nullable: true),
                    Number = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Users__1788CC4CF023086F", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Showrooms",
                columns: table => new
                {
                    ShowroomId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShowroomName = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    ShowroomLocation = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    ContactNumber = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: false),
                    ManagerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Showroom__A7726CBB0A7E547B", x => x.ShowroomId);
                    table.ForeignKey(
                        name: "FK__Showrooms__Manag__6383C8BA",
                        column: x => x.ManagerId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "Vehicle",
                columns: table => new
                {
                    VehicalId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShowroomId = table.Column<int>(type: "int", nullable: false),
                    VehicleName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Model = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    YearOfProduction = table.Column<int>(type: "int", nullable: false),
                    BasePrice = table.Column<int>(type: "int", nullable: false),
                    AgeInShowroom = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    stockCount = table.Column<int>(type: "int", nullable: false),
                    shortDescription = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Vehicle__1D89FCE1D3A942A5", x => x.VehicalId);
                    table.ForeignKey(
                        name: "FK__Vehicle__Showroo__71D1E811",
                        column: x => x.ShowroomId,
                        principalTable: "Showrooms",
                        principalColumn: "ShowroomId");
                });

            migrationBuilder.CreateTable(
                name: "VehicleImage",
                columns: table => new
                {
                    ImageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VehicleId = table.Column<int>(type: "int", nullable: false),
                    ImageLocation = table.Column<string>(type: "varchar(150)", unicode: false, maxLength: 150, nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__VehicleI__7516F70C3988B832", x => x.ImageId);
                    table.ForeignKey(
                        name: "FK_VehicleImage_Vehicle",
                        column: x => x.VehicleId,
                        principalTable: "Vehicle",
                        principalColumn: "VehicalId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VehicleSpecifications",
                columns: table => new
                {
                    specificationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    vehicalId = table.Column<int>(type: "int", nullable: false),
                    engine = table.Column<int>(type: "int", nullable: false),
                    PowerOfvehical = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    Torque = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    fuleType = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    Mileage = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    BodyType = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    SeatingCapacity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__VehicleS__FC1C560A6A276819", x => x.specificationId);
                    table.ForeignKey(
                        name: "FK_VehicleSpecification_Vehicle",
                        column: x => x.vehicalId,
                        principalTable: "Vehicle",
                        principalColumn: "VehicalId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Showrooms_ManagerId",
                table: "Showrooms",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "UQ__Showroom__570665C6F41D4DD4",
                table: "Showrooms",
                column: "ContactNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__Showroom__C46B526F0EFC64D4",
                table: "Showrooms",
                column: "ShowroomName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__Users__A9D1053468E7FA6E",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vehicle_AgeInShowroom",
                table: "Vehicle",
                column: "AgeInShowroom");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicle_BasePrice",
                table: "Vehicle",
                column: "BasePrice");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicle_ShowroomId",
                table: "Vehicle",
                column: "ShowroomId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicle_VehicleName",
                table: "Vehicle",
                column: "VehicleName");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleImage_VehicleId",
                table: "VehicleImage",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleSpecifications_BodyType",
                table: "VehicleSpecifications",
                column: "BodyType");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleSpecifications_Engine",
                table: "VehicleSpecifications",
                column: "engine");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleSpecifications_FuelType",
                table: "VehicleSpecifications",
                column: "fuleType");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleSpecifications_VehicleId",
                table: "VehicleSpecifications",
                column: "vehicalId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RideBookings");

            migrationBuilder.DropTable(
                name: "VehicleImage");

            migrationBuilder.DropTable(
                name: "VehicleSpecifications");

            migrationBuilder.DropTable(
                name: "Vehicle");

            migrationBuilder.DropTable(
                name: "Showrooms");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
