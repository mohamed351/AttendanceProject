using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Attendance_System.Models
{
    public class Permission
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string CauseOfAbsence { get; set; }
        public DateTime PermissionDate { get; set; }
        public bool IsApproved { get; set; }
        public string Admin { get; set; }
        public DateTime SendingDate { get; set; }
        public DateTime ApprovementDate { get; set; }
        [ForeignKey("ApplicationUser")]
        public string UserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}