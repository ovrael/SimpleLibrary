using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatabaseAccess.Database
{
	public class Reservation
	{
		[Key]
		public int ReservationID { get; set; }

		[Required]
		public DateTime ReservationDate { get; set; }

		public User User { get; set; }

		public Book Book { get; set; }
	}
}
