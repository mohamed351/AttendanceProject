using Attendance_System.Models;
using Attendance_System.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace Attendance_System.Controllers
{
    [Authorize(Roles = "Security")]
    public class SecurityController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();
        private UserManager<ApplicationUser> userManger;



        public SecurityController()
        {
            UserStore<ApplicationUser> userStore = new UserStore<ApplicationUser>(new ApplicationDbContext());
            userManger = new UserManager<ApplicationUser>(userStore);
        }


    
        public ActionResult Index()
        {
            var paperSheet = db.Attendance.Include(async => async.ApplicationUser)
                 .Where(a => a.Date == DateTime.Today && db.Users.Contains(a.ApplicationUser));

            if (paperSheet.Count() == 0)
            {
                foreach (var item in db.Users.ToList())
                {
                    db.Attendance.Add(new Attendance()
                    {
                        Date = DateTime.Now,
                        IsAbsent = true,
                        StudentId = item.Id

                    });
                }
                db.SaveChanges();
            }
            ViewBag.Departments = new SelectList(db.Departments.ToList(),"Id", "Name");
            return View();
        }

        [HttpGet]
        public ActionResult GetStudents (int? DeptId, int ?attendType)
        {
            if (DeptId == null && attendType == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            if (attendType == 1) {
               List<ApplicationUser> applicationUsers= db.Attendance
                    .Where(a => a.Arrival == null && a.Date == DateTime.Today)
                   .Select(a => a.ApplicationUser).Where(a=>a.DepartmentId == DeptId).ToList();

                return PartialView("_StudentListArrival", applicationUsers);
            }
            else if (attendType == 2)
            {
                List<ApplicationUser> applicationUsers = db.Attendance
                    .Where(a => a.Departure == null && a.Arrival != null)
                    .Select(a => a.ApplicationUser).Where(a => a.DepartmentId == DeptId).ToList();
                return PartialView("_StudentListDeparture", applicationUsers);
            }
           

            return Json("", JsonRequestBehavior.AllowGet);
           
           
        }

        [HttpPost]
        public ActionResult ArrivalAction(string UserID)
        {
           if(UserID == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
          Attendance attendance =  db.Attendance.SingleOrDefault(a => a.Date == DateTime.Today && a.ApplicationUser.Id == UserID);
            attendance.Arrival = DateTime.Now;
            attendance.IsAbsent = false;
            db.SaveChanges();

            return Json(new { status="ok" });
        }
        [HttpPost]
        public ActionResult DepartureAction(string userId)
        {
            Attendance attendance = db.Attendance.SingleOrDefault(a => a.Date == DateTime.Today && a.ApplicationUser.Id == userId);
            attendance.Departure = DateTime.Now;
            db.SaveChanges();

            return Json(new { status = "ok" });

        }
   
    }
}
