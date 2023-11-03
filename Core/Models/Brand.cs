﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Brand
    {
        public int ID { get; set; }
        [Required]
        public string? Name { get; set; }
    }
}
