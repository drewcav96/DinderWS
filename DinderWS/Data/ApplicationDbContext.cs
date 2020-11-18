using DinderWS.Models.Profile;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DinderWS.Data {
    /// <summary>
    /// The Application Database Context; the bridge between object instances and their database entities in SQL server.
    /// </summary>
    public sealed class ApplicationDbContext : IdentityDbContext {
        private readonly ILogger<ApplicationDbContext> _log;

        /// <summary>
        /// Contextual <see cref="Profile"/> entities.
        /// </summary>
        public DbSet<Profile> Profiles { get; set; }

        /// <summary>
        /// Instantiates the Application Database Context with dependency injection.
        /// </summary>
        /// <param name="logger">The Logger instance</param>
        /// <param name="options">The Database Context options.</param>
        public ApplicationDbContext(ILogger<ApplicationDbContext> logger,
                DbContextOptions<ApplicationDbContext> options)
                    : base(options) {
            _log = logger;
        }

        protected override void OnModelCreating(ModelBuilder builder) {
            base.OnModelCreating(builder);
            _log.LogInformation($"Application Database Context connected to {Database.ProviderName}.");
            // ==================================================
            // Profile entity 
            // ==================================================
            builder.Entity<Profile>(entity => {
                entity.Property(model => model.Id)
                    .HasColumnName("Id")
                    .HasMaxLength(450)
                    .IsUnicode()
                    .IsRequired();
                entity.Property(model => model.Firstname)
                    .HasColumnName("Firstname")
                    .HasMaxLength(35)
                    .IsUnicode()
                    .IsRequired();
                entity.Property(model => model.Lastname)
                    .HasColumnName("Lastname")
                    .HasMaxLength(35)
                    .IsUnicode()
                    .IsRequired();
                entity.Property(model => model.Gender)
                    .HasColumnName("Gender")
                    .IsRequired();
                entity.Property(model => model.AvatarUrl)
                    .HasColumnName("AvatarUrl")
                    .HasMaxLength(2048)
                    .IsUnicode(false)
                    .IsRequired();
                entity.Property(model => model.DietaryRestrictions)
                    .HasColumnName("DietaryRestrictions")
                    .IsRequired();
                entity.Property(model => model.Interests)
                    .HasColumnName("Interests")
                    .IsRequired();
                entity.HasOne(model => model.Identity)
                    .WithOne()
                    .HasForeignKey<Profile>(model => model.Id)
                    .HasConstraintName("FK_Identity-Profile")
                    .OnDelete(DeleteBehavior.ClientCascade);
                entity.HasKey(model => model.Id);
                entity.ToTable("AspNetProfiles");
            });
        }
    }
}
