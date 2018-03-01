using MyStock.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyStock.API.Models
{
    public class ProductRequest : Product
    {
        public byte[] ImageArray { get; set; }
    }
}