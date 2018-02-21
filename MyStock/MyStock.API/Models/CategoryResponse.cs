using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyStock.API.Models
{
    public class CategoryResponse
    {
        public int CategoryId { get; set; }

        public string Description { get; set; }

        public List<ProductResponse> Productos { get; set; }
    }
}