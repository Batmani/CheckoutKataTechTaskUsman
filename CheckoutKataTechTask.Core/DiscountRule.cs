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
