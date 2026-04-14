using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MasterNet.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "cursos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Titulo = table.Column<string>(type: "TEXT", nullable: true),
                    Descripcion = table.Column<string>(type: "TEXT", nullable: true),
                    FechaPublicacion = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cursos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "instructores",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Nombre = table.Column<string>(type: "TEXT", nullable: true),
                    Apellidos = table.Column<string>(type: "TEXT", nullable: true),
                    Grado = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_instructores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "precios",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Nombre = table.Column<string>(type: "VARCHAR", maxLength: 250, nullable: true),
                    PrecioActual = table.Column<decimal>(type: "TEXT", precision: 10, scale: 2, nullable: false),
                    PrecioPromocion = table.Column<decimal>(type: "TEXT", precision: 10, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_precios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "calificaciones",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Alumno = table.Column<string>(type: "TEXT", nullable: true),
                    Puntaje = table.Column<int>(type: "INTEGER", nullable: false),
                    Comentario = table.Column<string>(type: "TEXT", nullable: true),
                    CursoId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_calificaciones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_calificaciones_cursos_CursoId",
                        column: x => x.CursoId,
                        principalTable: "cursos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "imagenes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Url = table.Column<string>(type: "TEXT", nullable: true),
                    CursoId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_imagenes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_imagenes_cursos_CursoId",
                        column: x => x.CursoId,
                        principalTable: "cursos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "cursos_instructores",
                columns: table => new
                {
                    CursoId = table.Column<Guid>(type: "TEXT", nullable: false),
                    InstructorId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cursos_instructores", x => new { x.CursoId, x.InstructorId });
                    table.ForeignKey(
                        name: "FK_cursos_instructores_cursos_CursoId",
                        column: x => x.CursoId,
                        principalTable: "cursos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_cursos_instructores_instructores_InstructorId",
                        column: x => x.InstructorId,
                        principalTable: "instructores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "cursos_precios",
                columns: table => new
                {
                    CursoId = table.Column<Guid>(type: "TEXT", nullable: false),
                    PrecioId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cursos_precios", x => new { x.CursoId, x.PrecioId });
                    table.ForeignKey(
                        name: "FK_cursos_precios_cursos_CursoId",
                        column: x => x.CursoId,
                        principalTable: "cursos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_cursos_precios_precios_PrecioId",
                        column: x => x.PrecioId,
                        principalTable: "precios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "cursos",
                columns: new[] { "Id", "Descripcion", "FechaPublicacion", "Titulo" },
                values: new object[,]
                {
                    { new Guid("03ac5227-4f65-4ab1-ba66-2f2d0390516a"), "New ABC 13 9370, 13.3, 5th Gen CoreA5-8250U, 8GB RAM, 256GB SSD, power UHD Graphics, OS 10 Home, OS Office A & J 2016", new DateTime(2025, 10, 27, 19, 27, 28, 202, DateTimeKind.Local).AddTicks(7609), "Awesome Frozen Mouse" },
                    { new Guid("34476689-242a-4d02-8a3f-6c5eee1a2310"), "The Apollotech B340 is an affordable wireless mouse with reliable connectivity, 12 months battery life and modern design", new DateTime(2025, 11, 13, 11, 29, 42, 236, DateTimeKind.Local).AddTicks(7933), "Fantastic Concrete Shoes" },
                    { new Guid("3b1fb476-e2ac-4c89-a326-66a1ac807726"), "The Apollotech B340 is an affordable wireless mouse with reliable connectivity, 12 months battery life and modern design", new DateTime(2025, 6, 1, 17, 45, 34, 147, DateTimeKind.Local).AddTicks(3435), "Practical Soft Shoes" },
                    { new Guid("80fba108-598e-4584-b620-fdecb0119602"), "Carbonite web goalkeeper gloves are ergonomically designed to give easy fit", new DateTime(2025, 8, 29, 7, 59, 51, 529, DateTimeKind.Local).AddTicks(9900), "Practical Metal Shirt" },
                    { new Guid("8f998161-bfa8-4734-9b00-13236c260024"), "The Nagasaki Lander is the trademarked name of several series of Nagasaki sport bikes, that started with the 1984 ABC800J", new DateTime(2025, 4, 15, 20, 2, 45, 826, DateTimeKind.Local).AddTicks(7349), "Refined Rubber Fish" },
                    { new Guid("a7e961b9-d8df-4dad-bf8f-df0de987c5d3"), "Carbonite web goalkeeper gloves are ergonomically designed to give easy fit", new DateTime(2025, 9, 9, 1, 16, 9, 946, DateTimeKind.Local).AddTicks(894), "Handcrafted Frozen Towels" },
                    { new Guid("b396ac97-a118-4f47-a17c-ae0a41b9ea16"), "The beautiful range of Apple Naturalé that has an exciting mix of natural ingredients. With the Goodness of 100% Natural Ingredients", new DateTime(2025, 12, 8, 18, 26, 47, 300, DateTimeKind.Local).AddTicks(9525), "Handcrafted Granite Pants" },
                    { new Guid("b44118a6-ce7b-4a55-b463-4c4d16513e08"), "Andy shoes are designed to keeping in mind durability as well as trends, the most stylish range of shoes & sandals", new DateTime(2025, 5, 29, 6, 51, 20, 918, DateTimeKind.Local).AddTicks(4260), "Tasty Frozen Chair" },
                    { new Guid("e2c285a7-f395-4726-a6b5-d283fe124492"), "Carbonite web goalkeeper gloves are ergonomically designed to give easy fit", new DateTime(2026, 3, 17, 23, 24, 25, 57, DateTimeKind.Local).AddTicks(2700), "Practical Soft Mouse" },
                    { new Guid("faadcbfa-6a7a-49d0-837c-b84b2b9bfd79"), "The slim & simple Maple Gaming Keyboard from Dev Byte comes with a sleek body and 7- Color RGB LED Back-lighting for smart functionality", new DateTime(2025, 6, 25, 19, 32, 42, 266, DateTimeKind.Local).AddTicks(5259), "Rustic Wooden Fish" }
                });

            migrationBuilder.InsertData(
                table: "instructores",
                columns: new[] { "Id", "Apellidos", "Grado", "Nombre" },
                values: new object[,]
                {
                    { new Guid("19349b32-a3f0-4461-ad8c-8b41dcb87294"), "Sawayn", "Legacy Response Liaison", "Pierre" },
                    { new Guid("2379073a-efb3-4c4b-bba5-006909dbf56b"), "Wiza", "Human Optimization Developer", "Conrad" },
                    { new Guid("31ca3552-2e16-4eb9-87e9-e492bc69422b"), "Tremblay", "National Factors Associate", "Samir" },
                    { new Guid("384c9775-884f-4116-8705-f32f3ca9d004"), "Von", "Global Response Specialist", "Alexandra" },
                    { new Guid("40a56bb6-3b59-4617-b508-ae60d20a433c"), "Olson", "Senior Security Planner", "Andrew" },
                    { new Guid("45e97544-db15-42ed-8a2b-37626398f503"), "Macejkovic", "Senior Usability Facilitator", "Kennedi" },
                    { new Guid("7f12a9a5-2067-45aa-8831-057b5ec9a4b9"), "Beier", "Dynamic Marketing Analyst", "Regan" },
                    { new Guid("93ce3998-851b-4f24-9c70-6a31faf3f577"), "Ward", "Customer Usability Strategist", "Lera" },
                    { new Guid("988bdb82-eca5-4001-91f9-34967d3b1c87"), "Windler", "Regional Program Architect", "Darryl" },
                    { new Guid("ad25a34d-6108-487b-93f9-a45ce1fefebe"), "Mohr", "Forward Applications Producer", "Kolby" }
                });

            migrationBuilder.InsertData(
                table: "precios",
                columns: new[] { "Id", "Nombre", "PrecioActual", "PrecioPromocion" },
                values: new object[] { new Guid("fbe60fc6-205d-4702-aa88-571c5975bea0"), "Precio regular", 10.00m, 8.00m });

            migrationBuilder.CreateIndex(
                name: "IX_calificaciones_CursoId",
                table: "calificaciones",
                column: "CursoId");

            migrationBuilder.CreateIndex(
                name: "IX_cursos_instructores_InstructorId",
                table: "cursos_instructores",
                column: "InstructorId");

            migrationBuilder.CreateIndex(
                name: "IX_cursos_precios_PrecioId",
                table: "cursos_precios",
                column: "PrecioId");

            migrationBuilder.CreateIndex(
                name: "IX_imagenes_CursoId",
                table: "imagenes",
                column: "CursoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "calificaciones");

            migrationBuilder.DropTable(
                name: "cursos_instructores");

            migrationBuilder.DropTable(
                name: "cursos_precios");

            migrationBuilder.DropTable(
                name: "imagenes");

            migrationBuilder.DropTable(
                name: "instructores");

            migrationBuilder.DropTable(
                name: "precios");

            migrationBuilder.DropTable(
                name: "cursos");
        }
    }
}
