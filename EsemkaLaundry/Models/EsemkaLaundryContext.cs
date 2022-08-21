using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EsemkaLaundry.Models
{
    public partial class EsemkaLaundryContext : DbContext
    {
        public class UserCradential
        {
            public string Email { get; set; }
        }

        public string getUser(string token)
        {
            try
            {
                if (token != null)
                {
                    var bearer = new JwtSecurityToken(token.Replace("Bearer", string.Empty).Trim());
                    var payload = JsonSerializer.Serialize(bearer.Payload);
                    var result = JsonSerializer.Deserialize<UserCradential>(payload);
                    return result.Email;
                }
            }
            catch { }
            return null;
        }

        public EsemkaLaundryContext()
        {
        }

        public EsemkaLaundryContext(DbContextOptions<EsemkaLaundryContext> options)
        : base(options)
        {
        }

        public virtual DbSet<DetailDeposit> DetailDeposits { get; set; } = null!;
        public virtual DbSet<HeaderDeposit> HeaderDeposits { get; set; } = null!;
        public virtual DbSet<Package> Packages { get; set; } = null!;
        public virtual DbSet<PackageTransaction> PackageTransactions { get; set; } = null!;
        public virtual DbSet<Service> Services { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=.\\IT;Database=EsemkaLaundry;Trusted_Connection=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DetailDeposit>(entity =>
            {
                entity.HasIndex(e => e.HeaderDepositId, "IX_DetailDeposits_HeaderDepositId");

                entity.HasIndex(e => e.PackageTransactionId, "IX_DetailDeposits_PackageTransactionId");

                entity.HasIndex(e => e.ServiceId, "IX_DetailDeposits_ServiceId");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.HeaderDeposit)
                    .WithMany(p => p.DetailDeposits)
                    .HasForeignKey(d => d.HeaderDepositId);

                entity.HasOne(d => d.PackageTransaction)
                    .WithMany(p => p.DetailDeposits)
                    .HasForeignKey(d => d.PackageTransactionId);

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.DetailDeposits)
                    .HasForeignKey(d => d.ServiceId);
            });

            modelBuilder.Entity<HeaderDeposit>(entity =>
            {
                entity.HasIndex(e => e.CustomerEmail, "IX_HeaderDeposits_CustomerEmail");

                entity.HasIndex(e => e.EmployeeEmail, "IX_HeaderDeposits_EmployeeEmail");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CustomerEmail).HasMaxLength(100);

                entity.Property(e => e.EmployeeEmail).HasMaxLength(100);

                entity.HasOne(d => d.CustomerEmailNavigation)
                    .WithMany(p => p.HeaderDepositCustomerEmailNavigations)
                    .HasForeignKey(d => d.CustomerEmail)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.EmployeeEmailNavigation)
                    .WithMany(p => p.HeaderDepositEmployeeEmailNavigations)
                    .HasForeignKey(d => d.EmployeeEmail)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Package>(entity =>
            {
                entity.HasIndex(e => e.ServiceId, "IX_Packages_ServiceId");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.Packages)
                    .HasForeignKey(d => d.ServiceId);
            });

            modelBuilder.Entity<PackageTransaction>(entity =>
            {
                entity.HasIndex(e => e.PackageId, "IX_PackageTransactions_PackageId");

                entity.HasIndex(e => e.UserEmail, "IX_PackageTransactions_UserEmail");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.UserEmail).HasMaxLength(100);

                entity.HasOne(d => d.Package)
                    .WithMany(p => p.PackageTransactions)
                    .HasForeignKey(d => d.PackageId);
            });

            modelBuilder.Entity<Service>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Email);

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.Address).HasMaxLength(300);

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.Password).HasMaxLength(200);

                entity.Property(e => e.PhoneNumber).HasMaxLength(20);

                entity.Property(e => e.PhotoPath).HasMaxLength(200);
            });

            //OnModelCreatingPartial(modelBuilder);
        }

        //private partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}