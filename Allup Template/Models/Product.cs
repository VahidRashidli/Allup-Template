using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Allup_Template.Models
{
    public class Product : Base
    {
        public string ProductCode { get; set; }
        public string Name { get; set; }
        public Brand Brand { get; set; }
        public Rating Rating { get; set; }
        public double Price { get; set; }
        public float Tax { get; set; }
        public double Count { get; set; }
        public string Description { get; set; }
        public bool IsDiscounted { get; set; }
        public ICollection<ProductImage> Images { get; set; }
        public ICollection<CategoryProduct> CategoryProducts { get; set; }
    }
}