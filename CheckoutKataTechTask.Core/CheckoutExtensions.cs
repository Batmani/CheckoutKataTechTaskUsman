public static class CheckoutExtensions
{
    public static async Task ScanMultipleAsync(this ICheckout checkout, IEnumerable<Guid> productIds)
    {
        foreach (var id in productIds)
        {
            await checkout.ScanAsync(id);
        }
    }
}