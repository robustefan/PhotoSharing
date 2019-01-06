using PhotoSharing.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace PhotoSharing.Controllers
{
    public class CommentController : Controller
    {
        private PhotoDBContext db = new PhotoDBContext();
        // GET: Comment


        public ActionResult Index()
        {
            var comments = from comment in db.Comments
                           orderby comment.Id
                           select comment;
            return View(comments);
        }

        public ActionResult New(int PhotoId)
        {
            Photo photo = db.Photos.Find(PhotoId);
            return View(photo);
        }

        [HttpPost]
        public ActionResult New(int Id, Comment comment)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    comment.PhotoId = Id;
                    db.Comments.Add(comment);
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
    }
}