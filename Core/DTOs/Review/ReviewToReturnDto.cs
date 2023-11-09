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
    public class ReviewToReturnDto
    {
        public int Id { get; set; }
        public string? Text { get; set; }
        public string Date { get; set; } 

        [Range(0.0, 5.0)]
        public float Rating { get; set; }


        public int? ProductID { get; set; }
        public string? ProductName { get; set; }

        public int? UserID { get; set; }
        public string? UserName { get; set; }
    }
}
