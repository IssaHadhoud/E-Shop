namespace E_Shop.ViewModel;

public class ProductViewModel
{
    public Guid ID { get; set; }
    public string Name { get; set; }
    public int Price { get; set; }
    public string Details { get; set; }
    public string Type { get; set; }
    public IFormFile imageFile { get; set; }
    
    
}