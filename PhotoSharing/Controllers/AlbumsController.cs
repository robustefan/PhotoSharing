using Microsoft.AspNet.Identity;
using PhotoSharing.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PhotoSharing.Controllers
{
    public class AlbumsController : Controller
    {
        private PhotoDBContext db = new PhotoDBContext();
        // GET: Albums

        [Authorize(Roles = "RegisteredUser")]
        public ActionResult Index()
        {
            string id = User.Identity.GetUserId();
            var albums = from album in db.Albums
                             where album.UserId == id
                             select album;
            ViewBag.Albums = albums;
            ViewBag.UserName = User.Identity.GetUserName();
            return View();
        }

        [Authorize(Roles = "RegisteredUser")]
        public ActionResult Show(int id)
        {
            Album album = db.Albums.Find(id);
            var photos = from photo in album.Photos select photo;
            ViewBag.Photos = photos;
            ViewBag.Album = album;
            return View();
        }

        [Authorize(Roles ="RegisteredUser")]
        public ActionResult New()
        {
            return View();
        }

        [Authorize(Roles = "RegisteredUser")]
        [HttpPost]
        public ActionResult New(Album album)
        {
            album.UserId = User.Identity.GetUserId();
            if (ModelState.IsValid)
            {
                try
                {
                    db.Albums.Add(album);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception e)
                {
                    return View("New");
                }
            }
            else
            {
                return View("New");
            }
        }

        [Authorize(Roles = "RegisteredUser")]
        public ActionResult AddPhotoToAlbum(int id)
        {
            string idUser = User.Identity.GetUserId();
            var photos = from photo in db.Photos
                         where photo.UserId == idUser
                         select photo;
            Album album = db.Albums.Find(id);

            ViewBag.Photos = photos;
            ViewBag.AlbumId = id;
            ViewBag.AlbumName = album.Name;
            return View();
        }

        [Authorize(Roles = "RegisteredUser")]
        public ActionResult AddPhotoToThisAlbum(int albumId, int photoId)
        {
            Album album = db.Albums.Find(albumId);
            Photo photo = db.Photos.Find(photoId);
            if (ModelState.IsValid)
            {
                album.Photos.Add(photo);
                string idUser = User.Identity.GetUserId();
                db.SaveChanges();
                return RedirectToAction("AddPhotoToAlbum/" + albumId);
            }
            else
                return RedirectToAction("AddPhotoToThisAlbum/" + albumId);
        }
    }
}