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
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Brand> Brands{ get; set; }
        public DbSet<Product> Products{ get; set; }
        public DbSet<ProductImage>ProductImages{ get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoryProduct> CategoryProducts{ get; set; }
    }
}
