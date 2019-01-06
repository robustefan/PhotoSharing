using PhotoSharing.Models;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.IO;

namespace PhotoSharing.Controllers
{
    public class PhotoUploadController : Controller
    {
        private PhotoDBContext db = new PhotoDBContext();

        public ActionResult Index()
        {
            var photos = from photo in db.Photos
                         orderby photo.Id
                         select photo;
            return View(photos);
        }

        public ActionResult New()
        {
            var categories = from category in db.Categories
                         orderby category.Id
                         select category;
            return View(categories);
        }

        [HttpPost]
        public ActionResult New(Photo photo,HttpPostedFileBase PostedImage, Category category)
        {
            photo.UserId = User.Identity.GetUserId();
            Category cat = db.Categories.Find(category.Id);
            Category cpy = cat;
            if (cpy != null)
            {
                db.Categories.Add(cpy);
                photo.Categories.Add(cat);
            }
            if (PostedImage != null)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    PostedImage.InputStream.CopyTo(ms);
                    photo.Image = ms.GetBuffer();
                }
            }
            try
            {
                if (ModelState.IsValid)
                {                
                    db.Photos.Add(photo);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    return View("New");
                }
            }
            catch (Exception e)
            {
                return View("New");
            }
        }

        public ActionResult Show(int id)
        {
            Photo photo = db.Photos.Find(id);
            ViewBag.Photo = photo;
            return View();
        }

    }
}