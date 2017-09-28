﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication3.Models
{
    public partial class Country
    {
        [Key]
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public int StateId { get; set; }
    }
}