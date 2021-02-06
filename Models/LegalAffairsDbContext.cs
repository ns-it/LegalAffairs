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
        public virtual DbSet<RuleAttachement> RuleAttachements { get; set; }
        public virtual DbSet<Issuer> Issuers { get; set; }
        public virtual DbSet<Topic> Topics { get; set; }



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
                entity.Property(e => e.FullName).HasComputedColumnSql("(concat([first_name],' ',[middle_name],' ',[last_name]))");

                entity.HasOne(d => d.LatestUpdateUser)
                    .WithMany(p => p.CaseOwners)
                    .HasForeignKey(d => d.LatestUpdateUserId)
                    .HasConstraintName("FK_case_owner_user");
            });

            modelBuilder.Entity<Case>(entity =>
            {
                entity.HasKey(e => new { e.CaseYear, e.AnnualSerialNumber })
                    .HasName("PK_case");

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

            modelBuilder.Entity<Issuer>(entity =>
            {
                entity.HasKey(e => e.IssuerId)
                    .HasName("PK_Table_1");

                entity.Property(e => e.IssuerId).ValueGeneratedNever();
            });

            modelBuilder.Entity<InBook>(entity =>
            {
                entity.HasKey(e => new { e.InYear, e.InSerialNumber })
                    .HasName("PK_in_book");

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

                entity.HasOne(d => d.LatestUpdateUser)
                    .WithMany(p => p.OutBooks)
                    .HasForeignKey(d => d.LatestUpdateUserId)
                    .HasConstraintName("FK_out_book_user");
            });

            modelBuilder.Entity<Reading>(entity =>
            {
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
                entity.HasKey(e => e.RuleId)
                .HasName("PK_rule");

                entity.HasIndex(e => new { e.IssuerId, e.RuleYear, e.AnnualSerialNumber })
                    .HasName("IX_rules")
                    .IsUnique();

                entity.HasOne(d => d.LatestUpdateUser)
                    .WithMany(p => p.Rules)
                    .HasForeignKey(d => d.LatestUpdateUserId)
                    .HasConstraintName("FK_rule_user");

                entity.HasOne(d => d.Issuer)
                 .WithMany(p => p.Rules)
                 .HasForeignKey(d => d.IssuerId)
                 .HasConstraintName("FK_rules_issuers");

                entity.HasOne(d => d.Topic)
                   .WithMany(p => p.Rules)
                   .HasForeignKey(d => d.TopicId)
                   .HasConstraintName("FK_rules_topics");
            });

            modelBuilder.Entity<RuleAttachement>(entity =>
            {
                entity.HasKey(e => new { e.RuleId, e.AttachmentNumber })
                 .HasName("PK_rules_attachements");

                entity.HasOne(d => d.Rule)
                    .WithMany(p => p.RuleAttachements)
                    .HasForeignKey(d => d.RuleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_rule_attachements_rules");
            });


            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.FullName).HasComputedColumnSql("(concat([first_name],' ',[last_name]))");
            });

            modelBuilder.Entity<Topic>(entity =>
            {
                entity.Property(e => e.TopicId).ValueGeneratedNever();

                entity.HasOne(d => d.ParentTopic)
                    .WithMany(p => p.Children)
                    .HasForeignKey(d => d.ParentTopicId)
                    .HasConstraintName("FK_topics_topics");
            });


            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
