﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ApplicationCore.Models
{
    public class Role : IdentityRole
    {

        public string? Description { get; set; }

    }
}
