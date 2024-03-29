﻿using System;
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

    public virtual DbSet<Package> Packages { get; set; }

    public virtual DbSet<PackageType> PackageTypes { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Room> Rooms { get; set; }

    public virtual DbSet<RoomFavorite> RoomFavorites { get; set; }

    public virtual DbSet<RoomImage> RoomImages { get; set; }

    public virtual DbSet<RoomRating> RoomRatings { get; set; }

    public virtual DbSet<RoomReport> RoomReports { get; set; }

    public virtual DbSet<RoomType> RoomTypes { get; set; }

    public virtual DbSet<TransactionHistory> TransactionHistories { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserPackage> UserPackages { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
   /*        => optionsBuilder.UseSqlServer("Server=MRTHAWNG; Database=mojoin;Trusted_Connection=SSPI;Encrypt=false;TrustServerCertificate=true");
   */
   => optionsBuilder.UseSqlServer("Data Source=SQL8006.site4now.net,1433;Initial Catalog=db_aa314e_moojoin;User Id=db_aa314e_moojoin_admin;Password=29102002Tt.;\r\n\r\n");
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Package>(entity =>
        {
            entity.ToTable("Package");

            entity.Property(e => e.PackageId).HasColumnName("PackageID");
            entity.Property(e => e.PackageName).HasMaxLength(200);
            entity.Property(e => e.PackageTypeId).HasColumnName("PackageTypeID");
            entity.Property(e => e.Viptype).HasColumnName("VIPType");

            entity.HasOne(d => d.PackageType).WithMany(p => p.Packages)
                .HasForeignKey(d => d.PackageTypeId)
                .HasConstraintName("FK_Package_PackageType");
        });

        modelBuilder.Entity<PackageType>(entity =>
        {
            entity.ToTable("PackageType");

            entity.Property(e => e.PackageTypeId).HasColumnName("PackageTypeID");
            entity.Property(e => e.PackageTypeName).HasMaxLength(50);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RolelD);

            entity.Property(e => e.RoleName).HasMaxLength(50);
        });

        modelBuilder.Entity<Room>(entity =>
        {
            entity.Property(e => e.RoomId).HasColumnName("RoomID");
            entity.Property(e => e.City).HasMaxLength(100);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.District).HasMaxLength(100);
            entity.Property(e => e.HasAirConditioner).HasColumnName("hasAir_conditioner");
            entity.Property(e => e.HasElevator).HasColumnName("hasElevator");
            entity.Property(e => e.HasParking).HasColumnName("hasParking");
            entity.Property(e => e.HasRefrigerator).HasColumnName("hasRefrigerator");
            entity.Property(e => e.HasWasher).HasColumnName("hasWasher");
            entity.Property(e => e.IsActive).HasColumnName("isActive");
            entity.Property(e => e.LastUpdate).HasColumnType("datetime");
            entity.Property(e => e.NumBathrooms).HasColumnName("numBathrooms");
            entity.Property(e => e.NumRooms).HasColumnName("numRooms");
            entity.Property(e => e.RoomTypeId).HasColumnName("RoomTypeID");
            entity.Property(e => e.Street).HasMaxLength(100);
            entity.Property(e => e.StreetNumber)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Title).HasMaxLength(100);
            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.Video).IsUnicode(false);
            entity.Property(e => e.ViewCount).HasColumnName("view_count");
            entity.Property(e => e.Ward).HasMaxLength(100);

            entity.HasOne(d => d.RoomType).WithMany(p => p.Rooms)
                .HasForeignKey(d => d.RoomTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Rooms_RoomTypes");

            entity.HasOne(d => d.User).WithMany(p => p.Rooms)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Rooms_Users");
        });

        modelBuilder.Entity<RoomFavorite>(entity =>
        {
            entity.HasKey(e => e.FavoriteId);

            entity.Property(e => e.FavoriteId).HasColumnName("FavoriteID");
            entity.Property(e => e.RoomId).HasColumnName("RoomID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Room).WithMany(p => p.RoomFavorites)
                .HasForeignKey(d => d.RoomId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RoomFavorites_Rooms");

            entity.HasOne(d => d.User).WithMany(p => p.RoomFavorites)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RoomFavorites_Users");
        });

        modelBuilder.Entity<RoomImage>(entity =>
        {
            entity.HasKey(e => e.RoomImageId).HasName("PK_Roomimages");

            entity.Property(e => e.RoomImageId).HasColumnName("RoomImageID");
            entity.Property(e => e.Image).IsUnicode(false);
            entity.Property(e => e.RoomId).HasColumnName("RoomID");

            entity.HasOne(d => d.Room).WithMany(p => p.RoomImages)
                .HasForeignKey(d => d.RoomId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Roomimages_Rooms");
        });

        modelBuilder.Entity<RoomRating>(entity =>
        {
            entity.HasKey(e => e.RatingId);

            entity.Property(e => e.RatingId).HasColumnName("RatingID");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.RoomId).HasColumnName("RoomID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Room).WithMany(p => p.RoomRatings)
                .HasForeignKey(d => d.RoomId)
                .OnDelete(DeleteBehavior.ClientSetNull)
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
            entity.Property(e => e.IsResolved).HasColumnName("isResolved");
            entity.Property(e => e.RoomId).HasColumnName("RoomID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Room).WithMany(p => p.RoomReports)
                .HasForeignKey(d => d.RoomId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RoomReports_Rooms");

            entity.HasOne(d => d.User).WithMany(p => p.RoomReports)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RoomReports_Users");
        });

        modelBuilder.Entity<RoomType>(entity =>
        {
            entity.Property(e => e.RoomTypeId).HasColumnName("RoomTypeID");
            entity.Property(e => e.TypeName).HasMaxLength(50);
        });

        modelBuilder.Entity<TransactionHistory>(entity =>
        {
            entity.HasKey(e => e.TransactionId);

            entity.ToTable("TransactionHistory");

            entity.Property(e => e.TransactionId).HasColumnName("TransactionID");
            entity.Property(e => e.PaymentMethod).HasMaxLength(50);
            entity.Property(e => e.TransactionDate).HasColumnType("datetime");
            entity.Property(e => e.TransactionReference).IsUnicode(false);
            entity.Property(e => e.TransactionType).HasMaxLength(50);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.TransactionHistories)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_TransactionHistory_Users");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.Avatar).IsUnicode(false);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Dateofbirth).HasColumnType("date");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.GoogleId)
                .IsUnicode(false)
                .HasColumnName("GoogleID");
            entity.Property(e => e.InfoFacebook).IsUnicode(false);
            entity.Property(e => e.InfoZalo).IsUnicode(false);
            entity.Property(e => e.IsActive).HasColumnName("isActive");
            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.ResetPasswordToken).IsUnicode(false);
            entity.Property(e => e.RolesId).HasColumnName("RolesID");
            entity.Property(e => e.Salt).IsUnicode(false);
            entity.Property(e => e.Sex).HasMaxLength(10);
            entity.Property(e => e.SupportUserId).HasColumnName("support_UserID");

            entity.HasOne(d => d.Roles).WithMany(p => p.Users)
                .HasForeignKey(d => d.RolesId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Users_Roles");
        });

        modelBuilder.Entity<UserPackage>(entity =>
        {
            entity.ToTable("UserPackage");

            entity.Property(e => e.UserPackageId).HasColumnName("UserPackageID");
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.PackageId).HasColumnName("PackageID");
            entity.Property(e => e.RoomId).HasColumnName("RoomID");
            entity.Property(e => e.StartDate).HasColumnType("datetime");
            entity.Property(e => e.TransactionId).HasColumnName("TransactionID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Package).WithMany(p => p.UserPackages)
                .HasForeignKey(d => d.PackageId)
                .HasConstraintName("FK_UserPackage_Package");

            entity.HasOne(d => d.Room).WithMany(p => p.UserPackages)
                .HasForeignKey(d => d.RoomId)
                .HasConstraintName("FK_UserPackage_Rooms");

            entity.HasOne(d => d.Transaction).WithMany(p => p.UserPackages)
                .HasForeignKey(d => d.TransactionId)
                .HasConstraintName("FK_UserPackage_TransactionHistory");

            entity.HasOne(d => d.User).WithMany(p => p.UserPackages)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_UserPackage_Users");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
