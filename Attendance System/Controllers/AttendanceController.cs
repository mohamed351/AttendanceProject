using Attendance_System.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Attendance_System.Controllers
{
    public class AttendanceController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        [Authorize(Roles = "Admin")]
        // GET: Attendance
        public ActionResult SpecificDay()
        {
            ViewBag.Depts = db.Departments.ToList();
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult SpecificDay(string dept, string date)
        {
            // students that are exist in the specified dept and in the specified date            
            ViewBag.Depts = db.Departments.ToList();
            int deptId = Convert.ToInt32(dept);
            DateTime sentDate = Convert.ToDateTime(date);
            var stsInaDay = db.Attendance.Include("ApplicationUser").Where(at => at.Date.Equals(sentDate) && at.ApplicationUser.DepartmentId == deptId);
            return View(stsInaDay.ToList());
        }

        public ActionResult AttendanceBetweenSpecificPeriod()
        {
            ViewBag.depts = db.Departments.ToList();
            return View();
        }

        [HttpPost]
        public ActionResult AttendanceBetweenSpecificPeriod(AttendPeriodViewModel attendViewModel)
        {
            var attend = db.Attendance.Include("ApplicationUser").Where(
                at => (at.Date >= attendViewModel.FromDate && at.Date <= attendViewModel.ToDate) && at.ApplicationUser.DepartmentId == attendViewModel.DeptId
            );
            List<AttendSearchResultViewModel> all = new List<AttendSearchResultViewModel>();
            int lateCount = 0;
            int absentCount = 0;
            string status = "Good";
            var sts = db.Users.Where(user => user.DepartmentId != null);
            foreach (var st in sts.ToList())
            {
                foreach (var at in attend)
                {
                    if (st.Id == at.StudentId)
                    {
                        if (at.Arrival?.TimeOfDay > Convert.ToDateTime("03-18-2020 09:00:00.400").TimeOfDay)
                        {
                            lateCount++;
                        }
                        if (at.IsAbsent == true)
                        {
                            absentCount++;
                        }
                    }
                }
                if(attend.Count() > 0)
                {
                    if (lateCount > 5 && absentCount > 3)
                    {
                        status = "Bad";
                    }
                    AttendSearchResultViewModel asr = new AttendSearchResultViewModel
                    {
                        StudentName = st.Name,
                        NumOfAbsent = absentCount,
                        NumOfLate = lateCount,
                        Status = status
                    };
                    all.Add(asr);
                }
            }

            ViewBag.depts = db.Departments.ToList();
            return View(all);
        }        
    }
}