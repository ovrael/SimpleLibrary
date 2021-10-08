using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.AccessControl;
using DatabaseAccess.Database;
using Microsoft.EntityFrameworkCore;

namespace DatabaseAccess.Access
{
	public class LibraryContext : DbContext
	{
		public LibraryContext(DbContextOptions options) : base(options) { }

		public DbSet<User> Users { get; set; }
		public DbSet<Book> Books { get; set; }
		public DbSet<Reservation> Reservations { get; set; }
	}
}
