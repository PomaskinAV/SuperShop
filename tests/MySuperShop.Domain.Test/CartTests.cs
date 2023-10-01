using OnlineShop.Domain.Entities;

namespace MySuperShop.Domain.Test
{
    public class CartTests
    {
        [Fact]
        public void Item_is_added_to_cart()
        {
            // Arrange
            var cart = new Cart(Guid.NewGuid(), Guid.NewGuid());
            var productId = Guid.NewGuid();

            // Act
            cart.AddItem(productId, 1d);

            // Assert
            Assert.Single(cart.Items!);
        }

        [Fact]
        public void Adding_an_existing_item_to_cart_increases_its_quantity()
        {
            // Arrange
            var cart = new Cart(Guid.NewGuid(), Guid.NewGuid());
            var productId = Guid.NewGuid();

            // Act
            cart.AddItem(productId, 1d);
            cart.AddItem(productId, 1d);

            // Assert
            Assert.Single(cart.Items!);
            Assert.Equal(2d, cart.Items!.First().Quantity);
        }

        [Fact]
        public void Five_items_added_to_cart()
        {
            // Arrange
            var cartItems = new List<CartItem>();
            for (var i = 1; i < 6; ++i)
            {
                cartItems.Add(new CartItem(Guid.NewGuid(), Guid.NewGuid(), i));
            }

            var cart = new Cart(Guid.NewGuid(), Guid.NewGuid());

            // Act
            foreach (var it in cartItems)
            {
                cart.AddItem(it.ProductId, it.Quantity);
            }

            // Assert
            foreach (var it in cartItems)
            {
                var currentItem = cart.Items!.FirstOrDefault(item => item.ProductId == it.ProductId);
                Assert.NotNull(currentItem);

                Assert.Equal(it.Quantity, currentItem.Quantity);
            }

            Assert.Equal(cartItems.Count, cart.Items!.Count);
        }
    }
}