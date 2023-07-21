using EmptyBlazorApp;
using MySuperShop.HttpApiClient;

namespace MySuperShop
{
	public interface IMyShopClient
	{
		Task AddProduct(Product product, CancellationToken cancellationToken);
		Task<Product> GetProduct(Guid id, CancellationToken cancellationToken);
        Task<Product[]> GetProducts(CancellationToken cancellationToken);
        Task UpdateProduct(Product product, CancellationToken cancellationToken);
        Task Register(Account account, CancellationToken cancellationToken);
    }
}