
public class DiscountService : IDiscountService
{
    private readonly Dictionary<Guid, DiscountRule> _discountRules = new();
    private readonly IProductRepository _productRepository;

    public DiscountService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<decimal> CalculateDiscountAsync(IEnumerable<Guid> productIds)
    {
        var groupedProducts = new Dictionary<Guid, List<Product>>();

        foreach (var id in productIds)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null) continue;

            if (!groupedProducts.ContainsKey(product.Id))
                groupedProducts[product.Id] = new List<Product>();

            groupedProducts[product.Id].Add(product);
        }

        decimal totalDiscount = 0;
        foreach (var group in groupedProducts)
        {
            if (_discountRules.TryGetValue(group.Key, out var rule))
            {
                totalDiscount += rule.Strategy.CalculateDiscount(group.Value);
            }
        }

        return totalDiscount;
    }

    public void AddDiscountRule(DiscountRule rule)
    {
        _discountRules[rule.ProductId] = rule;
    }

    public void RemoveDiscountRule(Guid productId)
    {
        _discountRules.Remove(productId);
    }
}