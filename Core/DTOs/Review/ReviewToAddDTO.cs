using Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.Review
{
    public class ReviewToAddDTO
    {
        public string? Text { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;

        [Range(0.0, 5.0)]
        public float Rating { get; set; }

        public int? ProductID { get; set; }

    }
}
