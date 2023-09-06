using OnlineShop.Domain.Entities;
using OnlineShop.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OnlineShop.Domain.Services
{
    internal class CartService
    {
        private readonly ICartRepository _cartRepository;

        public CartService(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository ?? throw new ArgumentNullException(nameof(cartRepository));
        }

        public virtual async Task AddProduct(Guid accountId, Product product, CancellationToken cancellationToken, double quantity = 1d)
        {
            if (product == null) throw new ArgumentNullException(nameof(product));
            Cart cart = await _cartRepository.GetCartByAccountId(accountId, cancellationToken);
            var existedItem = cart.Items.FirstOrDefault(item => item.ProductId == product.Id);
            if(existedItem is null)
            {
                cart.Items.Add(new Cart.CartItem(Guid.Empty, product.Id, quantity));
            }
            else
            {
                existedItem.Quantity += quantity;
            }
                        
            await _cartRepository.Update(cart, cancellationToken);
        }

        public virtual Task<Cart> GetAccountCart(Guid accountId, CancellationToken cancellationToken)
        {
            return _cartRepository.GetCartByAccountId(accountId, cancellationToken);
        }
    }
}
