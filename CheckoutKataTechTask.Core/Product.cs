public class Product
{
    public Guid Id { get; }
    public string Code { get; }
    public decimal UnitPrice { get; }
    public string Description { get; }

    public Product(Guid id, string code, decimal unitPrice, string description)
    {
        Id = id;
        Code = code;
        UnitPrice = unitPrice;
        Description = description;
    }
}
