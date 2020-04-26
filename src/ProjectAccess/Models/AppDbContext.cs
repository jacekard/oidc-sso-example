namespace ProjectAccess.Models
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    using ProjectAccess.Areas.Identity.Data;

    public class AppDbContext : IdentityDbContext<AppUser, AppRole, long,
        IdentityUserClaim<long>, AppUserRole, IdentityUserLogin<long>,
        IdentityRoleClaim<long>, IdentityUserToken<long>>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Task> Tasks { get; set; }

        public DbSet<Project> Projects { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AppUser>(b =>
            {
                b.ToTable("Users");
            });

            builder.Entity<AppRole>(b =>
            {
                b.ToTable("Roles");
            });

            builder.Entity<AppUserRole>(b =>
            {
                b.ToTable("UserRoles");
            });

            builder.Entity<AppUser>(b =>
            {
                b.HasMany(e => e.Claims)
                    .WithOne()
                    .HasForeignKey(uc => uc.UserId)
                    .IsRequired();

                b.HasMany(e => e.Logins)
                    .WithOne()
                    .HasForeignKey(ul => ul.UserId)
                    .IsRequired();

                b.HasMany(e => e.Tokens)
                    .WithOne()
                    .HasForeignKey(ut => ut.UserId)
                    .IsRequired();

                b.HasMany(e => e.UserRoles)
                    .WithOne(e => e.User)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();

                b.HasMany(e => e.UserProjects)
                    .WithOne(e => e.User)
                    .HasForeignKey(fk => fk.UserId);
            });

            builder.Entity<AppRole>(b =>
            {
                b.HasMany(e => e.UserRoles)
                    .WithOne(e => e.Role)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();
            });

            builder.Entity<Project>(b =>
            {
                b.HasMany(e => e.UserProjects)
                    .WithOne(e => e.Project)
                    .HasForeignKey(fk => fk.ProjectId)
                    .OnDelete(DeleteBehavior.Cascade);

                b.HasMany(e => e.Tasks)
                    .WithOne(e => e.Project)
                    .OnDelete(DeleteBehavior.Cascade);

                b.Property(w => w.EndDate).IsRequired(false);
            });

            builder.Entity<AppUserProjects>()
                .HasKey(t => new { t.UserId, t.ProjectId });
        }
    }
}
