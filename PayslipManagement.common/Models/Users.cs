﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayslipManagement.Common.Models
{
    public class Users
    {
        public int Id { get; set; }
        public string Emp_Code { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}
