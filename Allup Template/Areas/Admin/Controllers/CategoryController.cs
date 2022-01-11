using System;
using System.Collections.Generic;
using System.IO;
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
            if(model.IsMain)
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
            else
            {
                if (model.ParentId == 0)
                {
                    ModelState.AddModelError(nameof(CategoryCreateViewModel.ParentId),
                        "A child category must have a parent category!");
                    return View(new CategoryCreateViewModel()
                    {
                        ParentList = await _context.Categories.Where(c => c.IsMain).ToListAsync()
                    });
                }
                await _context.Categories.AddAsync(new Category()
                {
                    Name = model.Category.Name,
                    Parent = await _context.Categories.FindAsync(model.ParentId)
                });
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int id)
        {
            Category category = await _context.Categories.Include(c=>c.Children).
                FirstOrDefaultAsync(c=>c.Id==id);
            if (category == null) return NotFound();
            if (category.IsMain)
            {
                if (category.Children.Any(c=>!c.IsDeleted))
                {
                    ViewBag.data = "First you need to get rid of all the child elements";
                }
            }
            return View(category);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id,Category category)
        {
            Category dbcategory = await _context.Categories.Include(c => c.Children).
                FirstOrDefaultAsync(c => c.Id == id);
            if (dbcategory == null) return NotFound();
            dbcategory.IsDeleted = true;
            dbcategory.DeletedDate = DateTime.Now;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int id)
        {
            Category dbcategory =await _context.Categories.FindAsync(id);
            if (dbcategory == null) return NotFound();
            return View(dbcategory);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id,Category category)
        {
            Category dbcategory = await _context.Categories.FindAsync(id);
            if (dbcategory == null) return NotFound();
            if (dbcategory.Id!=id) return BadRequest();
            if (!ModelState.IsValid) return View();
            if (category.IsMain)
            {
                if (category.File==null)
                {
                    ModelState.AddModelError(nameof(Category.File), "You must upload a file");
                    return View();
                }
                if (!category.File.CheckFileContent())
                {
                    ModelState.AddModelError(nameof(Category.File),
                      "The file  has to be image!");
                    return View();
                }
                if (!category.File.CheckFileSize())
                {
                    ModelState.AddModelError(nameof(Category.File),
                      "The file is too large!");
                    return View();
                }
                if (System.IO.File.Exists(Path.Combine(FileNameConstants.Image, dbcategory.Image)))
                {
                    System.IO.File.Delete(Path.Combine(FileNameConstants.Image, dbcategory.Image));
                }
                Guid guid = Guid.NewGuid();
                await category.File.CreateFileAsync(FileNameConstants.Image, guid);
                dbcategory.Image = guid + category.File.FileName;
            }
            dbcategory.Name = category.Name;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
