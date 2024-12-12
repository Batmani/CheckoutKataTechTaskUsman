public interface IDiscountService
{
    Task<decimal> CalculateDiscountAsync(IEnumerable<Guid> productIds);
    void AddDiscountRule(DiscountRule rule);
    void RemoveDiscountRule(Guid productId);
}