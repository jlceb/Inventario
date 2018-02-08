using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStock.Domain
{
    public class Category
    {
        [Key]
        public int IdCategory { get; set; }
        [Required(ErrorMessage ="The field {0} is required.")]
        [MaxLength(50,ErrorMessage ="The field {0} only can contain {1} characters length.")]
        [Index("Category_Description_Index", IsUnique = true)]
        public string Description { get; set; }
    }
}
