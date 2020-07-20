using System;
using System.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace LegalAffairs.Models
{
    public partial class LegalAffairsDbContext : DbContext
    {
        public LegalAffairsDbContext()
        {
        }

        public LegalAffairsDbContext(DbContextOptions<LegalAffairsDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<CaseOwner> CaseOwners { get; set; }
        public virtual DbSet<Case> Cases { get; set; }
        public virtual DbSet<InBook> InBooks { get; set; }
        public virtual DbSet<Movement> Movements { get; set; }
        public virtual DbSet<OutBook> OutBooks { get; set; }
        public virtual DbSet<Reading> Readings { get; set; }
        public virtual DbSet<Rule> Rules { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["LegalConnectionString"].ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CaseOwner>(entity =>
            {
                entity.Property(e => e.LatestUpdateTimestamp)
                    .IsRowVersion()
                    .IsConcurrencyToken();

                entity.HasOne(d => d.LatestUpdateUser)
                    .WithMany(p => p.CaseOwners)
                    .HasForeignKey(d => d.LatestUpdateUserId)
                    .HasConstraintName("FK_case_owner_user");
            });

            modelBuilder.Entity<Case>(entity =>
            {
                entity.HasKey(e => new { e.CaseYear, e.AnnualSerialNumber })
                    .HasName("PK_case");

                entity.Property(e => e.LatestUpdateTimestamp)
                    .IsRowVersion()
                    .IsConcurrencyToken();

                entity.HasOne(d => d.CaseOwner)
                    .WithMany(p => p.Cases)
                    .HasForeignKey(d => d.CaseOwnerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_case_case_owner");

                entity.HasOne(d => d.LatestUpdateUser)
                    .WithMany(p => p.Cases)
                    .HasForeignKey(d => d.LatestUpdateUserId)
                    .HasConstraintName("FK_case_user");
            });

            modelBuilder.Entity<InBook>(entity =>
            {
                entity.HasKey(e => new { e.InYear, e.InSerialNumber })
                    .HasName("PK_in_book");

                entity.Property(e => e.LatestUpdateTimestamp)
                    .IsRowVersion()
                    .IsConcurrencyToken();

                entity.HasOne(d => d.LatestUpdateUser)
                    .WithMany(p => p.InBooks)
                    .HasForeignKey(d => d.LatestUpdateUserId)
                    .HasConstraintName("FK_in_book_user");
            });

            modelBuilder.Entity<Movement>(entity =>
            {
                entity.HasKey(e => new { e.SerialNumber, e.CaseYear, e.AnnualSerialNumber })
                    .HasName("PK_movement");

                entity.Property(e => e.SerialNumber).ValueGeneratedOnAdd();

                entity.Property(e => e.LatestUpdateTimestamp)
                    .IsRowVersion()
                    .IsConcurrencyToken();

                entity.HasOne(d => d.LatestUpdateUser)
                    .WithMany(p => p.Movements)
                    .HasForeignKey(d => d.LatestUpdateUserId)
                    .HasConstraintName("FK_movement_user");

                entity.HasOne(d => d.Cases)
                    .WithMany(p => p.Movements)
                    .HasForeignKey(d => new { d.CaseYear, d.AnnualSerialNumber })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_movement_case");

                entity.HasOne(d => d.In)
                    .WithMany(p => p.Movements)
                    .HasForeignKey(d => new { d.InYear, d.InSerialNumber })
                    .HasConstraintName("FK_movement_in_book");

                entity.HasOne(d => d.Out)
                    .WithMany(p => p.Movements)
                    .HasForeignKey(d => new { d.OutYear, d.OutSerialNumber })
                    .HasConstraintName("FK_movement_out_book");
            });

            modelBuilder.Entity<OutBook>(entity =>
            {
                entity.HasKey(e => new { e.OutYear, e.OutSerialNumber })
                    .HasName("PK_out_book");

                entity.Property(e => e.LatestUpdateTimestamp)
                    .IsRowVersion()
                    .IsConcurrencyToken();

                entity.HasOne(d => d.LatestUpdateUser)
                    .WithMany(p => p.OutBooks)
                    .HasForeignKey(d => d.LatestUpdateUserId)
                    .HasConstraintName("FK_out_book_user");
            });

            modelBuilder.Entity<Reading>(entity =>
            {
                entity.Property(e => e.LatestUpdateTimestamp)
                    .IsRowVersion()
                    .IsConcurrencyToken();

                entity.HasOne(d => d.LatestUpdateUser)
                    .WithMany(p => p.Readings)
                    .HasForeignKey(d => d.LatestUpdateUserId)
                    .HasConstraintName("FK_readings_user");

                entity.HasOne(d => d.Out)
                    .WithMany(p => p.Readings)
                    .HasForeignKey(d => new { d.OutYear, d.OutSerialNumber })
                    .HasConstraintName("FK_readings_out_book");
            });

            modelBuilder.Entity<Rule>(entity =>
            {
                entity.HasKey(e => new { e.RuleYear, e.AnnualSerialNumber })
                    .HasName("PK_rule");

                entity.Property(e => e.LatestUpdateTimestamp)
                    .IsRowVersion()
                    .IsConcurrencyToken();

                entity.HasOne(d => d.LatestUpdateUser)
                    .WithMany(p => p.Rules)
                    .HasForeignKey(d => d.LatestUpdateUserId)
                    .HasConstraintName("FK_rule_user");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
