﻿using DinderWS.Models.Experience;
using DinderWS.Models.Match;
using DinderWS.Models.Profile;
using DinderWS.Models.Rejects;
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
        /// Contextual <see cref="Experience"/> entities.
        /// </summary>
        public DbSet<Experience> Experiences { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<Reject> Rejects { get; set; }

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
                entity.Property(model => model.Rating)
                    .HasColumnName("Rating")
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
                    .IsRequired(false);
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
                entity.ToTable("Profiles");
            });
            // ==================================================
            // Experience entity 
            // ==================================================
            builder.Entity<Experience>(entity => {
                entity.Property(model => model.Id)
                    .HasColumnName("Id")
                    .HasMaxLength(450)
                    .IsUnicode()
                    .IsRequired();
                entity.Property(model => model.CuisineType)
                    .HasColumnName("CuisineType")
                    .IsRequired();
                entity.Property(model => model.GroupSize)
                    .HasColumnName("GroupSize")
                    .IsRequired();
                entity.Property(model => model.GenderPreference)
                    .HasColumnName("GenderPreference")
                    .IsRequired();
                entity.Property(model => model.Longitude)
                    .HasColumnName("LocationLong")
                    .IsRequired();
                entity.Property(model => model.Latitude)
                    .HasColumnName("LocationLat")
                    .IsRequired();
                entity.Property(model => model.Timestamp)
                    .HasColumnName("Timestamp")
                    .IsRequired();
                entity.HasOne(model => model.Identity)
                    .WithOne()
                    .HasForeignKey<Experience>(model => model.Id)
                    .HasConstraintName("FK_Identity-Experience")
                    .OnDelete(DeleteBehavior.ClientCascade);
                entity.HasOne(model => model.Profile)
                    .WithOne(other => other.Experience)
                    .HasForeignKey<Experience>(model => model.Id)
                    .HasConstraintName("FK_Profile-Experience")
                    .OnDelete(DeleteBehavior.ClientCascade);
                entity.HasMany(model => model.Rejects)
                    .WithOne(other => other.Experience)
                    .HasForeignKey(other => other.ExperienceId)
                    .HasConstraintName("FK_Experience-Reject")
                    .OnDelete(DeleteBehavior.ClientCascade);
                entity.HasKey(model => model.Id);
                entity.ToTable("Experiences");
            });
            // ==================================================
            // Match entity 
            // ==================================================
            builder.Entity<Match>(entity => {
                entity.Property(model => model.Id)
                    .UseIdentityColumn()
                    .IsRequired();
                entity.Property(model => model.GroupSize)
                    .HasColumnName("GroupSize")
                    .IsRequired();
                entity.Property(model => model.Gender)
                    .HasColumnName("Gender")
                    .IsRequired();
                entity.Property(model => model.CuisineType)
                    .HasColumnName("CuisineType")
                    .IsRequired();
                entity.Property(model => model.Timestamp)
                    .HasColumnName("Timestamp")
                    .IsRequired();
                entity.Property(model => model.AvgLongitude)
                    .HasColumnName("AvgLongitude")
                    .IsRequired();
                entity.Property(model => model.AvgLatitude)
                    .HasColumnName("AvgLatitude")
                    .IsRequired();
                entity.HasMany(model => model.Experiences)
                    .WithOne(other => other.Match)
                    .HasForeignKey(other => other.MatchId)
                    .HasConstraintName("FK_Experience-Match")
                    .OnDelete(DeleteBehavior.ClientCascade);
                entity.HasMany(model => model.Rejects)
                    .WithOne(other => other.Match)
                    .HasForeignKey(other => other.MatchId)
                    .HasConstraintName("FK_Match-Reject")
                    .OnDelete(DeleteBehavior.ClientCascade);
                entity.HasKey(Model => Model.Id);
                entity.ToTable("Matches");
            });
            // ==================================================
            // Reject relationship 
            // ==================================================
            builder.Entity<Reject>(entity => {
                entity.Property(model => model.ExperienceId)
                    .HasColumnName("ExperienceId")
                    .HasMaxLength(450)
                    .IsUnicode()
                    .IsRequired();
                entity.Property(model => model.MatchId)
                    .HasColumnName("MatchId")
                    .IsRequired();
                entity.Property(model => model.Timestamp)
                    .HasColumnName("Timestamp")
                    .IsRequired();
                entity.HasKey(model => new { model.ExperienceId, model.MatchId });
            });
        }
    }
}
