using System;
using System.Collections.Generic;

namespace ResturantAPI.Models
{
    public partial class Order
    {
        public Order()
        {
            OrderItems = new HashSet<OrderItems>();
        }

        public long OrderId { get; set; }
        public string Address { get; set; }
        public int? CustomerId { get; set; }
        public string UserName { get; set; }
        public decimal? Gtotal { get; set; }
        public DateTime? OrderDate { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual ICollection<OrderItems> OrderItems { get; set; }
    }
}
