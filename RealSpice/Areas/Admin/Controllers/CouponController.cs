using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RealSpice.Data;
using RealSpice.Models;

namespace RealSpice.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CouponController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CouponController(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _db.Coupon.ToListAsync());
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Coupon coupon)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0)
                {
                    byte[] p1 = null;
                    using (var fs1 = files[0].OpenReadStream())
                    {
                        using (var ms1 = new MemoryStream())
                        {
                            fs1.CopyTo(ms1);
                            p1 = ms1.ToArray();
                        }
                    }

                    coupon.Picture = p1;
                }

                _db.Coupon.Add(coupon);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(coupon);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            var couponFromDb = await _db.Coupon.Where(a => a.Id == id).SingleOrDefaultAsync();
            var img = String.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(couponFromDb.Picture));
            if (couponFromDb == null)
            {
                return NotFound();
            }

            ViewData["img"] = img;
            return View(couponFromDb);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Coupon coupon)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                var couponFromDb = await _db.Coupon.Where(a => a.Id == coupon.Id).SingleOrDefaultAsync();


                if (files.Count > 0)
                {
                    byte[] p1 = null;
                    using (var fs1 = files[0].OpenReadStream())
                    {
                        using (var ms1 = new MemoryStream())
                        {
                            fs1.CopyTo(ms1);
                            p1 = ms1.ToArray();
                        }
                    }

                    couponFromDb.Picture = p1;
                }

                couponFromDb.Name = coupon.Name;
                couponFromDb.Discount = coupon.Discount;
                couponFromDb.MinimumAmount = coupon.MinimumAmount;
                couponFromDb.CouponType = coupon.CouponType;
                couponFromDb.isActive = coupon.isActive;

                
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(coupon);
        }

        public async Task<IActionResult> Details(int? id)
        {
            var couponFromDb = await _db.Coupon.Where(a => a.Id == id).SingleOrDefaultAsync();
            var img = String.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(couponFromDb.Picture));
            if (couponFromDb == null)
            {
                return NotFound();
            }

            ViewData["img"] = img;
            return View(couponFromDb);
        }


        public async Task<IActionResult> Delete(int? id)
        {
            var couponFromDb = await _db.Coupon.Where(a => a.Id == id).SingleOrDefaultAsync();
            var img = String.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(couponFromDb.Picture));
            if (couponFromDb == null)
            {
                return NotFound();
            }

            ViewData["img"] = img;
            return View(couponFromDb);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Coupon coupon)
        {
            var couponFromDb = await _db.Coupon.Where(a => a.Id == coupon.Id).SingleOrDefaultAsync();

            if (couponFromDb == null)
            {
                return NotFound();
            }
                
            _db.Coupon.Remove(couponFromDb);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
