using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Attendance_System.Models
{
    public class AttendSearchResultViewModel
    {
        public string StudentName { get; set; }
        public int NumOfLate { get; set; }
        public int NumOfAbsent { get; set; }
        public string Status { get; set; }

    }
}