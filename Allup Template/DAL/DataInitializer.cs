using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Allup_Template.Models;
using Microsoft.EntityFrameworkCore;

namespace Allup_Template.DAL
{
    public class DataInitializer
    {
        private readonly AppDbContext _context;

        public DataInitializer(AppDbContext context)
        {
            _context = context;
        }
        public async Task InitializeData() {
            _context.Database.Migrate();
            if (!_context.Categories.Any())
            {
                _context.Categories.AddRange(
                    new Category { Image= "category-1.jpg",IsMain=true,Name="Laptop" },
                    new Category { Image= "category-2.jpg",IsMain=true,Name="Computer" },
                    new Category { Image= "category-3.jpg",IsMain=true,Name="Smarthone" },
                    new Category { Image= "category-4.jpg",IsMain=true,Name="Game Consoles" },
                    new Category { Image= "category-5.jpg",IsMain=true,Name="Bottoms" }
                    );
            }
            await _context.SaveChangesAsync();
        }
    }
}
