namespace MyShopBackend.Data
{
	public class Product : IEntity
	{
		private Product()
		{

		}
		public Guid Id { get; init; }
		public string Name { get; set; }
		public decimal Price { get; set; }
	}
}