using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace awl_raumreservierung
{
    /// <summary>
    /// 
    /// </summary>
#pragma warning disable IDE1006 // Naming Styles
    public partial class checkITContext : DbContext
    {
        /// <summary>
        /// 
        /// </summary>
        public checkITContext()
        {
        }

/// <summary>
/// 
/// </summary>
/// <param name="options"></param>
        public checkITContext(DbContextOptions<checkITContext> options)
            : base(options)
        {
        }
/// <summary>
/// 
/// </summary>
/// <value></value>

        public virtual DbSet<Booking> Bookings { get; set; } = null!;

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public virtual DbSet<Room> Rooms { get; set; } = null!;

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public virtual DbSet<User> Users { get; set; } = null!;

/// <summary>
/// 
/// </summary>
/// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("DataSource=../checkIT.db;");
            }
        }

/// <summary>
/// 
/// </summary>
/// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Booking>(entity =>
            {
                entity.HasIndex(e => e.Id, "IX_Bookings_ID")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.UserId).HasColumnName("UserID");
            });

            modelBuilder.Entity<Room>(entity =>
            {
                entity.ToTable("Room");

                entity.HasIndex(e => e.Id, "IX_Room_ID")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.HasIndex(e => e.Id, "IX_User_ID")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
