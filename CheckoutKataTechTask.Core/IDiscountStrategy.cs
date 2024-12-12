public interface IDiscountStrategy
{
    decimal CalculateDiscount(IEnumerable<Product> products);
}
