using System;
using System.ComponentModel.DataAnnotations;


namespace E_Shop.Models;


public class Customer
{
    [Key] public Guid ID { get; set; }
    [Required] public string FirstName { get; set; }
    [Required] public string LastName { get; set; }
    [Required] public string Email { get; set; }
    [Required] public string Password { get; set; }
    [Required] public string Address { get; set; }
}
    
