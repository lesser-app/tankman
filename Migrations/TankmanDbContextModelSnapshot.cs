﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using tankman.Db;

#nullable disable

namespace tankman.Migrations
{
    [DbContext(typeof(TankmanDbContext))]
    partial class TankmanDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("tankman.Models.Org", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text")
                        .HasColumnName("id");

                    b.Property<bool>("Active")
                        .HasColumnType("boolean")
                        .HasColumnName("active");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.HasKey("Id")
                        .HasName("pk_orgs");

                    b.ToTable("orgs", (string)null);
                });

            modelBuilder.Entity("tankman.Models.Resource", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text")
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<string>("OrgId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("org_id");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("path");

                    b.HasKey("Id")
                        .HasName("pk_resources");

                    b.ToTable("resources", (string)null);
                });

            modelBuilder.Entity("tankman.Models.Role", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text")
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<string>("OrgId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("org_id");

                    b.HasKey("Id")
                        .HasName("pk_roles");

                    b.ToTable("roles", (string)null);
                });

            modelBuilder.Entity("tankman.Models.RoleAssignment", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text")
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("role_id");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_role_assignments");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_role_assignments_user_id");

                    b.ToTable("role_assignments", (string)null);
                });

            modelBuilder.Entity("tankman.Models.RolePermission", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text")
                        .HasColumnName("id");

                    b.Property<string>("Action")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("action");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<string>("Resource")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("resource");

                    b.Property<string>("ResourceId")
                        .HasColumnType("text")
                        .HasColumnName("resource_id");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("role");

                    b.HasKey("Id")
                        .HasName("pk_role_permissions");

                    b.HasIndex("ResourceId")
                        .HasDatabaseName("ix_role_permissions_resource_id");

                    b.ToTable("role_permissions", (string)null);
                });

            modelBuilder.Entity("tankman.Models.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text")
                        .HasColumnName("id");

                    b.Property<bool>("Active")
                        .HasColumnType("boolean")
                        .HasColumnName("active");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<string>("IdentityProvider")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("identity_provider");

                    b.Property<string>("IdentityProviderUserId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("identity_provider_user_id");

                    b.Property<string>("OrgId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("org_id");

                    b.HasKey("Id")
                        .HasName("pk_users");

                    b.ToTable("users", (string)null);
                });

            modelBuilder.Entity("tankman.Models.UserPermission", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text")
                        .HasColumnName("id");

                    b.Property<string>("Action")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("action");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<string>("Resource")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("resource");

                    b.Property<string>("ResourceId")
                        .HasColumnType("text")
                        .HasColumnName("resource_id");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_user_permissions");

                    b.HasIndex("ResourceId")
                        .HasDatabaseName("ix_user_permissions_resource_id");

                    b.ToTable("user_permissions", (string)null);
                });

            modelBuilder.Entity("tankman.Models.RoleAssignment", b =>
                {
                    b.HasOne("tankman.Models.User", null)
                        .WithMany("RoleAssignments")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_role_assignments_users_user_id");
                });

            modelBuilder.Entity("tankman.Models.RolePermission", b =>
                {
                    b.HasOne("tankman.Models.Resource", null)
                        .WithMany("RolePermissions")
                        .HasForeignKey("ResourceId")
                        .HasConstraintName("fk_role_permissions_resources_resource_id");
                });

            modelBuilder.Entity("tankman.Models.UserPermission", b =>
                {
                    b.HasOne("tankman.Models.Resource", null)
                        .WithMany("UserPermissions")
                        .HasForeignKey("ResourceId")
                        .HasConstraintName("fk_user_permissions_resources_resource_id");
                });

            modelBuilder.Entity("tankman.Models.Resource", b =>
                {
                    b.Navigation("RolePermissions");

                    b.Navigation("UserPermissions");
                });

            modelBuilder.Entity("tankman.Models.User", b =>
                {
                    b.Navigation("RoleAssignments");
                });
#pragma warning restore 612, 618
        }
    }
}
