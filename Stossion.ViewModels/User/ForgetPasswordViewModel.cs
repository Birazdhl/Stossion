﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stossion.ViewModels.User
{
    public class ForgetPasswordViewModel
    {
        public string Username { get; set; } = string.Empty;
        public string? Password { get; set; }
        public string? Token { get; set; }
    }
}
