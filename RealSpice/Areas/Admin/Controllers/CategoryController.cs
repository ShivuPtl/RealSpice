using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealSpice.Data;
using RealSpice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealSpice.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;
        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }

        //GET   
        public async Task<IActionResult> Index()
        {
            return View(await  _db.Category.ToListAsync());
        }

        //GET - Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if (ModelState.IsValid)
            {
                _db.Category.Add(category);
                await _db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(category);


        }

        //GET - EDIT
        public async Task<IActionResult> Edit(int? Id)
        {
            if(Id == null)
            {
                return NotFound();
            }
            else
            {
                var category = await _db.Category.FindAsync(Id);
                if(category == null)
                {
                    return NotFound();
                }
                else
                {
                    return View(category);
                }
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                _db.Update(category);
                await _db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }



        //GET - DELETE
        public async Task<IActionResult> Delete(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }
            else
            {
                var category = await _db.Category.FindAsync(Id);
                if (category == null)
                {
                    return NotFound();
                }
                else
                {
                    return View(category);
                }
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Category category)
        {

                _db.Remove(category);
                await _db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
        
        }


        //GET - DETAILS
        public async Task<IActionResult> Details(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }
            else
            {
                var category = await _db.Category.FindAsync(Id);
                if (category == null)
                {
                    return NotFound();
                }
                else
                {
                    return View(category);
                }
            }
        }


    }
}
