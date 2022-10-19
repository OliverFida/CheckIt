using awl_raumreservierung.Controllers;
using awl_raumreservierung.core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;

namespace awl_raumreservierung {
	/// <summary>
	///
	/// </summary>
#pragma warning disable IDE1006 // Naming Styles
	public partial class checkITContext : DbContext {
		/// <summary>
		///
		/// </summary>
		public checkITContext() {
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="options"></param>
		public checkITContext(DbContextOptions<checkITContext> options) : base(options) { }

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
		/// <param name="modelBuilder"></param>
		protected override void OnModelCreating(ModelBuilder modelBuilder) =>
			// Admin seeden
			modelBuilder.Entity<User>().HasData(new User { Username = "admin", Firstname = "Admin", Lastname = "Benutzer", Lastchange = DateTime.MinValue, Passwd = BCrypt.Net.BCrypt.HashPassword("admin"), Role = UserRole.Admin, Id = 1, Active = true });
	}
}
