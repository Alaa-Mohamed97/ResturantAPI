using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ResturantAPI.Models;

namespace ResturantAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly ResturantDBContext db;
        private readonly IHostingEnvironment _env;
        public ItemsController(ResturantDBContext db, IHostingEnvironment env)
        {
            this.db = db;
            this._env = env;
        }
        [HttpGet]
        public List<Item> AllItems()
        {
            return db.Item.ToList();

        }

        [HttpGet]
        public List<Item> GetItemsWithCatId(int CatId)
        {
            return db.Item.Where(a => a.CatId == CatId).ToList();

        }
        public IEnumerable<Category> Menu()
        {


            var getcategorieswithitems = db.Category.Where(a => a.Items.Count > 0).OrderBy(a => a.CatId)
                 .Select(a => new Category
                 {
                     CatId = a.CatId,
                     CatName = a.CatName,
                     Items = a.Items.ToList()
                 }).ToList();
            return getcategorieswithitems;
        }
        [HttpPost]
        public Item AddItem(Item model)
        {
            db.Item.Add(model);
            db.SaveChanges();
            return model;

        }
        [HttpPost]
        public void UpdateItem(Item model)
        {
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();
        }
        [HttpDelete]
        public void DeleteItem(int id)
        {
            var getitem = db.Item.Find(id);
            db.Item.Remove(getitem);
            db.SaveChanges();
        }
        [HttpGet]
        public Item ItemDetailes(int itemid)
        {
            return db.Item.Where(a => a.ItemId == itemid).FirstOrDefault();

        }
        [HttpPost]
        public JsonResult SaveFile()
        {
            try
            {
                Random rnd = new Random();
                var httprequest = Request.Form;
                var postedfile = httprequest.Files[0];
                string filename = Path.GetFileNameWithoutExtension(postedfile.FileName) + rnd.Next(0, 100000) + DateTime.Now.Millisecond.ToString() + Path.GetExtension(postedfile.FileName);
                var physicalpath = _env.WebRootPath + "/photos/" + filename;
                using (var stream = new FileStream(physicalpath, FileMode.Create))
                {
                    postedfile.CopyTo(stream);
                }
                return new JsonResult(filename);
            }
            catch (Exception)
            {

                return new JsonResult("anonymous.png");
            }
        }

        [HttpGet]
        public List<string> getCatName()
        {
            return db.Category.Select(a => a.CatName).ToList();
        }
    }
}