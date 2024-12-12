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