using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LampWebStore.Migrations
{
    public partial class CreateLampsStoreDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Lamps",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    LampType = table.Column<byte>(nullable: false),
                    Manufacturer = table.Column<string>(nullable: true),
                    Cost = table.Column<double>(nullable: false),
                    ImageRef = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lamps", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Lamps",
                columns: new[] { "Id", "Cost", "ImageRef", "LampType", "Manufacturer" },
                values: new object[,]
                {
                    { 1, 16.0, "https://avatars.mds.yandex.net/get-mpic/1767083/img_id4205890578895788084.jpeg/9hq", (byte)2, "Philips" },
                    { 2, 2.0, "https://avatars.mds.yandex.net/get-mpic/1045304/img_id688239816713013197.jpeg/9hq", (byte)1, "OSRAM" },
                    { 3, 4.5, "https://avatars.mds.yandex.net/get-mpic/1045304/img_id5760657839941547295.jpeg/9hq", (byte)2, "OSRAM" },
                    { 4, 2.4, "https://avatars.mds.yandex.net/get-mpic/1045304/img_id4656031640183289008.jpeg/9hq", (byte)1, "Camelion" },
                    { 5, 2.7, "https://avatars.mds.yandex.net/get-mpic/1045304/img_id4827177099789801154.jpeg/9hq", (byte)2, "Gauss" },
                    { 6, 0.5, "https://avatars.mds.yandex.net/get-mpic/1045304/img_id2657193580316822485.jpeg/9hq", (byte)0, "Philips" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Lamps");
        }
    }
}
