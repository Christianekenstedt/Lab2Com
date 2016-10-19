using Lab2Community.Models.DL;
using Lab2Community.Models;
using Lab2Community.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace Lab2Community.Controllers
{
    public class MessageController : Controller
    {
        [HttpGet]
        public ActionResult List()
        {
            using (var db = new ApplicationDbContext())
            {
                List<RecieverViewModel> list = new List<RecieverViewModel>();
                foreach (ApplicationUser au in db.Users.ToList())
                {
                    list.Add(new RecieverViewModel {RecieverId = au.Id, UserName = au.UserName});
                }
                return View(list);
            }
        }

        [HttpGet]
        public ActionResult From(string id)
        {
            if(id == null)
            {
                return RedirectToAction("List", "Message");
            }

            using (var db = new ApplicationDbContext())
            {
                var models = new List<ShortMessageViewModel>();
                var user = db.Users.Find(User.Identity.GetUserId());
                var messages = db.Messages.Where(m => m.RecipientUsers.Any(u => u.Id == user.Id)).ToList();
                var msgs = messages.Where(snd => snd.Sender.Id.Equals(id));

                //Inte lazy?
                foreach (Message m in msgs)
                {
                    models.Add(new ShortMessageViewModel { MessageId = m.MessageId, Sender = m.Sender.UserName, Read = m.Read, Timestamp = m.Timestamp, Title = m.Title });
                }

                return View(models);
            }
        }

        [HttpGet]
        // GET: Message
        public ActionResult Index()
        {
            using (var db = new ApplicationDbContext())
            {
                var models = new List<ShortMessageViewModel>();
                var user = db.Users.Find(User.Identity.GetUserId());
                var messages = db.Messages.Where(m => m.RecipientUsers.Any(u => u.Id == user.Id)).ToList();

                //Inte lazy?
                foreach (Message m in messages)
                {
                    models.Add(new ShortMessageViewModel { MessageId = m.MessageId, Sender = m.Sender.UserName,  Read = m.Read, Timestamp = m.Timestamp, Title = m.Title});
                }

                return View(models);
            }
           
        }
        [HttpGet]
        // GET: Message/Create
        public ActionResult Create()
        {
            using (var db = new ApplicationDbContext())
            {
                List<RecieverViewModel> recieverList = new List<RecieverViewModel>();
                foreach(ApplicationUser au in db.Users.ToList())
                {
                    recieverList.Add(new RecieverViewModel { RecieverId = au.Id, UserName = au.UserName });
                }

                CreateMessageViewModel model = new CreateMessageViewModel { Recievers = new MultiSelectList(recieverList, "RecieverId", "UserName")  };
                return View(model);
            }
        }

        [HttpPost]
        // POST: Message/Create
        public ActionResult Create(CreateMessageViewModel model)
        {
            if (ModelState.IsValid)
            {
                List<ApplicationUser> recipientUsers = new List<ApplicationUser>();
                
                using (var db = new ApplicationDbContext())
                {
                    var sender = db.Users.Find(User.Identity.GetUserId());
                    foreach(string id in model.SelectedRecieverId)
                    {
                        recipientUsers.Add(db.Users.Find(id));
                    }
                    
                    db.Messages.Add(new Message { Title = model.Title, Text = model.Text , Sender = sender, Timestamp = DateTime.Now, Read = false, RecipientGroups = new List<UserGroup>(), RecipientUsers = recipientUsers });
                    db.SaveChanges();
                    return RedirectToAction("Index", "Message");
                }
            }
            // Something went wrong.
            return View(model);
        }

        [HttpGet]
        public ActionResult Details()
        {
            return View();
        }
    }
}