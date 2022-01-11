using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Allup_Template.Areas.Admin.Utilities.FileActions;
using Allup_Template.Areas.Admin.View_Models;
using Allup_Template.Areas.Constants;
using Allup_Template.DAL;
using Allup_Template.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Allup_Template.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;
        public CategoryController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(new CategoryIndexViewModel()
            {
                ParentCategories = await _context.Categories
                .Where(c => !c.IsDeleted && c.IsMain).ToListAsync(),
                ChildCategories= await _context.Categories
                .Where(c => !c.IsDeleted && !c.IsMain).ToListAsync()
            });
        }
        public async Task<IActionResult> Create()
        {
            return View(new CategoryCreateViewModel()
            {
            ParentList=await _context.Categories.Where(c=>c.IsMain).ToListAsync()
            });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryCreateViewModel model,int Id)
        {
            if (model.Category.Name == null)
            {
                return View(new CategoryCreateViewModel()
                {
                    ParentList = await _context.Categories.Where(c => c.IsMain).ToListAsync()
                });
            }
            if (model.Category.IsMain!=true)
            {
                if (model.ParentId==0)
                {
                    ModelState.AddModelError(nameof(CategoryCreateViewModel.ParentId),
                        "A child category must have a parent category!");
                    return View(new CategoryCreateViewModel()
                    {
                        ParentList = await _context.Categories.Where(c => c.IsMain).ToListAsync()
                    });
                }
                await _context.Categories.AddAsync(new Category() {
                Name=model.Category.Name,
                Parent=await _context.Categories.FindAsync(model.ParentId)
                });
            }
            else
            {
                if (model.File==null)
                {
                    ModelState.AddModelError(nameof(CategoryCreateViewModel.File),
                       "A parent category must have an image!");
                    return View(new CategoryCreateViewModel()
                    {
                        ParentList = await _context.Categories.Where(c => c.IsMain).ToListAsync(),
                        IsMain=true
                    });
                }
                if (!model.File.CheckFileContent())
                {
                    ModelState.AddModelError(nameof(CategoryCreateViewModel.File),
                      "The file  has to be image!");
                    return View(new CategoryCreateViewModel()
                    {
                        ParentList = await _context.Categories.Where(c => c.IsMain).ToListAsync(),
                        IsMain=true
                    });
                }
                if (!model.File.CheckFileSize())
                {
                    ModelState.AddModelError(nameof(CategoryCreateViewModel.File),
                      "The file is too large!");
                    return View(new CategoryCreateViewModel()
                    {
                        ParentList = await _context.Categories.Where(c => c.IsMain).ToListAsync(),
                        IsMain=true
                    });
                }
                Guid guid = Guid.NewGuid();
               await model.File.CreateFileAsync(FileNameConstants.Image, guid);
                _context.Categories.Add(new Category()
                {
                    Name=model.Category.Name,
                    Image=guid+model.File.FileName,
                    IsMain=true
                });
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
