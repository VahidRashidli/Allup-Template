using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Allup_Template.Models;
using Microsoft.EntityFrameworkCore;

namespace Allup_Template.DAL
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options) { }
        public DbSet<Category> Categories { get; set; }
    }
}
