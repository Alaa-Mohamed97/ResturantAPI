using System;
using System.Collections.Generic;

namespace ResturantAPI.Models
{
    public partial class OrderItems
    {
        public long OrderItemId { get; set; }
        public long? OrderId { get; set; }
        public int? ItemId { get; set; }
        public int? Quantity { get; set; }

        public virtual Item Item { get; set; }
        public virtual Order Order { get; set; }
    }
}
