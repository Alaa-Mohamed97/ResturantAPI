using System;
using System.Collections.Generic;

namespace ResturantAPI.Models
{
    public partial class Item
    {
        public Item()
        {
            OrderItems = new HashSet<OrderItems>();
            ShoppingCart = new HashSet<ShoppingCart>();
        }

        public int ItemId { get; set; }
        public string Name { get; set; }
        public decimal? Price { get; set; }
        public string Image { get; set; }
        public int? CatId { get; set; }

        public virtual Category Cat { get; set; }
        public virtual ICollection<OrderItems> OrderItems { get; set; }
        public virtual ICollection<ShoppingCart> ShoppingCart { get; set; }
    }
}
