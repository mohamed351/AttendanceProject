using Attendance_System.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Attendance_System.Controllers
{
    public class PermisionController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        //[Authorize(Roles = "Admin")]
        // GET: Permision
        public ActionResult Index()
        {
            ViewBag.pers = db.Permissions.ToList();
            ViewBag.users = db.Users.ToList();
            return View();
        }

        //[Authorize(Roles = "Admin")]
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
    }
}