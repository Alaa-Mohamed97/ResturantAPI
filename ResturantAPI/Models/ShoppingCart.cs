using System;
using System.Collections.Generic;

namespace ResturantAPI.Models
{
    public partial class ShoppingCart
    {
        public int CartId { get; set; }
        public int? ItemId { get; set; }
        public int? CustomerId { get; set; }
        public DateTime? CartDate { get; set; }
        public int? Quantity { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Item Item { get; set; }
    }
}
