using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Attendance_System.Models
{
    public class Department
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string NickName { get; set; }
        // you forgot the Navigation Propertry
        public List<ApplicationUser> Users { get; set; }
    }
}