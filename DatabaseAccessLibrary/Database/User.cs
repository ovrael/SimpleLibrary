using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace DatabaseAccess.Database
{
	public class User
	{
		[Key]
		public int UserID { get; set; }

		[Required]
		[MaxLength(50)]
		public string Name { get; set; }

		[Required]
		[MaxLength(60)]
		public string LastName { get; set; }

		[Required]
		[MaxLength(50)]
		public string Login { get; set; }

		[Required]
		[MaxLength(50)]
		public string Email { get; set; }

		[Required]
		public string Salt { get; set; }

		[Required]
		public string Password { get; set; }

		public ICollection<Reservation> Reservations { get; set; }
	}
}
