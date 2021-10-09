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
		[RegularExpression(@"(\S)+", ErrorMessage = "White Space is not allowed")]
		public string Name { get; set; }

		[Required]
		[DataType(DataType.Text)]
		[Display(Name = "Last Name")]
		[RegularExpression(@"(\S)+", ErrorMessage = "White Space is not allowed")]
		public string LastName { get; set; }

		[Required]
		[DataType(DataType.Text)]
		[RegularExpression(@"(\S)+", ErrorMessage = "White Space is not allowed")]
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
		[StringLength(100, ErrorMessage = "Password must be at least 8 characters long.", MinimumLength = 8)]
		[RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$",
			ErrorMessage = "Passwords must be at least 8 characters and contain at least 1 lower case letter, 1 upper case letter, 1 number and 1 special character (e.g. !@#$%^&*)")]

		public string Password { get; set; }

		[DataType(DataType.Password)]
		[Display(Name = "Confirm password")]
		[Compare("Password", ErrorMessage = "Passwords do not match.")]
		public string ConfirmPassword { get; set; }
	}
}
