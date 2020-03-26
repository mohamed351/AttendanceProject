using Attendance_System.Models;
using Attendance_System.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace Attendance_System.Controllers
{
    [HandleError]
    public class UsersController : Controller
    {
        UserManager<ApplicationUser> users;
        RoleManager<IdentityRole> role;
        ApplicationDbContext context;
        public UsersController()
        {
            context = new ApplicationDbContext();
            users = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            role = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
           

        }
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult GetUsers(int start = 0, int length = 10)
        {
            string st = Request.QueryString["search[value]"];
            var query = users.Users.Where(a => a.Name.Contains(st)).OrderBy(a => a.Name).Skip(start)
               .Take(length).ToList();
            var count = users.Users.Where(a => a.Name.Contains(st)).Count();
            var myquery = new { data = query, recordsFiltered = count, recordsTotal = count };
            return Json(myquery, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Create()
        {

            ViewBag.RoleId = new SelectList(role.Roles.ToList(), "Name", "Name");
            return View();
        }
        public ActionResult Details(string ID)
        {
            if(ID == null)
            {
                throw new HttpException(400, "Bad Request");
            }
            var users = context.Users.Include(a => a.Roles)
            .Include(a => a.Department).FirstOrDefault(a => a.Id == ID);
            if(users == null)
            {
                throw new HttpException(404, "Not Found");
            }

            var UserRole = users.Roles.FirstOrDefault();
            if (UserRole != null)
                ViewBag.RoleName = role.Roles.FirstOrDefault(a => a.Id == UserRole.RoleId).Name;
            else
                ViewBag.RoleName = "This is User Doesn't have a role !!!";

            return View(users);

        }
       

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Create(CreateUserViewModel viewModel)
        {
            if (ModelState.IsValid)
            {

                ApplicationUser user = new ApplicationUser()
                {
                    Address = viewModel.Address,
                    DateOfBirth = viewModel.DateOfBirth,
                    Email = viewModel.Email,
                    PhoneNumber = viewModel.PhoneNumber,
                    UserName = viewModel.UserName,
                    Name = viewModel.Name,
                    DepartmentId = viewModel.DepartmentId,

                };

              IdentityResult Result =  users.Create(user, viewModel.Password);
                if (Result.Succeeded)
                {
                    users.AddToRole(user.Id, viewModel.RoleId);
                }

                return RedirectToAction("Index");

            }
            ViewBag.RoleId = new SelectList(role.Roles.ToList(), "Name", "Name");


            return View();
        }

        public ActionResult Edit(string ID)
        {
            if (ID == null)
            {
                throw new HttpException(400,"Bad Request");
            }

            ApplicationUser user = users.FindById(ID);
            if (user == null)
            {
                
                throw new HttpException(404, "User Not Found ");

            }

            
            EditUserViewModel edit = new EditUserViewModel()
            {
                Address = user.Address,
                DateOfBirth = user.DateOfBirth,
                DepartmentId = user.DepartmentId,
                Email = user.Email,
                Name = user.Name,
                PhoneNumber = user.PhoneNumber,
                UserName = user.UserName,
                ID = user.Id
            };

            InitialViewBags(ID);
            return View(edit);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditUserViewModel viewModel)
        {
            
            if (ModelState.IsValid)
            {
                ApplicationUser u = users.FindById(viewModel.ID);
                u.DateOfBirth = viewModel.DateOfBirth;
                u.DepartmentId = viewModel.DepartmentId;
                u.Email = viewModel.Email;
                u.Name = viewModel.Name;
                u.PhoneNumber = viewModel.PhoneNumber;
                u.UserName = viewModel.UserName;
                var deletedRoles = users.GetRoles(u.Id);
                users.RemoveFromRoles(u.Id, deletedRoles.ToArray());
                users.AddToRole(u.Id, viewModel.RoleId);
                context.Entry(u).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Index");

            }

            InitialViewBags(viewModel.ID);
            return View();

        }
        public void InitialViewBags(string ID)
        {
            ApplicationUser user = users.FindById(ID);
            var roleId = user.Roles.FirstOrDefault();
            var roleList = role.Roles.ToList();
            ViewBag.RoleId = new SelectList(roleList, "Name", "Name", roleList.FirstOrDefault(a => a.Id == roleId.RoleId).Name);
            ViewBag.DepartmentId = new SelectList(context.Departments.ToList(), "ID", "Name", user.DepartmentId);
        }

        [HttpGet]
        public ActionResult GetDepartment()
        {

            return PartialView("_SelectDepartment",context.Departments.ToList());
        }

        public ActionResult GetDepartmentWithID(int? ID)
        {

            ViewBag.DepartmentId = new SelectList(context.Departments.ToList(), "ID", "Name", ID);
            return PartialView("_Select_DepartmentWithID");
        }

        public ActionResult Delete(string ID)
        {
            if(ID == null)
            {
               throw new  HttpException(400,"Bad Request");
            }
          ApplicationUser user =   context.Users.SingleOrDefault(a => a.Id == ID);
            if(user == null)
            {
                throw new HttpException(404, "Not Found");

            }
            
            return View(user);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public ActionResult DeleteConfirm(string ID)
        {

            if (ID == null)
            {
                throw new HttpException(400, "Bad Request");
            }
            ApplicationUser user = context.Users.SingleOrDefault(a => a.Id == ID);
            if (user == null)
            {
                throw new HttpException(404, "Not Found");

            }
            context.Users.Remove(user);
            context.SaveChanges();
            return RedirectToAction("Index");

        }
     
      

    }
}