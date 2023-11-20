using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Phone
    {
        public string? PhoneNumber { get; set; }
        [ForeignKey("User")]
        public int? UserID { get; set; }
        public virtual User? User { get; set; }
    }
}
