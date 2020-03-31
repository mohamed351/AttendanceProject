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


        // GET: Security
        public ActionResult Index()
        {
            var model = new SecurityAttendenceViewModel()
            {
                Departments = db.Departments.ToList<Department>(),
                Students = db.Users.ToList<ApplicationUser>(),
                AttendenceTypes = new List<string>() { "Arrival", "Departure" }
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult _Index(int DeptId, int attendType)
        {
            var users = db.Users.ToList<ApplicationUser>();
            var studentsAttendence = db.Attendance.ToList();
            var students = new List<ApplicationUser>();
            var arrival = new List<ApplicationUser>();
            var leaving = new List<ApplicationUser>();
            var shipped = new List<ApplicationUser>();

            //creating a new list of students in a specific department
            foreach (var user in users)
            {
                if (userManger.IsInRole(user.Id, "Student") && user.DepartmentId == DeptId)
                {
                    students.Add(user);
                }
            }

            var day = db.Attendance.FirstOrDefault(s => s.Date.Year == DateTime.Now.Year
                    && s.Date.Month == DateTime.Now.Month && s.Date.Day == DateTime.Now.Day);

            //add records for students in the attendence table for the new day
            if (day == null)
            {
                foreach (var student in students)
                {
                    Attendance attendance = new Attendance();
                    attendance.Date = DateTime.Now;
                    attendance.StudentId = student.Id;
                    attendance.IsAbsent = true;

                    db.Attendance.Add(attendance);
                    db.SaveChanges();
                }
            }

            if (attendType == 1)    //Arrival list
            {
                //var stn = db.Users.Where(u => u.Department.Equals(DeptId)).ToList<ApplicationUser>();

                foreach (var student in students)
                {
                    var std = studentsAttendence.SingleOrDefault(s => s.StudentId.Equals(student.Id) && s.IsAbsent.Equals(true));
                    if (std != null)
                    {
                        arrival.Add(student);
                    }
                }
                shipped = arrival;
            }
            else if (attendType == 2)   //Departure list
            {
                foreach (var student in students)
                {
                    var std = studentsAttendence.SingleOrDefault(s => s.StudentId.Equals(student.Id) && s.IsAbsent.Equals(false));
                    if (std != null && std.Departure == null)
                    {
                        leaving.Add(student);
                    }
                }
                shipped = leaving;
            }

            var model = new SecurityAttendenceViewModel()
            {
                Departments = db.Departments.ToList<Department>(),
                Students = shipped,
                AttendenceTypes = new List<string>() { "Arrival", "Departure" }
            };
            return PartialView(model);
        }

        [HttpPost]
        public ActionResult Attendence(string stdId, int attendType)
        {
            if (ModelState.IsValid)
            {
                if (attendType == 1)    //set student arrival time
                {
                    var student = db.Attendance.Where(s => s.StudentId == stdId && s.Date.Year == DateTime.Now.Year
                    && s.Date.Month == DateTime.Now.Month && s.Date.Day == DateTime.Now.Day).FirstOrDefault();

                    if (student != null)
                    {
                        student.IsAbsent = false;
                        student.Arrival = DateTime.Now;
                        db.SaveChanges();
                    }

                }
                else if (attendType == 2)   //set student arrival Departure time
                {
                    var student = db.Attendance.Where(s => s.StudentId == stdId && s.Date.Year == DateTime.Now.Year
                    && s.Date.Month == DateTime.Now.Month && s.Date.Day == DateTime.Now.Day).FirstOrDefault();

                    if (student != null)
                    {
                        student.Departure = DateTime.Now;
                        db.Attendance.AddOrUpdate(student);
                        db.SaveChanges();
                    }
                }
            }
            return Content("done");
        }
    }
}
