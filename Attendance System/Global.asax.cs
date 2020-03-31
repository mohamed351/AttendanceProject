using Attendance_System.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Attendance_System
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            CreateDefaultRoleAndAdmin();
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        void CreateDefaultRoleAndAdmin()
        {
            var db = new ApplicationDbContext();
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            if (!roleManager.RoleExists("Security"))
            {
                IdentityRole role = new IdentityRole("Security");
                roleManager.Create(role);
            }
            if (!roleManager.RoleExists("Admin"))
            {
                IdentityRole role = new IdentityRole("Admin");
                roleManager.Create(role);
            }
            if (!roleManager.RoleExists("Student"))
            {
                IdentityRole role = new IdentityRole("Student");
                roleManager.Create(role);
            }
            var user = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            ApplicationUser u = user.Users.SingleOrDefault(a => a.UserName == "mohamed351");
            if (u == null)
            {
                ApplicationUser defaultAdmin = new ApplicationUser();
                defaultAdmin.UserName = "mohamed351";
                defaultAdmin.Name = "Mohamed Beshri Amer";
                defaultAdmin.PhoneNumber = "0201024181643";
                defaultAdmin.Email = "mohamed.perry351@gmail.com";
                defaultAdmin.DateOfBirth = new DateTime(1996, 4, 1);
                defaultAdmin.Address = "Alexandria";
               
                user.Create(defaultAdmin, "admin@123456789");
                user.AddToRole(defaultAdmin.Id, "Admin");
            }

        }
    }
}
