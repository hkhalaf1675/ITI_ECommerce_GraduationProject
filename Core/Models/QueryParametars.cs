using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class QueryParametars
    {
        public string? sort { get; set; } //  name(defualt) | priceAsc | priceDesc
        public int? categoryid { get; set; }
        public int? brandId { get; set; }

        public string? condition {  get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public double? Rating { get; set; }

        private string? search;

        public string? Search
        {
            get { return search; }
            set { search = value.ToLower(); }
        }


        private int pageSize = 8;
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value > 10 ? 10 : value; }
        }
        public int PageIndex { get; set; } = 1;

        // add the color
        public string? Color { get; set; }
    }
}
