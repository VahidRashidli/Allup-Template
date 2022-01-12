using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Allup_Template.Models
{
    public class Product : Base
    {
        [Required]
        public string ProductCode { get; set; }
        [Required]
        public string Name { get; set; }

        public Brand Brand { get; set; }
        public Rating Rating { get; set; }
        [Required]
        public double Price { get; set; }
        public float Tax { get; set; }
        public double Count { get; set; }
        [Required]
        public string Description { get; set; }
        public bool IsDiscounted { get; set; } = false;
        public double DiscountPercentage { get; set; }
        public ICollection<ProductImage> Images { get; set; }
        public ICollection<CategoryProduct> CategoryProducts { get; set; }
    }
}