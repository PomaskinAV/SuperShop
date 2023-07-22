using EmptyBlazorApp;
using OnlineShop.HttpModel.Requests;
using System.Net.Http.Json;

namespace OnlineShop.HttpApiClient
{
    public class MyShopClient : IDisposable, IMyShopClient
	{
		private readonly string _host;
		private readonly HttpClient _httpClient;

		public MyShopClient(string host = "http://myshop.com/", HttpClient? httpClient = null)
		{
			//ArgumentException.ThrowIfNullOrEmpty(host);
			if (!Uri.TryCreate(host, UriKind.Absolute, out var hostUri))
			{
				throw new ArgumentException("The host address should be a valid url", nameof(host));
			}
			_host = host;
			_httpClient = httpClient ?? new HttpClient();
			if (_httpClient.BaseAddress is null)
			{
				_httpClient.BaseAddress = hostUri;
			}
		}

		public void Dispose()
		{
			((IDisposable)_httpClient).Dispose();
		}

		public async Task<Product> GetProduct(Guid id, CancellationToken cancellationToken)
		{
			var product = await _httpClient.GetFromJsonAsync<Product>($"get_product?id={id}", cancellationToken);
			if (product is null)
			{
				throw new InvalidOperationException("The server returned null product");
			}
			return product;
		}

		public async Task AddProduct(Product product, CancellationToken cancellationToken)
		{
			ArgumentNullException.ThrowIfNull(product);
			using var response = await _httpClient.PostAsJsonAsync("add_products", product, cancellationToken);
			response.EnsureSuccessStatusCode();
		}

        public async Task<Product[]> GetProducts(CancellationToken cancellationToken)
        {
            var products = await _httpClient.GetFromJsonAsync<Product[]>($"get_products", cancellationToken);
            return products;
        }

        public async Task UpdateProduct(Product product, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(nameof(product));
            await _httpClient!
                .PostAsJsonAsync($"update_product", product, cancellationToken);
        }

        public async Task Register(RegisterRequest reqest, CancellationToken cancellationToken)
        {
			ArgumentNullException.ThrowIfNull(reqest);

			var uri = "/account/register";
			using var response = await _httpClient.PostAsJsonAsync(uri, reqest, cancellationToken);
			response.EnsureSuccessStatusCode();
        }
    }
}
