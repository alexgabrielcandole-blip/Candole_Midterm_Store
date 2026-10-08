using Candole_Midterm_Store.Data;
using Candole_Midterm_Store.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Candole_Midterm_Store.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

       
        public async Task<IActionResult> Index(string? search)
        {
            var query = _context.Products.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = search.Trim().ToLower();
                query = query.Where(p => p.Name.ToLower().Contains(term));
            }

            ViewData["Search"] = search?.Trim();

            var products = await query.OrderBy(p => p.Name).ToListAsync();
            return View(products);
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description,Price,Category")] Product product)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }

            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            TempData["Message"] = $"\"{product.Name}\" was added.";
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Price,Category")] Product product)
        {
            if (id != product.Id) return NotFound();

            if (!ModelState.IsValid)
            {
                return View(product);
            }

            var existing = await _context.Products.FindAsync(id);
            if (existing == null) return NotFound();

            existing.Name = product.Name;
            existing.Description = product.Description;
            existing.Price = product.Price;
            existing.Category = product.Category;

            await _context.SaveChangesAsync();
            TempData["Message"] = $"\"{existing.Name}\" was updated.";
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();

           
            var cartItems = _context.CartItems.Where(c => c.ProductId == product.Id);
            _context.CartItems.RemoveRange(cartItems);

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            TempData["Message"] = $"\"{product.Name}\" was deleted.";
            return RedirectToAction(nameof(Index));
        }
    }
}