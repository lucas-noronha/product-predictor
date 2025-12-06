using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductPrediction.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Email = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "purchases",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Items = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_purchases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_purchases_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "shopping_lists",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Items = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_shopping_lists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_shopping_lists_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_purchases_UserId",
                table: "purchases",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_shopping_lists_UserId",
                table: "shopping_lists",
                column: "UserId");

            migrationBuilder.Sql(@"-- USERS
INSERT INTO public.users (""Id"", ""Name"", ""Email"")
VALUES
('75ee61c4-9594-4e0d-bc94-836d464ad0c5', 'admin', 'admin@admin')
ON CONFLICT (""Id"") DO NOTHING;

-- PURCHASES
INSERT INTO public.purchases
(""Id"", ""UserId"", ""Date"", ""Items"") 
VALUES
('4e3f4c69-8c05-4dd5-a4e6-4adf7ae1d913',
 '75ee61c4-9594-4e0d-bc94-836d464ad0c5',
 '2025-01-05',
 'arroz, feijao, acucar, oleo, sal, cafe'
),
('9f0d1a80-3b1e-4ef9-b4c8-1a3b0dd19922',
 '75ee61c4-9594-4e0d-bc94-836d464ad0c5',
 '2025-01-19',
 'arroz, feijao, macarrao, molho de tomate, papel higienico, sabonete'
),
('e0d69e5e-a4f5-4bab-8bc5-9b77f0d1d7f8',
 '75ee61c4-9594-4e0d-bc94-836d464ad0c5',
 '2025-02-02',
 'arroz, feijao, acucar, leite, cafe, manteiga'
),
('32ad5b89-62e4-4c44-8f32-7db0a7df14a8',
 '75ee61c4-9594-4e0d-bc94-836d464ad0c5',
 '2025-02-23',
 'detergente, esponja, desinfetante, papel higienico, sabao em po'
),
('5f980f21-2c89-4db3-8c98-533b46684421',
 '75ee61c4-9594-4e0d-bc94-836d464ad0c5',
 '2025-03-09',
 'arroz, feijao, oleo, sal, macarrao, molho de tomate'
),
('b4c78cc3-cc0d-4a91-bdb1-af33b8f433c0',
 '75ee61c4-9594-4e0d-bc94-836d464ad0c5',
 '2025-03-30',
 'arroz, feijao, acucar, cafe, leite, biscoito'
),
('e8c81faa-2f71-44e2-b62d-d1ae3f52ed85',
 '75ee61c4-9594-4e0d-bc94-836d464ad0c5',
 '2025-04-27',
 'detergente, desinfetante, amaciante, sabao em po, papel higienico'
),
('46bb0901-18f6-4144-9df6-117e395f4c2d',
 '75ee61c4-9594-4e0d-bc94-836d464ad0c5',
 '2025-05-04',
 'arroz, feijao, acucar, leite'
),
('5dd52c03-05da-4898-969d-3c5ae6d02820',
 '75ee61c4-9594-4e0d-bc94-836d464ad0c5',
 '2025-05-25',
 'arroz, feijao, oleo, sal, macarrao, cafe'
),
('086e6d3a-7e62-4d30-90d4-67ab0b77f394',
 '75ee61c4-9594-4e0d-bc94-836d464ad0c5',
 '2025-06-08',
 'detergente, desinfetante, papel higienico, pasta de dente, sabonete'
),
('cc75241d-44e5-4b6e-ba4d-82e506fe8ec3',
 '75ee61c4-9594-4e0d-bc94-836d464ad0c5',
 '2025-08-10',
 'arroz, feijao, acucar, cafe, leite, biscoito, refrigerante'
),
('d75a67ef-08e3-4d4c-997e-4fbeed7bb7c0',
 '75ee61c4-9594-4e0d-bc94-836d464ad0c5',
 '2025-11-30',
 'detergente, desinfetante, sabao em po, amaciante, papel higienico, saco de lixo'
)
ON CONFLICT (""Id"") DO NOTHING;
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "purchases");

            migrationBuilder.DropTable(
                name: "shopping_lists");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
