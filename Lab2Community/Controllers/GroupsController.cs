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
        /// <summary>
        /// Controller method for the index page of user groups.
        /// Displays a list with all available groups.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Get the create new group view.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Controller method to use when creating a new group
        /// </summary>
        /// <param name="model">View model that contains necessery properties of the new group.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Show details of selected group.
        /// </summary>
        /// <param name="id">The id of the group to show.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Method to use when joining a group.
        /// </summary>
        /// <param name="id">Id of the group to join</param>
        /// <returns></returns>
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

        /// <summary>
        /// Action to use when leaving a group.
        /// </summary>
        /// <param name="id">The id of the group to leave</param>
        /// <returns></returns>
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