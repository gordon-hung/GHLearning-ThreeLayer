using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GHLearning.ThreeLayer.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: false, comment: "識別碼", collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    account = table.Column<string>(type: "varchar(31)", maxLength: 31, nullable: false, comment: "帳號名稱", collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    password = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, comment: "密碼", collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: false, comment: "創建時間"),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: false, comment: "最後更新時間")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                },
                comment: "使用者主表")
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "user_infos",
                columns: table => new
                {
                    user_id = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: false, comment: "使用者主表識別碼", collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    nick_name = table.Column<string>(type: "varchar(63)", maxLength: 63, nullable: false, comment: "暱稱", collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.user_id);
                    table.ForeignKey(
                        name: "fk_user_infos_users",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id");
                },
                comment: "使用者資訊")
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "user_logs",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false, comment: "識別碼", collation: "ascii_general_ci")
                        .Annotation("MySql:CharSet", "ascii"),
                    user_id = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: false, comment: "使用者識別碼", collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    @event = table.Column<string>(name: "event", type: "varchar(63)", maxLength: 63, nullable: false, comment: "事件", collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, comment: "描述", collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: false, comment: "日誌時間")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_logs_users",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "user_statuses",
                columns: table => new
                {
                    user_id = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: false, comment: "使用者主表識別碼", collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    status = table.Column<byte>(type: "tinyint unsigned", nullable: false, comment: "狀態")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.user_id);
                    table.ForeignKey(
                        name: "fk_user_statuses_users",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id");
                },
                comment: "使用者狀態")
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "user_vip_levels",
                columns: table => new
                {
                    user_id = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: false, comment: "使用者主表識別碼", collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    vip_level = table.Column<byte>(type: "tinyint unsigned", nullable: false, comment: "等級")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.user_id);
                    table.ForeignKey(
                        name: "fk_user_vip_levels_users",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id");
                },
                comment: "使用者等級")
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateIndex(
                name: "fk_user_log_user",
                table: "user_logs",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "uq_users_account",
                table: "users",
                column: "account",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "user_infos");

            migrationBuilder.DropTable(
                name: "user_logs");

            migrationBuilder.DropTable(
                name: "user_statuses");

            migrationBuilder.DropTable(
                name: "user_vip_levels");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
