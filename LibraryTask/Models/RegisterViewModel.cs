using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryTask.Models
{
	public class RegisterViewModel
	{
		[Required]
		[DataType(DataType.Text)]
		public string Name { get; set; }

		[Required]
		[DataType(DataType.Text)]
		[Display(Name = "Last Name")]
		public string LastName { get; set; }

		[Required]
		[DataType(DataType.Text)]
		public string Login { get; set; }

		[Required]
		[EmailAddress]
		[DataType(DataType.EmailAddress)]
		public string Email { get; set; }

		[DataType(DataType.EmailAddress)]
		[Display(Name = "Confirm Email")]
		[Compare("Email", ErrorMessage = "Emails do not match.")]
		public string ConfirmEmail { get; set; }

		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		[DataType(DataType.Password)]
		[Display(Name = "Confirm password")]
		[Compare("Password", ErrorMessage = "Passwords do not match.")]
		public string ConfirmPassword { get; set; }
	}
}
