using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ResturantAPI.Models;

namespace ResturantAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        private readonly ResturantDBContext db;

        public ShoppingCartController(ResturantDBContext db)
        {
            this.db = db;
        }
        public int IsExisting(ShoppingCart SHcart)
        {
            List<ShoppingCart> cart = db.ShoppingCart.ToList();
            for (int i = 0; i < cart.Count; i++)
                if (cart[i].ItemId == SHcart.ItemId && cart[i].CustomerId == SHcart.CustomerId)
                    return cart[i].CartId;
            return -1;
        }

        public void OrderNow(ShoppingCart cart)

        {
            cart.CartDate = DateTime.Now;
            int CartId = IsExisting(cart);
            if (CartId == -1)
            {
                db.ShoppingCart.Add(cart);
                db.SaveChanges();
            }
            else
            {
                var getcart = db.ShoppingCart.Where(a => a.CartId == CartId).FirstOrDefault();
                getcart.Quantity++;
                db.Entry(getcart).State = EntityState.Modified;
                db.SaveChanges();
            }


           
        }

        public JsonResult GetCartItems(int id)
        {

            var cartitem = (from i in db.Item
                            join c in db.ShoppingCart on i.ItemId equals c.ItemId
                            where c.CustomerId == id
                            select new
                            {
                                cartId=c.CartId,
                                Name = i.Name,
                                ItemId = i.ItemId,
                                Quantity = c.Quantity,
                                Date = c.CartDate,
                                Price = i.Price,
                                Img = i.Image
                            }).ToList();
            return new JsonResult(cartitem);
        }

        public void ConfirmOrder(Order order)
        {
            order.OrderDate = DateTime.Now;
            db.Order.Add(order);
            db.SaveChanges();

            var getcustomershoppingcart = db.ShoppingCart.Where(a => a.CustomerId == order.CustomerId).ToList();
            foreach (var item in getcustomershoppingcart)
            {
                db.ShoppingCart.Remove(item);
                db.SaveChanges();
            }
        }

        public JsonResult GetCustomerCartCoun(int id)
        {
            var customercartcount = db.ShoppingCart.Where(a => a.CustomerId == id).ToList().Count;
            return new JsonResult(customercartcount);
        }
        [HttpGet]
        public void plus(int id)
        {
            var item = db.ShoppingCart.Where(a => a.CartId == id).FirstOrDefault();
            item.Quantity += 1;
            db.Entry(item).State = EntityState.Modified;
            db.SaveChanges();
        }
       [HttpGet]
        public void minus(int id)
        {
            var item = db.ShoppingCart.Where(a => a.CartId == id).FirstOrDefault();
            if (item.Quantity > 1)
            {
                item.Quantity -= 1;
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
            }
            
        }
        [HttpDelete]
        public void DeleteShopincartitem(int id)
        {
            var getitem = db.ShoppingCart.Find(id);
            db.ShoppingCart.Remove(getitem);
            db.SaveChanges();
        }

    }
}