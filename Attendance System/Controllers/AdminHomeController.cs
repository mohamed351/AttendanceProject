using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Attendance_System.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AdminHomeController : Controller
    {
       
        public ActionResult Index()
        {
            return View();
        }
    }
}