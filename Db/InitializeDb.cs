using Microsoft.EntityFrameworkCore;

namespace tankman.Db;

public static class InitializeDb
{
  const string initSql =
      $$"""
      CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
          migration_id character varying(150) NOT NULL,
          product_version character varying(32) NOT NULL,
          CONSTRAINT pk___ef_migrations_history PRIMARY KEY (migration_id)
      );

      START TRANSACTION;

      CREATE TABLE orgs (
          id text NOT NULL,
          created_at timestamp with time zone NOT NULL,
          data text NOT NULL,
          CONSTRAINT pk_orgs PRIMARY KEY (id)
      );

      CREATE TABLE org_properties (
          name text NOT NULL,
          org_id text NOT NULL,
          value text NOT NULL,
          hidden boolean NOT NULL,
          created_at timestamp with time zone NOT NULL,
          CONSTRAINT pk_org_properties PRIMARY KEY (name, org_id),
          CONSTRAINT fk_org_properties_orgs_org_id FOREIGN KEY (org_id) REFERENCES orgs (id) ON DELETE CASCADE
      );

      CREATE TABLE resources (
          id text NOT NULL,
          org_id text NOT NULL,
          data text NOT NULL,
          created_at timestamp with time zone NOT NULL,
          CONSTRAINT pk_resources PRIMARY KEY (id, org_id),
          CONSTRAINT fk_resources_orgs_org_id FOREIGN KEY (org_id) REFERENCES orgs (id) ON DELETE CASCADE
      );

      CREATE TABLE roles (
          id text NOT NULL,
          org_id text NOT NULL,
          data text NOT NULL,
          created_at timestamp with time zone NOT NULL,
          CONSTRAINT pk_roles PRIMARY KEY (id, org_id),
          CONSTRAINT fk_roles_orgs_org_id FOREIGN KEY (org_id) REFERENCES orgs (id) ON DELETE CASCADE
      );

      CREATE TABLE users (
          id text NOT NULL,
          org_id text NOT NULL,
          data text NOT NULL,
          identity_provider text NOT NULL,
          identity_provider_user_id text NOT NULL,
          created_at timestamp with time zone NOT NULL,
          CONSTRAINT pk_users PRIMARY KEY (id, org_id),
          CONSTRAINT fk_users_orgs_org_id FOREIGN KEY (org_id) REFERENCES orgs (id) ON DELETE CASCADE
      );

      CREATE TABLE resource_paths (
          resource_id text NOT NULL,
          org_id text NOT NULL,
          parent_id text NOT NULL,
          parent1id text NOT NULL,
          parent2id text NOT NULL,
          parent3id text NOT NULL,
          parent4id text NOT NULL,
          parent5id text NOT NULL,
          parent6id text NOT NULL,
          parent7id text NOT NULL,
          parent8id text NOT NULL,
          parent9id text NOT NULL,
          parent10id text NOT NULL,
          parent11id text NOT NULL,
          parent12id text NOT NULL,
          parent13id text NOT NULL,
          parent14id text NOT NULL,
          parent15id text NOT NULL,
          parent16id text NOT NULL,
          depth integer NOT NULL,
          CONSTRAINT pk_resource_paths PRIMARY KEY (resource_id, org_id),
          CONSTRAINT fk_resource_paths_orgs_org_id FOREIGN KEY (org_id) REFERENCES orgs (id) ON DELETE CASCADE,
          CONSTRAINT fk_resource_paths_resources_resource_id_org_id FOREIGN KEY (resource_id, org_id) REFERENCES resources (id, org_id) ON DELETE CASCADE
      );

      CREATE TABLE role_permissions (
          role_id text NOT NULL,
          resource_id text NOT NULL,
          org_id text NOT NULL,
          action text NOT NULL,
          created_at timestamp with time zone NOT NULL,
          CONSTRAINT pk_role_permissions PRIMARY KEY (role_id, resource_id, org_id),
          CONSTRAINT fk_role_permissions_orgs_org_id FOREIGN KEY (org_id) REFERENCES orgs (id) ON DELETE CASCADE,
          CONSTRAINT fk_role_permissions_resources_resource_id_org_id FOREIGN KEY (resource_id, org_id) REFERENCES resources (id, org_id) ON DELETE CASCADE,
          CONSTRAINT fk_role_permissions_roles_role_id_org_id FOREIGN KEY (role_id, org_id) REFERENCES roles (id, org_id) ON DELETE CASCADE
      );

      CREATE TABLE role_properties (
          name text NOT NULL,
          role_id text NOT NULL,
          org_id text NOT NULL,
          value text NOT NULL,
          hidden boolean NOT NULL,
          created_at timestamp with time zone NOT NULL,
          CONSTRAINT pk_role_properties PRIMARY KEY (name, role_id, org_id),
          CONSTRAINT fk_role_properties_orgs_org_id FOREIGN KEY (org_id) REFERENCES orgs (id) ON DELETE CASCADE,
          CONSTRAINT fk_role_properties_roles_role_id_org_id FOREIGN KEY (role_id, org_id) REFERENCES roles (id, org_id) ON DELETE CASCADE
      );

      CREATE TABLE role_assignments (
          user_id text NOT NULL,
          role_id text NOT NULL,
          org_id text NOT NULL,
          created_at timestamp with time zone NOT NULL,
          CONSTRAINT pk_role_assignments PRIMARY KEY (role_id, user_id, org_id),
          CONSTRAINT fk_role_assignments_orgs_org_id FOREIGN KEY (org_id) REFERENCES orgs (id) ON DELETE CASCADE,
          CONSTRAINT fk_role_assignments_roles_role_id_org_id FOREIGN KEY (role_id, org_id) REFERENCES roles (id, org_id) ON DELETE CASCADE,
          CONSTRAINT fk_role_assignments_users_user_id_org_id FOREIGN KEY (user_id, org_id) REFERENCES users (id, org_id) ON DELETE CASCADE
      );

      CREATE TABLE user_permissions (
          user_id text NOT NULL,
          resource_id text NOT NULL,
          org_id text NOT NULL,
          action text NOT NULL,
          created_at timestamp with time zone NOT NULL,
          CONSTRAINT pk_user_permissions PRIMARY KEY (user_id, resource_id, org_id),
          CONSTRAINT fk_user_permissions_orgs_org_id FOREIGN KEY (org_id) REFERENCES orgs (id) ON DELETE CASCADE,
          CONSTRAINT fk_user_permissions_resources_resource_id_org_id FOREIGN KEY (resource_id, org_id) REFERENCES resources (id, org_id) ON DELETE CASCADE,
          CONSTRAINT fk_user_permissions_users_user_id_org_id FOREIGN KEY (user_id, org_id) REFERENCES users (id, org_id) ON DELETE CASCADE
      );

      CREATE TABLE user_properties (
          name text NOT NULL,
          user_id text NOT NULL,
          org_id text NOT NULL,
          value text NOT NULL,
          hidden boolean NOT NULL,
          created_at timestamp with time zone NOT NULL,
          CONSTRAINT pk_user_properties PRIMARY KEY (name, user_id, org_id),
          CONSTRAINT fk_user_properties_orgs_org_id FOREIGN KEY (org_id) REFERENCES orgs (id) ON DELETE CASCADE,
          CONSTRAINT fk_user_properties_users_user_id_org_id FOREIGN KEY (user_id, org_id) REFERENCES users (id, org_id) ON DELETE CASCADE
      );

      CREATE INDEX ix_org_properties_org_id ON org_properties (org_id);

      CREATE INDEX ix_resource_paths_org_id ON resource_paths (org_id);

      CREATE INDEX ix_resources_org_id ON resources (org_id);

      CREATE INDEX ix_role_assignments_org_id ON role_assignments (org_id);

      CREATE INDEX ix_role_assignments_role_id_org_id ON role_assignments (role_id, org_id);

      CREATE INDEX ix_role_assignments_user_id_org_id ON role_assignments (user_id, org_id);

      CREATE INDEX ix_role_permissions_org_id ON role_permissions (org_id);

      CREATE INDEX ix_role_permissions_resource_id_org_id ON role_permissions (resource_id, org_id);

      CREATE INDEX ix_role_permissions_role_id_org_id ON role_permissions (role_id, org_id);

      CREATE INDEX ix_role_properties_org_id ON role_properties (org_id);

      CREATE INDEX ix_role_properties_role_id_org_id ON role_properties (role_id, org_id);

      CREATE INDEX ix_roles_org_id ON roles (org_id);

      CREATE INDEX ix_user_permissions_org_id ON user_permissions (org_id);

      CREATE INDEX ix_user_permissions_resource_id_org_id ON user_permissions (resource_id, org_id);

      CREATE INDEX ix_user_permissions_user_id_org_id ON user_permissions (user_id, org_id);

      CREATE INDEX ix_user_properties_org_id ON user_properties (org_id);

      CREATE INDEX ix_user_properties_user_id_org_id ON user_properties (user_id, org_id);

      CREATE INDEX ix_users_org_id ON users (org_id);

      INSERT INTO "__EFMigrationsHistory" (migration_id, product_version)
      VALUES ('20230630163909_InitialCreate', '7.0.8');

      COMMIT;

      """;

  public static async void Init()
  {
    var context = new TankmanDbContext();

    using var transaction = context.Database.BeginTransaction(System.Data.IsolationLevel.Serializable);
    try
    {
      await context.Database.ExecuteSqlRawAsync(initSql);
      await transaction.CommitAsync();
    }
    catch (Exception ex)
    {
      Console.Write(ex.ToString());
      await transaction.RollbackAsync();
    }
  }
}