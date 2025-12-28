using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using VehicalApi.Entity;

namespace VehicalApi.Data;

public partial class MyDbContext : DbContext
{
    public MyDbContext()
    {
    }

    public MyDbContext(DbContextOptions<MyDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Showroom> Showrooms { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Vehicle> Vehicles { get; set; }

    public virtual DbSet<VehicleImage> VehicleImages { get; set; }

    public virtual DbSet<VehicleSpecification> VehicleSpecifications { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=VehicleShowroom;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Showroom>(entity =>
        {
            entity.HasKey(e => e.ShowroomId).HasName("PK__Showroom__A7726CBB0A7E547B");

            entity.HasOne(d => d.Manager).WithMany(p => p.Showrooms)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Showrooms__Manag__6383C8BA");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4CF023086F");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Role).UseCollation("Latin1_General_CS_AS");
        });

        modelBuilder.Entity<Vehicle>(entity =>
        {
            entity.HasKey(e => e.VehicalId).HasName("PK__Vehicle__1D89FCE1D3A942A5");

            entity.Property(e => e.AgeInShowroom).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Showroom).WithMany(p => p.Vehicles)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Vehicle__Showroo__71D1E811");
        });

        modelBuilder.Entity<VehicleImage>(entity =>
        {
            entity.HasKey(e => e.ImageId).HasName("PK__VehicleI__7516F70C3988B832");

            entity.HasOne(d => d.Vehicle).WithMany(p => p.VehicleImages)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__VehicleIm__Vehic__02FC7413");
        });

        modelBuilder.Entity<VehicleSpecification>(entity =>
        {
            entity.HasKey(e => e.SpecificationId).HasName("PK__VehicleS__FC1C560A6A276819");

            entity.HasOne(d => d.Vehical).WithMany(p => p.VehicleSpecifications)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__VehicleSp__vehic__05D8E0BE");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
