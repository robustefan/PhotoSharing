using PhotoSharing.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PhotoSharing.Controllers
{
    public class CategoryController : Controller
    {
        private PhotoDBContext db = new PhotoDBContext();

        public ActionResult Index()
        {
            var categories = from category in db.Categories
                         orderby category.Id
                         select category;
            return View(categories);
        }

        [Authorize(Roles ="Administrator")]
        public ActionResult New()
        {
            return View();
        }

        [Authorize(Roles ="Administrator")]
        [HttpPost]
        public ActionResult New(Category cat)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    db.Categories.Add(cat);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch(Exception e)
                {
                    return View("New");
                }
            }
            else
            {
                return View("New");
            }
        }

        [Authorize(Roles ="Administrator")]
        public ActionResult Edit(int id)
        {
            Category cat = db.Categories.Find(id);
            ViewBag.category = cat;
            return View();
        }

        [Authorize(Roles = "Administrator")]
        [HttpPut]
        public ActionResult Edit(int id, Category requestCategory)
        {
            try
            {
                Category category = db.Categories.Find(id);
                if(TryUpdateModel(category))
                {
                    category.Name = requestCategory.Name;
                    category.photo = requestCategory.photo;
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }catch(Exception e)
            {
                return View();
            }
        }

        [Authorize(Roles ="Administrator")]
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            Category category = db.Categories.Find(id);
            db.Categories.Remove(category);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}