using System;
using System.Collections.Generic;
using System.Text;

namespace MyStock.Models
{
    public class Product
    {
        public int ProductId { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public bool IsActive { get; set; }

        public string Remarks { get; set; }

        public DateTime LastPurchase { get; set; }

        public string Image { get; set; }

        public double Stock { get; set; }

        public byte[] ImageArray { get; set; }

        public string ImageFullPath
        {
            get
            {
                if(string.IsNullOrEmpty(Image))
                {
                    return "ic_noimage";
                }
                return string.Format("http://productszulu.azurewebsites.net/{0}", Image.Substring(1));
            }
        }
        public int CategoryId { get; set; }
    }
}
