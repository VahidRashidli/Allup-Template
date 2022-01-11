using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Allup_Template.Models;

namespace Allup_Template.Areas.Admin.View_Models
{
    public class CategoryIndexViewModel
    {
       public ICollection<Category> ParentCategories { get; set; }
        public ICollection<Category> ChildCategories { get; set; }
    }
}
