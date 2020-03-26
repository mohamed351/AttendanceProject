using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Attendance_System.ViewModels
{
    public class EditUserViewModel
    {


        public string ID { get; set; }
        [Required]
        //[Remote]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]

        public DateTime DateOfBirth { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        //[Remote]
        public string UserName { get; set; }


        [Required]
        public string RoleId { get; set; }

        public int? DepartmentId { get; set; }

    }
}