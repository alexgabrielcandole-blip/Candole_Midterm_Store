using Candole_Midterm_Store.Data;
using Candole_Midterm_Store.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lastname_Midterm_Store.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        // READ: show all cart items (the view computes the total)
        public async Task<IActionResult> Index()
        {
            var items = await _context.CartItems.OrderBy(c => c.Id).ToListAsync();
            return View(items);
        }

        // CREATE: Add to Cart (quantity 1; if already in the cart, quantity goes up by 1)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) return NotFound();

            var existing = await _context.CartItems.FirstOrDefaultAsync(c => c.ProductId == productId);
            if (existing != null)
            {
                existing.Quantity++;
            }
            else
            {
                _context.CartItems.Add(new CartItem
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    Price = product.Price,
                    Quantity = 1
                });
            }

            await _context.SaveChangesAsync();
            TempData["Message"] = $"\"{product.Name}\" was added to your cart.";
            return RedirectToAction("Index", "Products");
        }

        // UPDATE: change quantity
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateQuantity(int id, int quantity)
        {
            var item = await _context.CartItems.FindAsync(id);
            if (item == null) return NotFound();

            if (quantity < 1) quantity = 1;
            if (quantity > 999) quantity = 999;

            item.Quantity = quantity;
            await _context.SaveChangesAsync();
            TempData["Message"] = $"Quantity for \"{item.ProductName}\" updated.";
            return RedirectToAction(nameof(Index));
        }

        // DELETE: Remove from cart
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Remove(int id)
        {
            var item = await _context.CartItems.FindAsync(id);
            if (item == null) return NotFound();

            _context.CartItems.Remove(item);
            await _context.SaveChangesAsync();
            TempData["Message"] = $"\"{item.ProductName}\" was removed from your cart.";
            return RedirectToAction(nameof(Index));
        }
    }
}