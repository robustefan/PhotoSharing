using Microsoft.AspNet.Identity;
using PhotoSharing.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace PhotoSharing.Controllers
{
    [Authorize(Roles ="RegisteredUser,Administrator")]
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
                    comment.UserId = User.Identity.GetUserId();
                    comment.UserName = User.Identity.GetUserName();
                    comment.PhotoId = Id;
                    db.Comments.Add(comment);
                    db.SaveChanges();
                    return RedirectToAction("Show","PhotoUpload",new { id = Id});
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

        public ActionResult Edit(int commentId)
        {
            Comment comment = db.Comments.Find(commentId);
            ViewBag.Comment = comment;
            return View();
        }

        [HttpPut]
        public ActionResult Edit(int Id, Comment requestComment)
        {
            try
            {
                Comment comment = db.Comments.Find(Id);
                if (comment.UserId.Equals(User.Identity.GetUserId()))
                {
                    if (TryUpdateModel(comment))
                    {
                        comment.Text = requestComment.Text;
                        db.SaveChanges();
                    }
                    return RedirectToAction("Show", "PhotoUpload", new { id = comment.PhotoId });
                }
                else
                    return RedirectToAction("Show", "PhotoUpload", new { id = comment.PhotoId });
            }
            catch (Exception e)
            {
                return View();
            }
        }

        [HttpDelete]
        public ActionResult Delete(int Id)
        {
            Comment comment = db.Comments.Find(Id);
            if (comment.UserId.Equals(User.Identity.GetUserId()))
            {
                db.Comments.Remove(comment);
                db.SaveChanges();
                return RedirectToAction("Show", "PhotoUpload", new { id = comment.PhotoId });
            }
            else
                return RedirectToAction("Show", "PhotoUpload", new { id = comment.PhotoId });
        }

        [HttpDelete]
        public ActionResult Approve(int Id)
        {
            Comment comment = db.Comments.Find(Id);
            if(!comment.UserId.Equals(User.Identity.GetUserId()))
            {                
                if (TryUpdateModel(comment))
                {
                    comment.AcceptedOrDeclined = 1;
                    db.SaveChanges();
                }
                return RedirectToAction("Show", "PhotoUpload", new { id = comment.PhotoId });
            }
            else
                return RedirectToAction("Show", "PhotoUpload", new { id = comment.PhotoId });
        }

        [HttpDelete]
        public ActionResult Disapprove(int Id)
        {
            Comment comment = db.Comments.Find(Id);
            if (!comment.UserId.Equals(User.Identity.GetUserId()))
            {
                if (TryUpdateModel(comment))
                {
                    comment.AcceptedOrDeclined = -1;
                    db.SaveChanges();
                }
                return RedirectToAction("Show", "PhotoUpload", new { id = comment.PhotoId });
            }
            else
                return RedirectToAction("Show", "PhotoUpload", new { id = comment.PhotoId });
        }
        
    }
}