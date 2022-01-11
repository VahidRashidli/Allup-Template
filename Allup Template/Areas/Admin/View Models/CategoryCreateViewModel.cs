using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Allup_Template.Models;
using Microsoft.AspNetCore.Http;

namespace Allup_Template.Areas.Admin.View_Models
{
    public class CategoryCreateViewModel
    {
        public IFormFile File { get; set; }
        public Category Category { get; set; }
        public ICollection<Category> ParentList { get; set; }
        public int ParentId { get; set; }
        public bool IsMain { get; set; } 
    }
}
