using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace mojoin.Models;

public partial class DbmojoinContext : DbContext
{
    public DbmojoinContext()
    {
    }

    public DbmojoinContext(DbContextOptions<DbmojoinContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Banner> Banners { get; set; }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<District> Districts { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Furniture> Furnitures { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Room> Rooms { get; set; }

    public virtual DbSet<RoomAddress> RoomAddresses { get; set; }

    public virtual DbSet<RoomFurniture> RoomFurnitures { get; set; }

    public virtual DbSet<RoomRating> RoomRatings { get; set; }

    public virtual DbSet<RoomReport> RoomReports { get; set; }

    public virtual DbSet<RoomStatus> RoomStatuses { get; set; }

    public virtual DbSet<RoomType> RoomTypes { get; set; }

    public virtual DbSet<RoomUtility> RoomUtilities { get; set; }

    public virtual DbSet<Street> Streets { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserAccount> UserAccounts { get; set; }

    public virtual DbSet<Utility> Utilities { get; set; }

    public virtual DbSet<Ward> Wards { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=MRTHAWNG; Database=dbmojoin;Integrated Security=true; TrustServerCertificate=True;");
    ///hereeeeeeeeee
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Banner>(entity =>
        {
            entity.HasKey(e => e.BannerlD);

            entity.Property(e => e.BannerlD).ValueGeneratedNever();
            entity.Property(e => e.BannerName).HasMaxLength(100);
            entity.Property(e => e.Image).IsUnicode(false);
        });

        modelBuilder.Entity<City>(entity =>
        {
            entity.ToTable("City");

            entity.Property(e => e.CityId).HasColumnName("CityID");
            entity.Property(e => e.CityName).HasMaxLength(100);
        });

        modelBuilder.Entity<District>(entity =>
        {
            entity.ToTable("District");

            entity.Property(e => e.DistrictId).HasColumnName("DistrictID");
            entity.Property(e => e.CityId).HasColumnName("CityID");
            entity.Property(e => e.DistrictName).HasMaxLength(100);

            entity.HasOne(d => d.City).WithMany(p => p.Districts)
                .HasForeignKey(d => d.CityId)
                .HasConstraintName("FK_District_City");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
            entity.Property(e => e.AccountId).HasColumnName("AccountID");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FacebookInfo)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.ZaloInfo)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Account).WithMany(p => p.Employees)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK_Employees_UserAccounts");
        });

        modelBuilder.Entity<Furniture>(entity =>
        {
            entity.HasKey(e => e.FurniturelD);

            entity.Property(e => e.FurbitureName).HasMaxLength(100);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.RoleName).HasMaxLength(50);
        });

        modelBuilder.Entity<Room>(entity =>
        {
            entity.Property(e => e.RoomId).HasColumnName("RoomID");
            entity.Property(e => e.AddressId).HasColumnName("AddressID");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Image1).IsUnicode(false);
            entity.Property(e => e.Image2).IsUnicode(false);
            entity.Property(e => e.Image3).IsUnicode(false);
            entity.Property(e => e.Image4).IsUnicode(false);
            entity.Property(e => e.LastUpdate).HasColumnType("datetime");
            entity.Property(e => e.RoomTypeId).HasColumnName("RoomTypeID");
            entity.Property(e => e.StatusId).HasColumnName("StatusID");
            entity.Property(e => e.Title).HasMaxLength(100);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Address).WithMany(p => p.Rooms)
                .HasForeignKey(d => d.AddressId)
                .HasConstraintName("FK_Rooms_RoomAddresses");

            entity.HasOne(d => d.RoomType).WithMany(p => p.Rooms)
                .HasForeignKey(d => d.RoomTypeId)
                .HasConstraintName("FK_Rooms_RoomTypes");

            entity.HasOne(d => d.Status).WithMany(p => p.Rooms)
                .HasForeignKey(d => d.StatusId)
                .HasConstraintName("FK_Rooms_RoomStatuses");

            entity.HasOne(d => d.User).WithMany(p => p.Rooms)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Rooms_Users");
        });

        modelBuilder.Entity<RoomAddress>(entity =>
        {
            entity.HasKey(e => e.AddressId);

            entity.Property(e => e.AddressId).HasColumnName("AddressID");
            entity.Property(e => e.Address).HasMaxLength(20);
            entity.Property(e => e.City).HasMaxLength(100);
            entity.Property(e => e.District).HasMaxLength(100);
            entity.Property(e => e.StreetName).HasMaxLength(100);
            entity.Property(e => e.Ward).HasMaxLength(100);
        });

        modelBuilder.Entity<RoomFurniture>(entity =>
        {
            entity.Property(e => e.RoomFurnitureId).HasColumnName("RoomFurnitureID");
            entity.Property(e => e.RoomId).HasColumnName("RoomID");

            entity.HasOne(d => d.FurniturelDNavigation).WithMany(p => p.RoomFurnitures)
                .HasForeignKey(d => d.FurniturelD)
                .HasConstraintName("FK_RoomFurnitures_Furnitures");

            entity.HasOne(d => d.Room).WithMany(p => p.RoomFurnitures)
                .HasForeignKey(d => d.RoomId)
                .HasConstraintName("FK_RoomFurnitures_Rooms");
        });

        modelBuilder.Entity<RoomRating>(entity =>
        {
            entity.HasKey(e => e.RatingId);

            entity.Property(e => e.RatingId).HasColumnName("RatingID");
            entity.Property(e => e.RoomId).HasColumnName("RoomID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Room).WithMany(p => p.RoomRatings)
                .HasForeignKey(d => d.RoomId)
                .HasConstraintName("FK_RoomRatings_Rooms");

            entity.HasOne(d => d.User).WithMany(p => p.RoomRatings)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_RoomRatings_Users");
        });

        modelBuilder.Entity<RoomReport>(entity =>
        {
            entity.HasKey(e => e.ReportId);

            entity.Property(e => e.ReportId).HasColumnName("ReportID");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.RoomId).HasColumnName("RoomID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Room).WithMany(p => p.RoomReports)
                .HasForeignKey(d => d.RoomId)
                .HasConstraintName("FK_RoomReports_Rooms");

            entity.HasOne(d => d.User).WithMany(p => p.RoomReports)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_RoomReports_Users");
        });

        modelBuilder.Entity<RoomStatus>(entity =>
        {
            entity.HasKey(e => e.StatusId);

            entity.Property(e => e.StatusId).HasColumnName("StatusID");
            entity.Property(e => e.StatusName).HasMaxLength(50);
        });

        modelBuilder.Entity<RoomType>(entity =>
        {
            entity.Property(e => e.RoomTypeId).HasColumnName("RoomTypeID");
            entity.Property(e => e.TypeName).HasMaxLength(50);
        });

        modelBuilder.Entity<RoomUtility>(entity =>
        {
            entity.HasKey(e => e.RoomUtilities);

            entity.Property(e => e.RoomId).HasColumnName("RoomID");
            entity.Property(e => e.UtilityId).HasColumnName("UtilityID");

            entity.HasOne(d => d.Room).WithMany(p => p.RoomUtilities)
                .HasForeignKey(d => d.RoomId)
                .HasConstraintName("FK_RoomUtilities_Rooms");

            entity.HasOne(d => d.Utility).WithMany(p => p.RoomUtilities)
                .HasForeignKey(d => d.UtilityId)
                .HasConstraintName("FK_RoomUtilities_Utilities");
        });

        modelBuilder.Entity<Street>(entity =>
        {
            entity.ToTable("Street");

            entity.Property(e => e.StreetId).HasColumnName("StreetID");
            entity.Property(e => e.StreetName).HasMaxLength(100);
            entity.Property(e => e.WardId).HasColumnName("WardID");

            entity.HasOne(d => d.Ward).WithMany(p => p.Streets)
                .HasForeignKey(d => d.WardId)
                .HasConstraintName("FK_Street_Ward");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.AccountId).HasColumnName("AccountID");
            entity.Property(e => e.Dateofbirth).HasColumnType("date");
            entity.Property(e => e.Email)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.FullName).HasMaxLength(50);

            entity.HasOne(d => d.Account).WithMany(p => p.Users)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK_Users_UserAccounts");
        });

        modelBuilder.Entity<UserAccount>(entity =>
        {
            entity.HasKey(e => e.AccountId);

            entity.Property(e => e.AccountId).HasColumnName("AccountID");
            entity.Property(e => e.Avatar).IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Phone)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.Username)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.Role).WithMany(p => p.UserAccounts)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK_UserAccounts_Roles");
        });

        modelBuilder.Entity<Utility>(entity =>
        {
            entity.Property(e => e.UtilityId).HasColumnName("UtilityID");
            entity.Property(e => e.UtilityName).HasMaxLength(100);
        });

        modelBuilder.Entity<Ward>(entity =>
        {
            entity.ToTable("Ward");

            entity.Property(e => e.WardId).HasColumnName("WardID");
            entity.Property(e => e.DistrictId).HasColumnName("DistrictID");
            entity.Property(e => e.WardName).HasMaxLength(100);

            entity.HasOne(d => d.District).WithMany(p => p.Wards)
                .HasForeignKey(d => d.DistrictId)
                .HasConstraintName("FK_Ward_District");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
