using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealSpice.Data;
using RealSpice.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using RealSpice.Models;


namespace RealSpice.Areas.Admin.Controllers
{ 
    [Area("Admin")]
    public class SubCategoryController : Controller
    {

        private readonly ApplicationDbContext _db;

        [TempData]
        public string StatusMessage { get; set; }

        public SubCategoryController(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var subCategories = await _db.SubCategory.Include(c=> c.Category).ToListAsync();
            return View(subCategories); 
        }



        //GET - CREATE

        public async Task<IActionResult> Create()
        {    
            CategoryAndSubCategoryViewModel model = new CategoryAndSubCategoryViewModel()
            {
                CategoryList = await _db.Category.ToListAsync(),
                SubCategory = new Models.SubCategory(),
                SubCategoryList = await _db.SubCategory.OrderBy(o => o.Name).Select(o => o.Name).Distinct().ToListAsync()
            };

            return View(model);
        }


        //POST - CREATE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryAndSubCategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var doesSubCategoryExist = _db.SubCategory.Include(s => s.Category).Where(s =>
                    s.Name == model.SubCategory.Name && s.Category.Id == model.SubCategory.CategoryId);

                if (doesSubCategoryExist.Count() > 0)
                {
                    StatusMessage = "Error: SubCategory exist under " + doesSubCategoryExist.First().Category.Name +
                                    " category. Please use another name.";
                }
                else
                {
                    _db.SubCategory.Add(model.SubCategory);
                    await _db.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

            CategoryAndSubCategoryViewModel modelVM = new CategoryAndSubCategoryViewModel()
            {
                CategoryList = await _db.Category.ToListAsync(),
                SubCategory = model.SubCategory,
                SubCategoryList = await _db.SubCategory.OrderBy(o => o.Name).Select(o => o.Name).ToListAsync(),
                StatusMessage = StatusMessage
                
            };
            return View(modelVM);
        }


        public async Task<IActionResult> GetSubCategory(int id)
        {
            List<SubCategory> subCategories = new List<SubCategory>();

            subCategories = await (from subCategory in _db.SubCategory
                where subCategory.CategoryId == id
                select subCategory).ToListAsync();

            return Json(new SelectList(subCategories, "Id", "Name"));


        }



        //GET - EDIT
        public async Task<IActionResult> Edit(int? id)  
        {
            if (id == null)
            {
                return NotFound();
            }

            var subCategory = await _db.SubCategory.SingleOrDefaultAsync(a => a.Id == id);

            if (subCategory == null)
            {
                return NotFound();
            }

            CategoryAndSubCategoryViewModel model = new CategoryAndSubCategoryViewModel()
            {
                CategoryList = await _db.Category.ToListAsync(),
                SubCategory = subCategory,
                SubCategoryList = await _db.SubCategory.OrderBy(o => o.Name).Select(o => o.Name).Distinct().ToListAsync()
            };

            return View(model);
        }


        //POST - CREATE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id ,CategoryAndSubCategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var doesSubCategoryExist = _db.SubCategory.Include(s => s.Category).Where(s =>
                    s.Name == model.SubCategory.Name && s.Category.Id == model.SubCategory.CategoryId);

                if (doesSubCategoryExist.Count() > 0)
                {
                    StatusMessage = "Error: SubCategory exist under " + doesSubCategoryExist.First().Category.Name +
                                    " category. Please use another name.";
                }
                else
                {
                    var subCatFromDb = await _db.SubCategory.FindAsync(id);
                    subCatFromDb.Name = model.SubCategory.Name;

                    
                    await _db.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

            CategoryAndSubCategoryViewModel modelVM = new CategoryAndSubCategoryViewModel()
            {
                CategoryList = await _db.Category.ToListAsync(),
                SubCategory = model.SubCategory,
                SubCategoryList = await _db.SubCategory.OrderBy(o => o.Name).Select(o => o.Name).ToListAsync(),
                StatusMessage = StatusMessage

            };
            return View(modelVM);
        }



        //GET - EDIT
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subCategory = await _db.SubCategory.SingleOrDefaultAsync(a => a.Id == id);

            if (subCategory == null)
            {
                return NotFound();
            }

            CategoryAndSubCategoryViewModel model = new CategoryAndSubCategoryViewModel()
            {
                CategoryList = await _db.Category.ToListAsync(),
                SubCategory = subCategory,
                SubCategoryList = await _db.SubCategory.OrderBy(o => o.Name).Select(o => o.Name).Distinct().ToListAsync()
            };

            return View(model);
        }



        //GET - EDIT
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subCategory = await _db.SubCategory.SingleOrDefaultAsync(a => a.Id == id);

            if (subCategory == null)
            {
                return NotFound();
            }

            CategoryAndSubCategoryViewModel model = new CategoryAndSubCategoryViewModel()
            {
                CategoryList = await _db.Category.ToListAsync(),
                SubCategory = subCategory,
                SubCategoryList = await _db.SubCategory.OrderBy(o => o.Name).Select(o => o.Name).Distinct().ToListAsync()
            };

            return View(model);
        }


        //POST - DELETE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, CategoryAndSubCategoryViewModel model)
        {
            
                var doesSubCategoryExist = _db.SubCategory.Include(s => s.Category).Where(s =>
                    s.Name == model.SubCategory.Name && s.Category.Id == model.SubCategory.CategoryId);

                if (doesSubCategoryExist.Count() > 0)
                {
                    StatusMessage = "Error: SubCategory exist under " + doesSubCategoryExist.First().Category.Name +
                                    " category. Please use another name.";
                }
                else
                {
                    var subCatFromDb = await _db.SubCategory.FindAsync(id);
                    _db.SubCategory.Remove(subCatFromDb);


                    await _db.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            

            CategoryAndSubCategoryViewModel modelVM = new CategoryAndSubCategoryViewModel()
            {
                CategoryList = await _db.Category.ToListAsync(),
                SubCategory = model.SubCategory,
                SubCategoryList = await _db.SubCategory.OrderBy(o => o.Name).Select(o => o.Name).ToListAsync(),
                StatusMessage = StatusMessage

            };
            return View(modelVM);
        }


    }
}
