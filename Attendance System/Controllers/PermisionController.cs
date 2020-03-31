using Attendance_System.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace Attendance_System.Controllers
{
    public class PermisionController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        [Authorize(Roles = "Admin")]
        // GET: Permision
        public ActionResult Index()
        {
            ViewBag.pers = db.Permissions.ToList();
            ViewBag.users = db.Users.ToList();
            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult ApprovePermission(string perId)
        {
            int perid = Convert.ToInt32(perId);
            var permission = db.Permissions.FirstOrDefault(per => per.Id == perid);
            permission.IsApproved = true;
            permission.Admin = User.Identity.Name;
            permission.ApprovementDate = DateTime.Now;
            db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Student")]
        public ActionResult SendPermission()
        {
            string curUserId = User.Identity.GetUserId();
            ViewBag.Permissions = db.Permissions.Where(per => per.UserId == curUserId).ToList();
            return View();
        }

        [Authorize(Roles = "Student")]
        [HttpPost]
        public ActionResult SendPermission(Permission permission)
        {
            var preExistPerm = db.Permissions.FirstOrDefault(per => per.PermissionDate == permission.PermissionDate);
            if(preExistPerm == null)
            {
                permission.SendingDate = DateTime.Now;
                permission.UserId = User.Identity.GetUserId();
                permission.IsApproved = false;
                db.Permissions.Add(permission);
                db.SaveChanges();
                return RedirectToAction(nameof(SendPermission));
            }
            return Content("You have already taken permission in that day");
        }

        [Authorize(Roles = "Student")]
        public ActionResult Cancel(string id)
        {
            int perId = Convert.ToInt32(id);
            var permission = db.Permissions.FirstOrDefault(per => per.Id == perId);

            if(permission != null)
            {
                db.Permissions.Remove(permission);
                db.SaveChanges();
            }
            return RedirectToAction(nameof(SendPermission));
        }
    }
}