using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Allup_Template.Areas.Admin.Utilities.FileActions;
using Allup_Template.Areas.Admin.View_Models;
using Allup_Template.Areas.Constants;
using Allup_Template.DAL;
using Allup_Template.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Allup_Template.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Create()
        {
            return View(new ProductCreateViewModel() { 
            ParentCategories=await _context.Categories.Where(c=>!c.IsDeleted&&c.IsMain)
            .Select(c=>new CategorySelectViewModel() {Id=c.Id,Name=c.Name }).ToListAsync(),
             ChildCategories= await _context.Categories.Where(c => !c.IsDeleted && !c.IsMain)
            .Select(c => new CategorySelectViewModel() { Id = c.Id, Name = c.Name }).ToListAsync(),
             Brands=await _context.Brands.Where(b=>!b.IsDeleted).ToListAsync()
            });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCreateViewModel model)
        {
            ProductCreateViewModel errorModel = new ProductCreateViewModel()
            {
             ParentCategories = await _context.Categories.Where(c => !c.IsDeleted && c.IsMain)
            .Select(c => new CategorySelectViewModel() { Id = c.Id, Name = c.Name }).ToListAsync(),
             ChildCategories = await _context.Categories.Where(c => !c.IsDeleted && !c.IsMain)
            .Select(c => new CategorySelectViewModel() { Id = c.Id, Name = c.Name }).ToListAsync(),
             Brands=await _context.Brands.Where(b=>!b.IsDeleted).ToListAsync(),
             IsDiscounted=model.IsDiscounted
            };
            if (!ModelState.IsValid) return View(errorModel);
            if (model.IsDiscounted == true)
            {
                if (model.Product.DiscountPercentage == 0)
                {
                    ModelState.AddModelError(nameof(ProductCreateViewModel.IsDiscounted),
                        "You must add the discount percentage!");
                    return View(errorModel);
                }
            }
            if (model.BrandId==0)
            {
                ModelState.AddModelError(nameof(ProductCreateViewModel.BrandId), "The field is required");
                return View(errorModel);
            }
            if (model.ChildCategoryId==0)
            {
                ModelState.AddModelError(nameof(ProductCreateViewModel.ChildCategoryId), "The field is required");
                return View(errorModel);
            }
            if (model.ParentCategoryId == 0)
            {
                ModelState.AddModelError(nameof(ProductCreateViewModel.ParentCategoryId), "The field is required");
                return View(errorModel);
            }
            Category parentCategory = await _context.Categories.FindAsync(model.ParentCategoryId);
            Category childCategory = await _context.Categories.Include(c=>c.Parent)
                .FirstOrDefaultAsync(c=>c.Id==model.ChildCategoryId);
            if (childCategory.Parent.Id!=parentCategory.Id)
            {
                ModelState.AddModelError(nameof(ProductCreateViewModel.ParentCategoryId),
                    "Please select a valid parent category for this child category.");
                return View(errorModel);
            }
            if (model.MainImage==null)
            {
                ModelState.AddModelError(nameof(ProductCreateViewModel.MainImage),
                   "A product must have a main image");
                return View(errorModel);
            }
            if (model.ImageFiles==null)
            {
                ModelState.AddModelError(nameof(ProductCreateViewModel.ImageFiles),
                   "A product must have at least one image");
                return View(errorModel);
            }
            if (!model.MainImage.CheckFileContent())
            {
                ModelState.AddModelError(nameof(ProductCreateViewModel.MainImage),
                 "The file must be an image");
                return View(errorModel);
            }
            if (!model.MainImage.CheckFileSize())
            {
                ModelState.AddModelError(nameof(ProductCreateViewModel.MainImage),
                "The file is too large");
                return View(errorModel);
            }
            List<ProductImage> productImages = new List<ProductImage>();
            int imageOrder = 0;
            foreach(IFormFile file in model.ImageFiles)
            {
                if (!file.CheckFileContent())
                {
                    ModelState.AddModelError(nameof(ProductCreateViewModel.ImageFiles),
                     $"{file.FileName} file must be an image");
                    return View(errorModel);
                }
                if (!file.CheckFileSize())
                {
                    ModelState.AddModelError(nameof(ProductCreateViewModel.ImageFiles),
                    $"{file.FileName} file is too large");
                    return View(errorModel);
                }
                Guid guid = Guid.NewGuid();
                await file.CreateFileAsync(FileNameConstants.Image, guid);
                productImages.Add(new ProductImage() { Name = guid + file.FileName, Order = imageOrder});
                imageOrder++;
            }
            Guid guidMain = Guid.NewGuid();
            await model.MainImage.CreateFileAsync(FileNameConstants.Image, guidMain);
            productImages.Add(new ProductImage() { Name = guidMain + model.MainImage.FileName, IsMain = true });
            Product product = new Product()
            {
                Name = model.Product.Name,
                Brand = await _context.Brands.FindAsync(model.BrandId),
                DiscountPercentage = model.Product.DiscountPercentage,
                IsDiscounted = model.IsDiscounted,
                Count = model.Product.Count,
                Price = model.Product.Price,
                ProductCode = model.Product.ProductCode,
                Description = model.Product.Description,
                Images= productImages
            };
            await _context.Products.AddAsync(product);
            await _context.CategoryProducts.AddAsync(new CategoryProduct()
            {
                Category = childCategory,
                Product =product
            });
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(CategoryController.Index),"Category");
        }
    }
}
