using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatabaseAccess.Database
{
	public class Book
	{
		[Key]
		public int BookID { get; set; }

		[Required]
		[MaxLength(100)]
		public string Title { get; set; }

		[Required]
		[MaxLength(100)]
		public string Author { get; set; }

		[Required]
		[DataType(DataType.Date)]
		public DateTime PublishDate { get; set; }

		[Required]
		[MaxLength(150)]
		public string Description { get; set; }

		public ICollection<Reservation> Reservations { get; set; }
	}
}
