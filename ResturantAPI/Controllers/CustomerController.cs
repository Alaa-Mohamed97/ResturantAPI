using System;
using System.Collections.Generic;

using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ResturantAPI.Models;
using EntityState = Microsoft.EntityFrameworkCore.EntityState;

namespace ResturantAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ResturantDBContext db;

        public CustomerController(ResturantDBContext db)
        {
            this.db = db;
        }
        [HttpPost]
        public Customer Register(Customer model)
        {
            var getcustomer = db.Customer.Where(a => a.Email == model.Email).FirstOrDefault();
            if (getcustomer == null)
            {
                db.Customer.Add(model);
                db.SaveChanges();
                return model;
            }
            else
            {
                return null;
            }
        }
        [HttpPost]
        public Customer Login(Customer model)
        {

            var getcustomer = db.Customer.Where(a => a.Email == model.Email && a.Password == model.Password).FirstOrDefault();
            if (getcustomer != null)
            {
                return getcustomer;
            }
            else
            {
                return null;
            }
        }
        [HttpPost]
        public Customer EditUserName(Customer model)
        {
            var getcustomer = db.Customer.Where(a => a.CustomerId == model.CustomerId).FirstOrDefault();
            if (getcustomer != null)
            {
                getcustomer.Name = model.Name;
                db.Entry(getcustomer).State = EntityState.Modified;
                db.SaveChanges();
                return getcustomer;
            }
            else
            {
                return null;
            }
        }
        [HttpPost]
        public Customer changePassword(Customer model)
        {
            var getcustomer = db.Customer.Where(a => a.CustomerId == model.CustomerId).FirstOrDefault();
            if (getcustomer != null)
            {
                //if old passowrd not equal the new password
                if (getcustomer.Password != model.Password)
                {
                    getcustomer.Password = model.Password;
                    getcustomer.ConfirmPassword = model.ConfirmPassword;
                    db.Entry(getcustomer).State = EntityState.Modified;
                    db.SaveChanges();
                    return getcustomer;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        [HttpGet]

        public JsonResult GetCustomerWithOrders()
        {

            var customerorders = (from c in db.Customer
                                  join o in db.Order on c.CustomerId equals o.CustomerId into g
                                  from x in g.DefaultIfEmpty()

                                  select new
                                  {
                                      Name = c.Name,
                                      ordercount = c.Order.Count,


                                  }).ToList().Distinct().OrderByDescending(a => a.ordercount);
            return new JsonResult(customerorders);
        }

        [HttpGet]

        public JsonResult GetOrdersOfCustomer(int id)
        {
       
            var getorders = db.Order.Where(a => a.CustomerId == id).ToList();
            var aa = (from or in db.Order
                      where or.CustomerId == id
                      select new
                      {
                          orderno = or.OrderId,
                          gtotal=or.Gtotal,
                          address=or.Address,
                          orderdate=or.OrderDate,
                          orderitem = (from o in db.OrderItems
                                       join i in db.Item on o.ItemId equals i.ItemId
                                       where o.OrderId == or.OrderId
                                       select new
                                       {
                                           quantity = o.Quantity,
                                           name = i.Name,
                                           price = i.Price
                                       })
                      }
                    ).OrderByDescending(a=>a.orderno);
            return new JsonResult(aa);
        }

        public JsonResult GetOrderItems(int id)
        {
            var orderitemlist = (from o in db.OrderItems
                                 join i in db.Item on o.ItemId equals i.ItemId
                                 where o.OrderId == id
                                 select new
                                 {
                                     quantity = o.Quantity,
                                     name = i.Name,
                                     price = i.Price
                                 });
            return new JsonResult(orderitemlist);
        }

    }
}