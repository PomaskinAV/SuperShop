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
    public class CartService
    {
        private readonly ICartRepository _cartRepository;

        public CartService(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository ?? throw new ArgumentNullException(nameof(cartRepository));
        }

        public virtual async Task AddProduct(Guid accountId, Guid productId, double quantity, CancellationToken cancellationToken)
        {
            var cart = await _cartRepository.GetCartByAccountId(accountId, cancellationToken);
            cart.AddItem(productId, quantity);
            await _cartRepository.Update(cart, cancellationToken);
        }

        public virtual Task<Cart> GetAccountCart(Guid accountId, CancellationToken cancellationToken)
        {
            return _cartRepository.GetCartByAccountId(accountId, cancellationToken);
        }
    }
}
