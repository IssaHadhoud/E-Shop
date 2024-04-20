using E_Shop.Data;
using E_Shop.Models;
using E_Shop.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_Shop.Controllers;

public class ProductController : Controller
{
private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public ProductController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
    {
        _context = context;
        _webHostEnvironment = webHostEnvironment;
    }
    

    // GET: Product
    public async Task<IActionResult> Index()
    {
        return _context.Products != null
            ? View(await _context.Products.ToListAsync())
            : Problem("Entity set 'ApplicationDbContext.Products'  is null.");
    }

    // GET: Product/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null || _context.Products == null)
        {
            return NotFound();
        }

        var product = await _context.Products
            .FirstOrDefaultAsync(m => m.ID == id);
        if (product == null)
        {
            return NotFound();
        }

        return View(product);
    }

    // GET: Product/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Product/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    public async Task<IActionResult> Create(ProductViewModel model)
    {
        if (ModelState.IsValid)
        {
            // Map the properties from the view model to the domain model
            var product = new Product
            {
                Name = model.Name,
                Detalis = model.Details,
                Price = model.Price,
                Type = model.Type
            };

            if (model.imageFile != null)
            {
                // Generate a unique file name using a GUID
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.imageFile.FileName);

                // Combine the file name with the uploads directory path
                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Images", fileName);

                // Copy the file to the server
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.imageFile.CopyToAsync(stream);
                }

                // Set the product image URL
                product.ImageURL = "/Images/" + fileName;
            }

            // Add the product to the database
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        return View(model);
    }

    // GET: /Product/Edit/5
    public IActionResult Edit(Guid id)
    {
        var product = _context.Products.Find(id);
        if (product == null)
        {
            return NotFound();
        }

        var model = new ProductViewModel
        {
            ID = product.ID,
            Name = product.Name,
            Details = product.Detalis,
            Price = product.Price,
            Type = product.Type
        };
        return View(model);
    }

    // POST: /Product/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, ProductViewModel model)
    {
        if (ModelState.IsValid)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            product.Name = model.Name;
            product.Detalis = model.Details;
            product.Price = model.Price;
            product.Type = model.Type;

            if (model.imageFile != null && model.imageFile.Length > 0)
            {
                var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "Images");
                var imageName = Guid.NewGuid().ToString() + Path.GetExtension(model.imageFile.FileName);
                var imageUrl = Path.Combine("Images", imageName);
                var fullPath = Path.Combine(imagePath, imageName);
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await model.imageFile.CopyToAsync(stream);
                }

                product.ImageURL = imageUrl;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Details", new { id = product.ID });
        }

        return View(model);
    }

    // GET: Product/Delete/5
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null || _context.Products == null)
        {
            return NotFound();
        }

        var product = await _context.Products
            .FirstOrDefaultAsync(m => m.ID == id);
        if (product == null)
        {
            return NotFound();
        }


        return View(product);
    }

    // POST: Product/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        if (_context.Products == null)
        {
            return Problem("Entity set 'ApplicationDbContext.Products'  is null.");
        }

        var product = await _context.Products.FindAsync(id);
        if (product != null)
        {
            _context.Products.Remove(product);
        }
        if (!string.IsNullOrEmpty(product.ImageURL))
        {
            var ImageURL = Path.Combine(_webHostEnvironment.WebRootPath, "Images" ,product.ImageURL);
            if (System.IO.File.Exists(ImageURL))
            {
                System.IO.File.Delete(ImageURL);
            }
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool ProductExists(Guid id)
    {
        return (_context.Products?.Any(e => e.ID == id)).GetValueOrDefault();
    }
}