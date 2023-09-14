using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Domain.Entities
{
    public class Cart : IEntity
    {
        public Guid Id { get; init; }
        public Guid AccountId { get; set; }

        public List<CartItem>? Items { get; set; }

        public record CartItem : IEntity
        {
            protected CartItem() { }

            public CartItem(Guid id, Guid productId, double quantity)
            {
                Id = id;
                ProductId = productId;
                Quantity = quantity;
            }

            public Guid Id { get; init; }

            public Guid ProductId { get; init; }
            public double Quantity { get; set; }

            public Cart Cart { get; set; } = null!;
        }

        public void AddItem(Guid productId, double quantity)
        {
            if (quantity <= 0) throw new ArgumentOutOfRangeException(nameof(quantity));
            if (Items == null) throw new InvalidOperationException("Cart items is null");
            var existedItem = Items!.SingleOrDefault(item => item.ProductId == productId);
            if (existedItem is null)
            {
                Items.Add(new CartItem(Guid.Empty, productId, quantity));
            }
            else
            {
                existedItem.Quantity += quantity;
            }
        }
    }
}
