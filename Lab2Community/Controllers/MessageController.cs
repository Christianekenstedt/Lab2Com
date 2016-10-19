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
        // GET: Message
        public ActionResult Index()
        {
            using (var db = new ApplicationDbContext())
            {
                var models = new List<ShortMessageViewModel>();

                /*foreach(Message m in Message.GetMessages())
                {
                    models.Add(new ShortMessageViewModel {MessageId = m.MessageId, Sender = m.Sender.Username, Read = m.Read, Timestamp = m.Timestamp, Title = m.Title });
                }*/

                //Inte lazy?
                foreach (Message m in db.Messages.ToList())
                {
                    models.Add(new ShortMessageViewModel { MessageId = m.MessageId, Read = m.Read, Timestamp = m.Timestamp, Title = m.Title });
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
                /*List<SelectListItem> list = new List<SelectListItem>();
                foreach(ApplicationUser au in db.Users.ToList())
                {
                    list.Add(new SelectListItem {Value = au.Id, Text = au.UserName });
                }*/
                List<RecieverViewModel> recieverList = new List<RecieverViewModel>();
                foreach(ApplicationUser au in db.Users.ToList())
                {
                    recieverList.Add(new RecieverViewModel { RecieverId = au.Id, UserName = au.UserName });
                }

                CreateMessageViewModel model = new CreateMessageViewModel { Recievers = new SelectList(recieverList, "RecieverId", "UserName")  };
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
                    recipientUsers.Add(db.Users.Find(model.SelectedRecieverId));
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