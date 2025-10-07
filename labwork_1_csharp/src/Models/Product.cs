
namespace labwork_1_csharp.Models
{
    public class Product
    {
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }

    public Product(int id, string name, decimal price, int quantity)
    {
        Id = id;
        Name = name;
        Price = price;
        Quantity = quantity;
    }
    
    public override string ToString() => $"{Id}.{Name} \nСтоимость:{Price:C} \nВ наличии:{Quantity}";
    
    public Product Clone() => new Product(Id, Name, Price, Quantity);
    }
}