using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Address
    {
        public int Id { get; set; }
        public string? Street { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public int? PostalCode { get; set; }
        public string? SpecialInstructions { get; set; }
        [ForeignKey("User")]
        public int? UserID { get; set; }
        public virtual User? User { get; set; }
        public override string ToString()
        {
            return $"{Street}, {City}, {Country}";
        }
    }
}
