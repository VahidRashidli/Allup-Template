using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Allup_Template.Models;
using Microsoft.AspNetCore.Http;

namespace Allup_Template.Areas.Admin.View_Models
{
    public class ProductCreateViewModel
    {
        public Product Product { get; set; } = new Product();
        public bool IsDiscounted { get; set; }
        public ICollection<Brand> Brands { get; set; }
        public int BrandId { get; set; }
        public IFormFile[] ImageFiles { get; set; }
        public IFormFile MainImage { get; set; }
        public ICollection<CategorySelectViewModel> ParentCategories { get; set; }
        [Required]
        public int ParentCategoryId { get; set; }
        [Required]
        public int ChildCategoryId { get; set; }
        public ICollection<CategorySelectViewModel> ChildCategories { get; set; }
    }
}
