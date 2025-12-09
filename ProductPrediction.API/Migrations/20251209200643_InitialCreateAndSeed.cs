using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductPrediction.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreateAndSeed : Migration
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
                    Title = table.Column<string>(type: "text", nullable: false),
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
-- ============================
-- GRUPO 1: itens com previsão ~ONTEM
-- Itens: arroz, feijao, oleo
-- Intervalo fixo: 30 dias
-- Última compra: CURRENT_DATE - 31 dias
-- ============================
('0a3c5f10-1b2c-4d3e-9a10-111213141516',
 '75ee61c4-9594-4e0d-bc94-836d464ad0c5',
 CURRENT_DATE - INTERVAL '121 days',
 'arroz, feijao, oleo'
),
('1b4d6a21-2c3d-4e5f-8b21-212223242526',
 '75ee61c4-9594-4e0d-bc94-836d464ad0c5',
 CURRENT_DATE - INTERVAL '91 days',
 'arroz, feijao, oleo'
),
('2c5e7b32-3d4e-5f60-7c32-313233343536',
 '75ee61c4-9594-4e0d-bc94-836d464ad0c5',
 CURRENT_DATE - INTERVAL '61 days',
 'arroz, feijao, oleo'
),
('3d6f8c43-4e5f-6071-6d43-414243444546',
 '75ee61c4-9594-4e0d-bc94-836d464ad0c5',
 CURRENT_DATE - INTERVAL '30 days',
 'arroz, feijao, oleo'
),

-- ============================
-- GRUPO 2: itens com previsão ~AMANHÃ
-- Itens: detergente, desinfetante, sabao em po
-- Intervalo fixo: 20 dias
-- Última compra: CURRENT_DATE - 19 dias
-- ============================
('4e709d54-5f60-7182-5e54-515253545556',
 '75ee61c4-9594-4e0d-bc94-836d464ad0c5',
 CURRENT_DATE - INTERVAL '79 days',
 'detergente, desinfetante, sabao em po'
),
('5f81ae65-6071-8293-4f65-616263646566',
 '75ee61c4-9594-4e0d-bc94-836d464ad0c5',
 CURRENT_DATE - INTERVAL '59 days',
 'detergente, desinfetante, sabao em po'
),
('6a92bf76-7182-93a4-3a76-717273747576',
 '75ee61c4-9594-4e0d-bc94-836d464ad0c5',
 CURRENT_DATE - INTERVAL '39 days',
 'detergente, desinfetante, sabao em po'
),
('7ba3d087-8293-a4b5-2b87-818283848586',
 '75ee61c4-9594-4e0d-bc94-836d464ad0c5',
 CURRENT_DATE - INTERVAL '19 days',
 'detergente, desinfetante, sabao em po'
),

