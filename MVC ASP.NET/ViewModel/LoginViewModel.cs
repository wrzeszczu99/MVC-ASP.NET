﻿using System.ComponentModel.DataAnnotations;

namespace MVC_ASP.NET.ViewModel
{
	public class LoginViewModel
	{
		[Display(Name = "Email Address")]
		[Required(ErrorMessage ="Email address is required")]
        public string Email { get; set; }

		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; }
    }
}
