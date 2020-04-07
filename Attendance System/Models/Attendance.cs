using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Attendance_System.Models
{
    public class Attendance
    {
        [Key]
        [Column(Order = 0,TypeName = "date")]
        public DateTime Date { get; set; }
        public DateTime? Arrival { get; set; }
        public DateTime? Departure { get; set; }
        public bool IsAbsent { get; set; }

        [Key]
        [Column(Order = 1)]
        [ForeignKey("ApplicationUser")]
        public string StudentId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        
    }
}