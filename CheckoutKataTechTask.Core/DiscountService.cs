public class DiscountService
{
    public DiscountService(IProductRepository repository) { }
    private readonly Dictionary<Guid, DiscountRule> _discountRules = new();

    public void AddDiscountRule(DiscountRule rule)
    {
        _discountRules[rule.ProductId] = rule;
    }
}
