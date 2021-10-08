using DatabaseAccess.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryTask.Models
{
	public class BookReservationsModel
	{
		public string BookTitle { get; set; }
		public List<Reservation> Reservations { get; set; }
	}
}
