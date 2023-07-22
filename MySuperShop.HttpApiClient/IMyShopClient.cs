using EmptyBlazorApp;
using OnlineShop.HttpModel.Requests;

namespace OnlineShop.HttpApiClient
{
	public interface IMyShopClient
	{
		Task AddProduct(Product product, CancellationToken cancellationToken);
		Task<Product> GetProduct(Guid id, CancellationToken cancellationToken);
        Task<Product[]> GetProducts(CancellationToken cancellationToken);
        Task UpdateProduct(Product product, CancellationToken cancellationToken);
        Task DeleteProduct(Product product, CancellationToken cancellationToken);
        Task Register(RegisterRequest account, CancellationToken cancellationToken);
    }
}