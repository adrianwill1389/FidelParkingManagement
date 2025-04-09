using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace FidelParkingManagement.Models;

public partial class FidelParkingDbContext : DbContext
{
    public FidelParkingDbContext()
    {
    }

    public FidelParkingDbContext(DbContextOptions<FidelParkingDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Medium> Media { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<UserAccount> UserAccounts { get; set; }

    public virtual DbSet<VehiclesDetected> VehiclesDetecteds { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=23.95.235.16;Database=Fidel_Parking_Management_System;User=vtdi_student;Password=P@ssword1;TrustServerCertificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Medium>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Url)
                .HasMaxLength(50)
                .HasColumnName("url");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Paid).HasColumnType("money");
            entity.Property(e => e.TimeStamp).HasColumnType("datetime");
        });

        modelBuilder.Entity<UserAccount>(entity =>
        {
            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.ProfilePhotoUrl)
                .HasMaxLength(100)
                .HasColumnName("ProfilePhotoURL");
            entity.Property(e => e.Role)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.UserName).HasMaxLength(50);
        });

        modelBuilder.Entity<VehiclesDetected>(entity =>
        {
            entity.HasKey(e => e.TicketNumber);

            entity.ToTable("VehiclesDetected");

            entity.Property(e => e.Color).HasMaxLength(50);
            entity.Property(e => e.LicensePlateNumber).HasMaxLength(50);
            entity.Property(e => e.Make).HasMaxLength(50);
            entity.Property(e => e.Model).HasMaxLength(50);
            entity.Property(e => e.Operation).HasMaxLength(50);

            entity.HasOne(d => d.Media).WithMany(p => p.VehiclesDetecteds)
                .HasForeignKey(d => d.MediaId)
                .HasConstraintName("FK_VehiclesDetected_Media");

            entity.HasOne(d => d.Payment).WithMany(p => p.VehiclesDetecteds)
                .HasForeignKey(d => d.PaymentId)
                .HasConstraintName("FK_VehiclesDetected_Payments");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
