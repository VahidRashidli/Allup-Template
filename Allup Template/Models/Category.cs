using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Allup_Template.Models
{
    public class Category:Base
    {
        [Required,Column(TypeName ="varchar(max)")]
        public string Name { get; set; }
        public bool IsMain { get; set; }
        public Category Parent { get; set; }
       
        public List<Category> Children { get; set; }
        public string Image { get; set; }
        [NotMapped]
        public IFormFile File { get; set; }
        public ICollection<CategoryProduct> CategoryProducts { get; set; }
    }
}
