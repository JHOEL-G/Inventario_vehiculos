using Microsoft.EntityFrameworkCore.Migrations;

namespace AutoManager.AutoManager_Infrastructure.Configurations
{
    public partial class InitialBrandsAndModelsData : Migration
    {

        // Dentro del archivo [Timestamp]_FinalDataSeeding.cs

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Insertar datos de Marcas (Brands)
            migrationBuilder.InsertData(
                table: "Brands",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
            { 1, "Toyota" }, { 2, "Honda" }, { 3, "Ford" }, { 4, "Nissan" }
                });

            // Insertar datos de Modelos (Models)
            migrationBuilder.InsertData(
                table: "Models",
                columns: new[] { "Id", "Name", "BrandId" },
                values: new object[,]
                {
            // Modelos para Toyota (BrandId = 1)
            { 1, "Corolla", 1 }, { 2, "Hilux", 1 },
            // Modelos para Honda (BrandId = 2)
            { 3, "Civic", 2 }, { 4, "CR-V", 2 },
            // Modelos para Ford (BrandId = 3)
            { 5, "F-150", 3 }, { 6, "Ranger", 3 },
            // Modelos para Nissan (BrandId = 4)
            { 7, "Versa", 4 }, { 8, "Frontier", 4 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Revertir: Eliminar datos de Modelos
            migrationBuilder.DeleteData(table: "Models", keyColumn: "Id", keyValues: new object[] { 1, 2, 3, 4, 5, 6, 7, 8 });

            // Revertir: Eliminar datos de Marcas
            migrationBuilder.DeleteData(table: "Brands", keyColumn: "Id", keyValues: new object[] { 1, 2, 3, 4 });
        }
    }
}