using Microsoft.AspNet.Identity;
using PhotoSharing.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PhotoSharing.Controllers
{
    public class UsersController : Controller
    {
        // GET: Users
        private ApplicationDbContext db = ApplicationDbContext.Create();
        public ActionResult Index()
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            return View(user);
        }

        [Authorize(Roles ="RegisteredUser,Administrator")]
        public ActionResult Edit(string id)
        {
            ApplicationUser user = db.Users.Find(id);
            return View(user);
        }

        [HttpPut]
        public ActionResult Edit(string id, ApplicationUser newData)
        {
            ApplicationUser user = db.Users.Find(id);

            try
            {
                if(TryUpdateModel(user))
                {
                    user.UserName = newData.UserName;
                    user.Email = newData.Email;
                    user.PhoneNumber = newData.PhoneNumber;
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch(Exception e)
            {
                return View(user);
            }
        }

    }
  
}