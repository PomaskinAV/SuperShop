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

        public List<CartItem>? Items;

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
    }
}
