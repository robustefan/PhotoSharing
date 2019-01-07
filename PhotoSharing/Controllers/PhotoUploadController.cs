using PhotoSharing.Models;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.IO;
using System.Collections.Generic;

namespace PhotoSharing.Controllers
{
    public class PhotoUploadController : Controller
    {
        private PhotoDBContext db = new PhotoDBContext();

        public ActionResult Index()
        {
            var photos = from photo in db.Photos
                         orderby photo.Id descending
                         select photo;
            return View(photos.Take(12));
        }

        [Authorize(Roles = "RegisteredUser,Administrator")]
        public ActionResult New()
        {
            var categories = from category in db.Categories
                         orderby category.Id
                         select category;
            return View(categories);
        }

        public ActionResult ShowRecentPhotos()
        {
            var Photos = from photo in db.Photos
                         orderby photo.Id descending
                         select photo;
            return View(Photos.Take(12));
        }

        [Authorize(Roles ="RegisteredUser,Administrator")]
        [HttpPost]
        public ActionResult New(Photo photo,HttpPostedFileBase PostedImage, Category category)
        {
            photo.UserId = User.Identity.GetUserId();
            Category cat = db.Categories.Find(category.Id);
            photo.category = cat;
            photo.CategoryId = cat.Id;
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
                    cat.Photos.Add(photo);
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
            var categories = from category in db.Categories
                               where category.Id == photo.CategoryId
                               select category;
            var comments = from comment in db.Comments
                           where comment.PhotoId == id
                           select comment;
            ViewBag.Category = categories;
            ViewBag.Comments = comments;
            return View();
        }

        [HttpPost]
        public ActionResult Search(string searchString)
        {
            var photos = db.Photos.ToList().Where(x => x.Name.ToLower().Contains(searchString.ToLower()));
            var categories = db.Categories.ToList().Where(x => x.Name.ToLower().Contains(searchString.ToLower()));
            List<Photo> photoList = new List<Photo>();
            photoList = photos.ToList();
            foreach(Category category in categories)
            {
                foreach (Photo photo in category.Photos)
                {
                    photoList.Add(photo);
                }
            }
           
            ViewBag.Photos = photoList.Distinct();
            return View();
        }

        [Authorize(Roles="RegisteredUser,Administrator")]
        public ActionResult Edit(int id)
        {
            Photo photo = db.Photos.Find(id);
            ViewBag.Photo = photo;
            return View();
        }

        [Authorize(Roles = "RegisteredUser,Administrator")]
        [HttpPut]
        public ActionResult Edit(int id, Photo requestPhoto)
        {
            try
            {
                Photo photo = db.Photos.Find(id);
                if (User.Identity.GetUserId().Equals(photo.UserId))
                {
                    if (TryUpdateModel(photo))
                    {
                        photo.Name = requestPhoto.Name;
                        photo.Description = requestPhoto.Description;
                        db.SaveChanges();
                        return RedirectToAction("Show", "PhotoUpload", new { id = id });
                    }
                    else
                        return RedirectToAction("Show", "PhotoUpload", new { id = id });
                }
                else
                    return View();
            }
            catch(Exception e)
            {
                return View();
            }
        }

        [HttpDelete]
        public ActionResult Delete(int Id)
        {

            Photo photo = db.Photos.Find(Id);
            if (photo.UserId.Equals(User.Identity.GetUserId()) || User.IsInRole("Administrator"))
            {
                db.Photos.Remove(photo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
                return View();
        }

    }
}