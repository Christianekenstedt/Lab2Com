using Lab2Community.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lab2Community.Models.View;
using Lab2Community.Models.DL;
using Microsoft.AspNet.Identity;
using System.Data.Entity;

namespace Lab2Community.Controllers
{
    [Authorize]
    public class GroupsController : Controller
    {
        [HttpGet]
        // GET: Groups
        public ActionResult Index()
        {
            using (var db = new ApplicationDbContext())
            {
                var groups = db.UserGroups.ToList();

                var models = new List<ShortViewUserGroupViewModel>();
                foreach(UserGroup ug in groups)
                {
                    models.Add(new ShortViewUserGroupViewModel { GroupId = ug.GroupId, Name = ug.Name});
                }

                return View(models);
            }
            
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        // POST: Groups/Create
        public ActionResult Create(CreateUserGroupViewModel model)
        {
            if (ModelState.IsValid)
            {
                using (var db = new ApplicationDbContext())
                {
                    var group = new UserGroup { Name = model.Name };
                    group.Members.Add(db.Users.Find(User.Identity.GetUserId()));

                    db.UserGroups.Add(group);
                    db.SaveChanges();
                    return RedirectToAction("Index", "Groups");
                }
            }
            //Something went wrong
            return View("Error");
        }

        [HttpGet]
        public ActionResult Details(int? id)
        {
            using(var db = new ApplicationDbContext())
            {
                var group = db.UserGroups.Include(g => g.Members).FirstOrDefault(g => g.GroupId == id);

                if (group != null)
                {
                    var members = new List<UserViewModel>();

                    foreach(ApplicationUser member in group.Members)
                    {
                        members.Add(new UserViewModel { UserId = member.Id, Username = member.UserName });
                    }

                    var model = new UserGroupDetailsViewModel() { GroupId = group.GroupId, Members = members, Name = group.Name };
                    return View(model);
                }else
                {
                    return View("Error");
                }
                
            }
        }

        [HttpGet]
        public ActionResult Join(int id)
        {
            using (var db = new ApplicationDbContext())
            {
                var user_id = User.Identity.GetUserId();
                var user = db.Users.First(u => u.Id.Equals(user_id));
                db.UserGroups.FirstOrDefault(g => g.GroupId == id).Members.Add(user);
                db.SaveChanges();
                return RedirectToAction("Details", "Groups", new { id=id});
            }
        }

        [HttpGet]
        public ActionResult Leave(int id)
        {
            using (var db = new ApplicationDbContext())
            {
                var user_id = User.Identity.GetUserId();
                var user = db.Users.First(u => u.Id.Equals(user_id));
                db.UserGroups.FirstOrDefault(g => g.GroupId == id).Members.Remove(user);
                db.SaveChanges();
                return RedirectToAction("Details", "Groups", new { id = id });
            }
        }


    }
}