using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Review
    {
        public int Id { get; set; }
        public string? Text { get; set; }
        public DateTime Date { get; set; }
        [Range(0.0,5.0)]
        public float Rating { get; set; }
        [ForeignKey("Product")]
        public int? ProductID { get; set; }
        public virtual Product? Product { get; set; }
        [ForeignKey("User")]
        public int? UserID { get; set; }
        public virtual User? User { get; set; }
    }
}
