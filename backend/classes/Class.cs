using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;

public class Class : DbContext
{
	public DbSet<User> User { get; set; }
	public DbSet<Room> Room { get; set; }
	public DbSet<Bookings> Bookings { get; set; }

	public string DbPath { get; }

	public Class()
	{
		var folder = Environment.SpecialFolder.LocalApplicationData;
		var path = Environment.GetFolderPath(folder);
		DbPath = System.IO.Path.Join(path, "checkIT.db");
	}
	protected override void OnConfiguring(DbContextOptionsBuilder options)
		 => options.UseSqlite($"Data Source={DbPath}");
}

public class Bookings
{
	public int BookingId { get; set; }
	public int StartTime { get; set; }
	public int EndTime { get; set; }
	public int Room { get; set; }
	public int UserID { get; set; }
	public int CreateTime { get; set; }
	public int CreatedBy { get; set; }

}

public class User
{
	public int UserId { get; set; }
	public string Username { get; set; }
	public string Firstname { get; set; }
	public string LastName { get; set; }
	public string Passwd { get; set; }
	public int LastLogon { get; set; }
	public int LastChange { get; set; }
	public int Active { get; set; }
	public int Role { get; set; }
}
public class Room
{
	public int RoomID { get; set; }
	public string Number { get; set; }
	public string Name { get; set; }
}