using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyStock.API.Models
{
    public class ProductResponse
    {
 
        public int ProductId { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public bool IsActive { get; set; }

        public string Remarks { get; set; }

        public DateTime LastPurchase { get; set; }

        public string Image { get; set; }

        public double Stock { get; set; }

    }
}