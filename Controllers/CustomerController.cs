using E_Shop.Data;
using E_Shop.Models;
using E_Shop.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_Shop.Controllers;

public class CustomerController : Controller
{
    // GET
    private readonly ApplicationDbContext _context; 
    public CustomerController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegistrationViewModel model)
    {
        if (ModelState.IsValid)
        {
            var existingCustomer = await _context.Customers.FirstOrDefaultAsync(c => c.Email == model.Email);

            if (existingCustomer != null)
            {
                ModelState.AddModelError(string.Empty, "Email address already registered.");
                return View(model);
            }

            var customer = new Customer
            {
                Email = model.Email,
                Password = model.Password,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Address = model.Address
            };

            _context.Add(customer);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Product");
        }

        return View(model);
    }

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            var customer =
                await _context.Customers.FirstOrDefaultAsync(
                    c => c.Email == model.Email && c.Password == model.Password);

            if (customer != null)
            {
                // Add customer to session or authentication cookie
                return RedirectToAction("Index", "Product");
            }
        }

        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        return View(model);
    }
}