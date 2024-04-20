using System.ComponentModel.DataAnnotations;

namespace E_Shop.Models;

public class Product
{
    [Key]
    public Guid ID { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string Detalis { get; set; }
    [Required]
    public int Price { get; set; }
    [Required]
    public string ImageURL { get; set; }
    public string Type { get; set; }
}