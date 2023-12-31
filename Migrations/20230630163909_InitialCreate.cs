﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace tankman.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "orgs",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    data = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_orgs", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "org_properties",
                columns: table => new
                {
                    name = table.Column<string>(type: "text", nullable: false),
                    org_id = table.Column<string>(type: "text", nullable: false),
                    value = table.Column<string>(type: "text", nullable: false),
                    hidden = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_org_properties", x => new { x.name, x.org_id });
                    table.ForeignKey(
                        name: "fk_org_properties_orgs_org_id",
                        column: x => x.org_id,
                        principalTable: "orgs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "resources",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    org_id = table.Column<string>(type: "text", nullable: false),
                    data = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_resources", x => new { x.id, x.org_id });
                    table.ForeignKey(
                        name: "fk_resources_orgs_org_id",
                        column: x => x.org_id,
                        principalTable: "orgs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    org_id = table.Column<string>(type: "text", nullable: false),
                    data = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_roles", x => new { x.id, x.org_id });
                    table.ForeignKey(
                        name: "fk_roles_orgs_org_id",
                        column: x => x.org_id,
                        principalTable: "orgs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    org_id = table.Column<string>(type: "text", nullable: false),
                    data = table.Column<string>(type: "text", nullable: false),
                    identity_provider = table.Column<string>(type: "text", nullable: false),
                    identity_provider_user_id = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => new { x.id, x.org_id });
                    table.ForeignKey(
                        name: "fk_users_orgs_org_id",
                        column: x => x.org_id,
                        principalTable: "orgs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "resource_paths",
                columns: table => new
                {
                    resource_id = table.Column<string>(type: "text", nullable: false),
                    org_id = table.Column<string>(type: "text", nullable: false),
                    parent_id = table.Column<string>(type: "text", nullable: false),
                    parent1id = table.Column<string>(type: "text", nullable: false),
                    parent2id = table.Column<string>(type: "text", nullable: false),
                    parent3id = table.Column<string>(type: "text", nullable: false),
                    parent4id = table.Column<string>(type: "text", nullable: false),
                    parent5id = table.Column<string>(type: "text", nullable: false),
                    parent6id = table.Column<string>(type: "text", nullable: false),
                    parent7id = table.Column<string>(type: "text", nullable: false),
                    parent8id = table.Column<string>(type: "text", nullable: false),
                    parent9id = table.Column<string>(type: "text", nullable: false),
                    parent10id = table.Column<string>(type: "text", nullable: false),
                    parent11id = table.Column<string>(type: "text", nullable: false),
                    parent12id = table.Column<string>(type: "text", nullable: false),
                    parent13id = table.Column<string>(type: "text", nullable: false),
                    parent14id = table.Column<string>(type: "text", nullable: false),
                    parent15id = table.Column<string>(type: "text", nullable: false),
                    parent16id = table.Column<string>(type: "text", nullable: false),
                    depth = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_resource_paths", x => new { x.resource_id, x.org_id });
                    table.ForeignKey(
                        name: "fk_resource_paths_orgs_org_id",
                        column: x => x.org_id,
                        principalTable: "orgs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_resource_paths_resources_resource_id_org_id",
                        columns: x => new { x.resource_id, x.org_id },
                        principalTable: "resources",
                        principalColumns: new[] { "id", "org_id" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "role_permissions",
                columns: table => new
                {
                    role_id = table.Column<string>(type: "text", nullable: false),
                    resource_id = table.Column<string>(type: "text", nullable: false),
                    org_id = table.Column<string>(type: "text", nullable: false),
                    action = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_role_permissions", x => new { x.role_id, x.resource_id, x.org_id });
                    table.ForeignKey(
                        name: "fk_role_permissions_orgs_org_id",
                        column: x => x.org_id,
                        principalTable: "orgs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_role_permissions_resources_resource_id_org_id",
                        columns: x => new { x.resource_id, x.org_id },
                        principalTable: "resources",
                        principalColumns: new[] { "id", "org_id" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_role_permissions_roles_role_id_org_id",
                        columns: x => new { x.role_id, x.org_id },
                        principalTable: "roles",
                        principalColumns: new[] { "id", "org_id" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "role_properties",
                columns: table => new
                {
                    name = table.Column<string>(type: "text", nullable: false),
                    role_id = table.Column<string>(type: "text", nullable: false),
                    org_id = table.Column<string>(type: "text", nullable: false),
                    value = table.Column<string>(type: "text", nullable: false),
                    hidden = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_role_properties", x => new { x.name, x.role_id, x.org_id });
                    table.ForeignKey(
                        name: "fk_role_properties_orgs_org_id",
                        column: x => x.org_id,
                        principalTable: "orgs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_role_properties_roles_role_id_org_id",
                        columns: x => new { x.role_id, x.org_id },
                        principalTable: "roles",
                        principalColumns: new[] { "id", "org_id" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "role_assignments",
                columns: table => new
                {
                    user_id = table.Column<string>(type: "text", nullable: false),
                    role_id = table.Column<string>(type: "text", nullable: false),
                    org_id = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_role_assignments", x => new { x.role_id, x.user_id, x.org_id });
                    table.ForeignKey(
                        name: "fk_role_assignments_orgs_org_id",
                        column: x => x.org_id,
                        principalTable: "orgs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_role_assignments_roles_role_id_org_id",
                        columns: x => new { x.role_id, x.org_id },
                        principalTable: "roles",
                        principalColumns: new[] { "id", "org_id" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_role_assignments_users_user_id_org_id",
                        columns: x => new { x.user_id, x.org_id },
                        principalTable: "users",
                        principalColumns: new[] { "id", "org_id" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_permissions",
                columns: table => new
                {
                    user_id = table.Column<string>(type: "text", nullable: false),
                    resource_id = table.Column<string>(type: "text", nullable: false),
                    org_id = table.Column<string>(type: "text", nullable: false),
                    action = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_permissions", x => new { x.user_id, x.resource_id, x.org_id });
                    table.ForeignKey(
                        name: "fk_user_permissions_orgs_org_id",
                        column: x => x.org_id,
                        principalTable: "orgs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_permissions_resources_resource_id_org_id",
                        columns: x => new { x.resource_id, x.org_id },
                        principalTable: "resources",
                        principalColumns: new[] { "id", "org_id" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_permissions_users_user_id_org_id",
                        columns: x => new { x.user_id, x.org_id },
                        principalTable: "users",
                        principalColumns: new[] { "id", "org_id" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_properties",
                columns: table => new
                {
                    name = table.Column<string>(type: "text", nullable: false),
                    user_id = table.Column<string>(type: "text", nullable: false),
                    org_id = table.Column<string>(type: "text", nullable: false),
                    value = table.Column<string>(type: "text", nullable: false),
                    hidden = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_properties", x => new { x.name, x.user_id, x.org_id });
                    table.ForeignKey(
                        name: "fk_user_properties_orgs_org_id",
                        column: x => x.org_id,
                        principalTable: "orgs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_properties_users_user_id_org_id",
                        columns: x => new { x.user_id, x.org_id },
                        principalTable: "users",
                        principalColumns: new[] { "id", "org_id" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_org_properties_org_id",
                table: "org_properties",
                column: "org_id");

            migrationBuilder.CreateIndex(
                name: "ix_resource_paths_org_id",
                table: "resource_paths",
                column: "org_id");

            migrationBuilder.CreateIndex(
                name: "ix_resources_org_id",
                table: "resources",
                column: "org_id");

            migrationBuilder.CreateIndex(
                name: "ix_role_assignments_org_id",
                table: "role_assignments",
                column: "org_id");

            migrationBuilder.CreateIndex(
                name: "ix_role_assignments_role_id_org_id",
                table: "role_assignments",
                columns: new[] { "role_id", "org_id" });

            migrationBuilder.CreateIndex(
                name: "ix_role_assignments_user_id_org_id",
                table: "role_assignments",
                columns: new[] { "user_id", "org_id" });

            migrationBuilder.CreateIndex(
                name: "ix_role_permissions_org_id",
                table: "role_permissions",
                column: "org_id");

            migrationBuilder.CreateIndex(
                name: "ix_role_permissions_resource_id_org_id",
                table: "role_permissions",
                columns: new[] { "resource_id", "org_id" });

            migrationBuilder.CreateIndex(
                name: "ix_role_permissions_role_id_org_id",
                table: "role_permissions",
                columns: new[] { "role_id", "org_id" });

            migrationBuilder.CreateIndex(
                name: "ix_role_properties_org_id",
                table: "role_properties",
                column: "org_id");

            migrationBuilder.CreateIndex(
                name: "ix_role_properties_role_id_org_id",
                table: "role_properties",
                columns: new[] { "role_id", "org_id" });

            migrationBuilder.CreateIndex(
                name: "ix_roles_org_id",
                table: "roles",
                column: "org_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_permissions_org_id",
                table: "user_permissions",
                column: "org_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_permissions_resource_id_org_id",
                table: "user_permissions",
                columns: new[] { "resource_id", "org_id" });

            migrationBuilder.CreateIndex(
                name: "ix_user_permissions_user_id_org_id",
                table: "user_permissions",
                columns: new[] { "user_id", "org_id" });

            migrationBuilder.CreateIndex(
                name: "ix_user_properties_org_id",
                table: "user_properties",
                column: "org_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_properties_user_id_org_id",
                table: "user_properties",
                columns: new[] { "user_id", "org_id" });

            migrationBuilder.CreateIndex(
                name: "ix_users_org_id",
                table: "users",
                column: "org_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "org_properties");

            migrationBuilder.DropTable(
                name: "resource_paths");

            migrationBuilder.DropTable(
                name: "role_assignments");

            migrationBuilder.DropTable(
                name: "role_permissions");

            migrationBuilder.DropTable(
                name: "role_properties");

            migrationBuilder.DropTable(
                name: "user_permissions");

            migrationBuilder.DropTable(
                name: "user_properties");

            migrationBuilder.DropTable(
                name: "roles");

            migrationBuilder.DropTable(
                name: "resources");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "orgs");
        }
    }
}
