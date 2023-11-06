using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Wishlist
    {
        public int Id { get; set; }
        [ForeignKey("User")]
        public int? UserID { get; set; }
        public virtual User? User { get; set; }
        [ForeignKey("Product")]
        public int? ProductID { get; set; }
        public virtual Product? Product { get; set; }
    }
}
