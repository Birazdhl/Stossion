﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

	namespace Stossion.ViewModels.User
	{
		public class RefreshTokenViewModel
		{
			[Required]
			public string RefreshToken { get; set; }
		}
	}
