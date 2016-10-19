using Lab2Community.Models;
using Lab2Community.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Data.Entity.SqlServer;
using System.Data.Entity;

namespace Lab2Community.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            using(var db = new ApplicationDbContext())
            {
                DateTime lastLogin;
                var userId = User.Identity.GetUserId();
                //var userObject = db.Users.First(p => p.Id.Equals(userId)); // maybe not useful
                var loginCountLastMonth = db.LoginRecords.Where(p => p.User.Id == userId && DbFunctions.DiffDays(p.TimeOfLogin, DateTime.Now) < 30).Count();
                var lastLoginRecord = db.LoginRecords.Where(p => p.User.Id == userId).OrderByDescending(p => p.TimeOfLogin).Skip(1).FirstOrDefault();
                var unreadMessagesDirectedToUser = db.Messages.Where(p => !p.Read && p.RecipientUsers.Where(u=>u.Id.Equals(userId)).Any()).ToList();
                var unreadMessagesDirectedToUserGroup = 
                    db.Messages.Where(p => !p.Read && p.RecipientGroups.Intersect(db.UserGroups.Where(g => g.Members.Where(u=>u.Id.Equals(userId)).Any())).Any()).ToList();
                var totalUnreadMessages = unreadMessagesDirectedToUser.Union(unreadMessagesDirectedToUserGroup).Distinct().Count();
                if (lastLoginRecord == null)
                {
                    lastLogin = DateTime.Now;
                }else
                {
                    lastLogin = lastLoginRecord.TimeOfLogin;
                }
                    
                var model = new StatisticsViewModel {LoginsLastMonth = loginCountLastMonth, LastLogin = lastLogin, UnreadMessagesCount = totalUnreadMessages};
                return View(model);
            }
            
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}