using System;
using System.Collections.Generic;

namespace ResturantAPI.Models
{
    public partial class Customer
    {
        public Customer()
        {
            Order = new HashSet<Order>();
            ShoppingCart = new HashSet<ShoppingCart>();
        }

        public int CustomerId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

        public virtual ICollection<Order> Order { get; set; }
        public virtual ICollection<ShoppingCart> ShoppingCart { get; set; }
    }
}
