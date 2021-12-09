﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TemplatePractice.DAL;
using TemplatePractice.Models;
using TemplatePractice.ViewModels;

namespace TemplatePractice.Controllers
{
    public class ProductController : Controller
    {

            private AppDbContext _context;
            public ProductController(AppDbContext context)
            {
                _context = context;
            }
            public IActionResult Index()
            {

                int count = 0;
                AboutSection aboutSection = _context.AboutSections.FirstOrDefault();
                ICollection<InfoList> infoLists = _context.InfoLists.ToList();
                ICollection<Product> products = _context.Products.Include(p => p.Category).Skip(count).Take(4).ToList();
                ICollection<Category> categories = _context.Categories.OrderBy(x => x.Order).ToList();
                Subscribe subscribe = _context.Subscribes.FirstOrDefault();
                ExpertSection expertSection = _context.ExpertSections.FirstOrDefault();
                ICollection<ExpertImage> expertImages = _context.ExpertImages.OrderBy(i => i.Order).ToList();
                return View(new HomeIndexViewModel { AboutSection = aboutSection, InfoLists = infoLists, Categories = categories, Products = products, Subscribe = subscribe, ExpertImages = expertImages, ExpertSection = expertSection });
            }
        public IActionResult GetPartial(int count)
        {
          
            ICollection<Product> products = _context.Products.Include(p => p.Category).Skip(count).Take(2).ToList();
            
            return Json(products);
        }
        
    }
}
