using Attendance_System.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Attendance_System.ViewModels
{
    public class SecurityAttendenceViewModel
    {
        public ApplicationUser Student { get; set; }
        public Department Department { get; set; }

        public List<ApplicationUser> Students { get; set; }
        public List<Department> Departments { get; set; }

        public string AttendenceType { get; set; }
        public List<string> AttendenceTypes { get; set; }
    }
}