
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

    public virtual DbSet<RideBooking> RideBookings { get; set; }

    public virtual DbSet<Showroom> Showrooms { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Vehicle> Vehicles { get; set; }

    public virtual DbSet<VehicleImage> VehicleImages { get; set; }

    public virtual DbSet<VehicleSpecification> VehicleSpecifications { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//     {
//         if (!optionsBuilder.IsConfigured)
//         {
//             optionsBuilder.UseSqlServer(
//                 "Server=localhost\\SQLEXPRESS;Database=VehicleShowroom;Trusted_Connection=True;TrustServerCertificate=True;"
//             );
//         }
//     }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<Showroom>(entity =>
        {
            entity.HasKey(e => e.ShowroomId).HasName("PK__Showroom__A7726CBB0A7E547B");

            entity.HasIndex(e => e.ContactNumber, "UQ__Showroom__570665C6F41D4DD4").IsUnique();

            entity.HasIndex(e => e.ShowroomName, "UQ__Showroom__C46B526F0EFC64D4").IsUnique();

            entity.Property(e => e.ContactNumber)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.ShowroomLocation)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ShowroomName)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Manager).WithMany(p => p.Showrooms)
                .HasForeignKey(d => d.ManagerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Showrooms__Manag__6383C8BA");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4CF023086F");

            entity.HasIndex(e => e.Email, "UQ__Users__A9D1053468E7FA6E").IsUnique();

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("createdAt");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FullName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Number)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.RefreshToken)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.RefreshTokenExpiry).HasColumnType("datetime");
            entity.Property(e => e.Role)
                .HasMaxLength(20)
                .IsUnicode(false)
                .UseCollation("Latin1_General_CS_AS");
        });

        modelBuilder.Entity<Vehicle>(entity =>
        {
            entity.HasKey(e => e.VehicalId).HasName("PK__Vehicle__1D89FCE1D3A942A5");

            entity.ToTable("Vehicle");

            entity.HasIndex(e => e.AgeInShowroom, "IX_Vehicle_AgeInShowroom");

            entity.HasIndex(e => e.BasePrice, "IX_Vehicle_BasePrice");

            entity.HasIndex(e => e.VehicleName, "IX_Vehicle_VehicleName");

            entity.Property(e => e.AgeInShowroom)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Model)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ShortDescription)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("shortDescription");
            entity.Property(e => e.StockCount).HasColumnName("stockCount");
            entity.Property(e => e.VehicleName)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Showroom).WithMany(p => p.Vehicles)
                .HasForeignKey(d => d.ShowroomId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Vehicle__Showroo__71D1E811");
        });

        modelBuilder.Entity<VehicleImage>(entity =>
        {
            entity.HasKey(e => e.ImageId).HasName("PK__VehicleI__7516F70C3988B832");

            entity.ToTable("VehicleImage");

            entity.Property(e => e.ImageLocation)
                .HasMaxLength(150)
                .IsUnicode(false);

            entity.HasOne(d => d.Vehicle).WithMany(p => p.VehicleImages)
                .HasForeignKey(d => d.VehicleId)
                .HasConstraintName("FK_VehicleImage_Vehicle");
        });

        modelBuilder.Entity<VehicleSpecification>(entity =>
        {
            entity.HasKey(e => e.SpecificationId).HasName("PK__VehicleS__FC1C560A6A276819");

            entity.HasIndex(e => e.BodyType, "IX_VehicleSpecifications_BodyType");

            entity.HasIndex(e => e.Engine, "IX_VehicleSpecifications_Engine");

            entity.HasIndex(e => e.FuleType, "IX_VehicleSpecifications_FuelType");

            entity.HasIndex(e => e.VehicalId, "IX_VehicleSpecifications_VehicleId");

            entity.Property(e => e.SpecificationId).HasColumnName("specificationId");
            entity.Property(e => e.BodyType)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Engine).HasColumnName("engine");
            entity.Property(e => e.FuleType)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("fuleType");
            entity.Property(e => e.Mileage)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.PowerOfvehical)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Torque)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.VehicalId).HasColumnName("vehicalId");

            entity.HasOne(d => d.Vehical).WithMany(p => p.VehicleSpecifications)
                .HasForeignKey(d => d.VehicalId)
                .HasConstraintName("FK_VehicleSpecification_Vehicle");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