-- ============================
-- OUTRAS COMPRAS (variedade de itens, sem mexer nos padrões acima)
-- ============================
('8cb4e198-93a4-b5c6-1c98-919293949596',
 '75ee61c4-9594-4e0d-bc94-836d464ad0c5',
 CURRENT_DATE - INTERVAL '115 days',
 'acucar, leite, cafe, biscoito'
),
('9dc5f2a9-a4b5-c6d7-0da9-a1a2a3a4a5a6',
 '75ee61c4-9594-4e0d-bc94-836d464ad0c5',
 CURRENT_DATE - INTERVAL '105 days',
 'leite, manteiga, pao, presunto'
),
('adb6c3da-b5c6-d7e8-1bea-b1b2b3b4b5b6',
 '75ee61c4-9594-4e0d-bc94-836d464ad0c5',
 CURRENT_DATE - INTERVAL '98 days',
 'macarrao, molho de tomate, queijo ralado'
),
('bec7d4eb-c6d7-e8f9-2cfb-c1c2c3c4c5c6',
 '75ee61c4-9594-4e0d-bc94-836d464ad0c5',
 CURRENT_DATE - INTERVAL '88 days',
 'papel higienico, sabonete, pasta de dente'
),
('cfd8e5fc-d7e8-f90a-3d0c-d1d2d3d4d5d6',
 '75ee61c4-9594-4e0d-bc94-836d464ad0c5',
 CURRENT_DATE - INTERVAL '82 days',
 'amaciante, sabao em pedra, saco de lixo'
),
('d0e9f60d-e8f9-0a1b-4e1d-e1e2e3e4e5e6',
 '75ee61c4-9594-4e0d-bc94-836d464ad0c5',
 CURRENT_DATE - INTERVAL '72 days',
 'cafe, acucar, leite, bolacha'
),
('e1faf71e-f90a-1b2c-5f2e-f1f2f3f4f5f6',
 '75ee61c4-9594-4e0d-bc94-836d464ad0c5',
 CURRENT_DATE - INTERVAL '66 days',
 'arroz integral, feijao preto, sal grosso'
),
('f20b082f-0a1b-2c3d-603f-010203040506',
 '75ee61c4-9594-4e0d-bc94-836d464ad0c5',
 CURRENT_DATE - INTERVAL '58 days',
 'detergente neutro, desinfetante pinho, limpa vidro'
),
('012c1930-1b2c-3d4e-7140-111213141516',
 '75ee61c4-9594-4e0d-bc94-836d464ad0c5',
 CURRENT_DATE - INTERVAL '52 days',
 'shampoo, condicionador, sabonete liquido'
),
('123d2a41-2c3d-4e5f-8251-212223242526',
 '75ee61c4-9594-4e0d-bc94-836d464ad0c5',
 CURRENT_DATE - INTERVAL '46 days',
 'suco em po, refrigerante, biscoito recheado'
),
('234e3b52-3d4e-5f60-9362-313233343536',
 '75ee61c4-9594-4e0d-bc94-836d464ad0c5',
 CURRENT_DATE - INTERVAL '38 days',
 'leite em po, achocolatado, cereal matinal'
),
('345f4c63-4e5f-6071-a473-414243444546',
 '75ee61c4-9594-4e0d-bc94-836d464ad0c5',
 CURRENT_DATE - INTERVAL '34 days',
 'guardanapo, papel toalha, filme plastico'
),
('45605d74-5f60-7182-b584-515253545556',
 '75ee61c4-9594-4e0d-bc94-836d464ad0c5',
 CURRENT_DATE - INTERVAL '30 days',
 'arroz parboilizado, ervilha, milho verde'
),
('56716e85-6071-8293-c695-616263646566',
 '75ee61c4-9594-4e0d-bc94-836d464ad0c5',
 CURRENT_DATE - INTERVAL '24 days',
 'esponja de aco, desengordurante, limpa aluminio'
),
('67827f96-7182-93a4-d7a6-717273747576',
 '75ee61c4-9594-4e0d-bc94-836d464ad0c5',
 CURRENT_DATE - INTERVAL '18 days',
 'iogurte, queijo, presunto, mortadela'
),
('789390a7-8293-a4b5-e8b7-818283848586',
 '75ee61c4-9594-4e0d-bc94-836d464ad0c5',
 CURRENT_DATE - INTERVAL '12 days',
 'frango congelado, carne moida, linguica'
)
ON CONFLICT (""Id"") DO NOTHING;

-- SHOPPING LISTS
INSERT INTO public.shopping_lists
(""Id"", ""Items"", ""UserId"", ""Title"")
VALUES
('c5a1c2c0-2e7c-4f39-9dd7-eae902d113fb',
 'arroz,feijao,oleo,sal,acucar,cafe',
 '75ee61c4-9594-4e0d-bc94-836d464ad0c5',
 'compra mensal basica'),

('d2b5f96c-13f5-4bf1-8b6c-8dc4df9e97c5',
 'macarrao,molho de tomate,queijo ralado,sal',
 '75ee61c4-9594-4e0d-bc94-836d464ad0c5',
 'ingredientes para massas'),

('fa43f1ab-6df7-4b34-9dd4-91c5dfbcab4e',
 'detergente,desinfetante,amaciante,sabao em po,saco de lixo',
 '75ee61c4-9594-4e0d-bc94-836d464ad0c5',
 'limpeza da casa'),

('a63af8a1-b601-4595-8eab-3afc93dc9631',
 'papel higienico,pasta de dente,sabonete',
 '75ee61c4-9594-4e0d-bc94-836d464ad0c5',
 'higiene pessoal'),

('b8b5d741-7ec4-4cd3-91b1-b16c64c75b30',
 'leite,manteiga,biscoito,refrigerante',
 '75ee61c4-9594-4e0d-bc94-836d464ad0c5',
 'lanche da semana'),

('e7e3b1f4-3218-4df6-8e73-4f142c88f0f2',
 'arroz,feijao,cafe,acucar,leite',
 '75ee61c4-9594-4e0d-bc94-836d464ad0c5',
 'repor essenciais'),

('70c43927-26b8-4456-b844-9ce4cdb36655',
 'detergente,esponja,desinfetante',
 '75ee61c4-9594-4e0d-bc94-836d464ad0c5',
 'limpeza leve'),

('0d1f2dda-4a4b-4c03-a6ae-7b5f996c5b32',
 'sabao em po,amaciante,papel higienico,sabonete',
 '75ee61c4-9594-4e0d-bc94-836d464ad0c5',
 'lavanderia')
;
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
