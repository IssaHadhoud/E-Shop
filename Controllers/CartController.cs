using System.Diagnostics;
using E_Shop.Data;
using Microsoft.AspNetCore.Mvc;
using E_Shop.Models;
using Newtonsoft.Json;
using System.Net;
using E_Shop.Models;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;

namespace E_Shop.Controllers;

public class CartController : Controller
{
    private readonly ApplicationDbContext _db;

    public CartController(ApplicationDbContext db)
    {
        _db = db;
    }

    public IActionResult AddToCart(Guid id)
    {
        var product = _db.Products.Find(id);
        if (product != null)
        {
            var cart = GetCartFromCookie();
            cart.Add(product);
            SaveCartToCookie(cart);
        }

        return RedirectToAction("Cart");
    }

    public IActionResult Cart()
    {
        var cart = GetCartFromCookie();
        return View(cart);
    }



    private List<Product> GetCartFromCookie()
    {
        var cartJson = Request.Cookies["Cart"];
        if (cartJson != null)
        {
            return JsonConvert.DeserializeObject<List<Product>>(cartJson);
        }

        return new List<Product>();
    }
    private void SaveCartToCookie(List<Product> cart)
    {
        var cartJson = JsonConvert.SerializeObject(cart);
        Response.Cookies.Append("Cart", cartJson);
    }

    
    

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _db.Dispose();
        }
        base.Dispose(disposing);
    }

  public IActionResult RemoveFromCart(Guid id)
{
    var cart = GetCartFromCookie(); // Retrieve the cart from the cookie
    cart.RemoveAll(item => item.ID == id);  // Remove the specified item from the cart
    SaveCartToCookie(cart); // Save the updated cart to the cookie

    return RedirectToAction("Cart");
}
}