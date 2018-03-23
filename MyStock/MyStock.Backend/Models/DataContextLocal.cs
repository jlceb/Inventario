using MyStock.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyStock.Backend.Models
{
    public class DataContextLocal : DataContext
    {
        public System.Data.Entity.DbSet<MyStock.Domain.Ubication> Ubications { get; set; }
    }
}