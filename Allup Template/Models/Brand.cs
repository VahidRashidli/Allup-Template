using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Allup_Template.Models
{
    public class Brand:Base
    {
        public string Name { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
