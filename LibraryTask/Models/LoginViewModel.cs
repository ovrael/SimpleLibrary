﻿using System.ComponentModel.DataAnnotations;

namespace LibraryTask.Models
{
	public class LoginViewModel
	{
		[Required]
		[EmailAddress]
		[DataType(DataType.EmailAddress)]
		public string Email { get; set; }

		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		[Display(Name = "Remember Me")]
		public bool RememberMe { get; set; }
	}
}
