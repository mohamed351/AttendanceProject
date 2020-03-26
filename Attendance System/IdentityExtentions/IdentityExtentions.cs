using Attendance_System.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace Attendance_System.IdentityExtentions
{
    public static class IdentityExtentions
    {
        public static string GetName(this IIdentity identity)
        {
            string name = "";
            if (identity.IsAuthenticated)
            {
                ApplicationDbContext context = new ApplicationDbContext();
                string userId = identity.GetUserId();
                ApplicationUser user = context.Users.SingleOrDefault(a => a.Id == userId);
                if (user != null)
                {
                    name = user.Name;
                }
            }
            return name;

        }
    }
}