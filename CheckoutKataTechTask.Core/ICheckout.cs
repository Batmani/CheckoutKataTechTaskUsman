public interface ICheckout
{
    Task ScanAsync(Guid productId);
    Task<int> GetTotalPriceAsync();
}
