using static Program;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
    }
}
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

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(Guid id);
    Task AddAsync(Product product);
}

public class InMemoryProductRepository : IProductRepository
{
    private readonly Dictionary<Guid, Product> _products = new();

    public Task<Product?> GetByIdAsync(Guid id)
    {
        _products.TryGetValue(id, out var product);
        return Task.FromResult(product);
    }

    public Task AddAsync(Product product)
    {
        _products[product.Id] = product;
        return Task.CompletedTask;
    }
}
public class DiscountRule
{
    public Guid ProductId { get; }
    public IDiscountStrategy Strategy { get; }

    public DiscountRule(Guid productId, IDiscountStrategy strategy)
    {
        ProductId = productId;
        Strategy = strategy;
    }
}
public class DiscountService
{
    public DiscountService(IProductRepository repository) { }
    private readonly Dictionary<Guid, DiscountRule> _discountRules = new();

    public void AddDiscountRule(DiscountRule rule)
    {
        _discountRules[rule.ProductId] = rule;
    }
}

public class Checkout : ICheckout
{
    private readonly List<Guid> _scannedItems = new();
    private readonly IProductRepository _productRepository;

    public Checkout(
        IProductRepository productRepository,
        DiscountService discountService
      )
    {
        _productRepository = productRepository;
    }

    public async Task ScanAsync(Guid productId)
    {
        _scannedItems.Add(productId);
    }

    public async Task<int> GetTotalPriceAsync()
    {
        decimal total = 0;
        foreach (var id in _scannedItems)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product != null)
            {
                total += product.UnitPrice;
            }
        }
        return (int)total;
    }
}
public interface ICheckout
{
    Task ScanAsync(Guid productId);
    Task<int> GetTotalPriceAsync();
}

public interface IDiscountStrategy
{
    decimal CalculateDiscount(IEnumerable<Product> products);
}

public class BulkDiscountStrategy : IDiscountStrategy
{
    private readonly int _quantity;
    private readonly decimal _specialPrice;

    public BulkDiscountStrategy(int quantity, decimal specialPrice)
    {
        _quantity = quantity;
        _specialPrice = specialPrice;
    }

    public decimal CalculateDiscount(IEnumerable<Product> products)
    {
        var count = products.Count();
        var regularPrice = products.Sum(p => p.UnitPrice);
        var specialOfferSets = count / _quantity;
        var remainingItems = count % _quantity;
        var discountedPrice = (specialOfferSets * _specialPrice) +
                              (remainingItems * (products.FirstOrDefault()?.UnitPrice ?? 0));

        return regularPrice - discountedPrice;
    }
}