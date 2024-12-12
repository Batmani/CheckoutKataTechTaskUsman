public class Product
{
    public Guid Id { get; }
    public string Code { get; }
    public decimal UnitPrice { get; set; }
    public string Description { get; }
    public DateTime LastUpdated { get; private set; }

    public Product(Guid id, string code, decimal unitPrice, string description)
    {
        Id = id;
        Code = code;
        UnitPrice = unitPrice;
        Description = description;
        LastUpdated = DateTime.UtcNow;
    }

    public void UpdatePrice(decimal newPrice)
    {
        UnitPrice = newPrice;
        LastUpdated = DateTime.UtcNow;
    }
}