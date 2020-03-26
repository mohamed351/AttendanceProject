using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Attendance_System.Models
{
    public class AttendPeriodViewModel
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int DeptId { get; set; }
    }
}