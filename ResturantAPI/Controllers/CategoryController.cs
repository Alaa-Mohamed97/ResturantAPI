using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ResturantAPI.Models;

namespace ResturantAPI.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ResturantDBContext db;

        public CategoryController(ResturantDBContext db)
        {
            this.db = db;
        }
        [HttpGet]
        public List<Category> getCategory()
        {
            return db.Category.ToList();
        }
        [HttpPost]
        public void AddCategory(Category model)
        {
            db.Category.Add(model);
            db.SaveChanges();
        }
        [HttpPost]
        public void EditCategory(Category model)
        {
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();
        }
        [HttpDelete]
        public void DeleteCategory(int id)
        {
          var getcategory=  db.Category.Find(id);
            db.Category.Remove(getcategory);
            db.SaveChanges();

        }
    }
}