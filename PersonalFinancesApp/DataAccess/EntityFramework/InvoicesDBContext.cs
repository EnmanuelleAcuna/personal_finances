using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace REST_API.DataAccess.EntityFramework
{
    public partial class InvoicesDBContext : DbContext
    {
        public InvoicesDBContext()
        {
        }

        public InvoicesDBContext(DbContextOptions<InvoicesDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Categories> Categories { get; set; } = null!;
        public virtual DbSet<Invoices> Invoices { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Name=Invoices");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Categories>(entity =>
            {
                entity.ToTable("CATEGORIES", "fin");

                entity.HasIndex(e => e.Nombre, "UQ_CATEGORIES_NOMBRE")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Nombre)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Invoices>(entity =>
            {
                entity.ToTable("INVOICES", "fin");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Amount).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Detail)
                    .HasMaxLength(4000)
                    .IsUnicode(false);

                entity.Property(e => e.InvoiceDate)
                    .HasColumnType("datetime")
                    .HasColumnName("Invoice_Date");

                entity.Property(e => e.Payee)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.PaymentMethod)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("Payment_Method");

                entity.HasOne(d => d.CategoryNavigation)
                    .WithMany(p => p.Invoices)
                    .HasForeignKey(d => d.Category)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_INVOICES_CATEGORY");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
