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

        // POST: Message/Create
        public ActionResult Create(CreateMessageViewModel model)
        {
            if (ModelState.IsValid)
            {
                using (var db = new ApplicationDbContext())
                {
                    var sender = db.Users.Find(User.Identity.GetUserId());
                    db.Messages.Add(new Message { Title = model.Title, Text = model.Text , Timestamp = DateTime.Now, Read = false, RecipientGroups = new List<UserGroup>(), RecipientUsers = new List<ApplicationUser>()});
                    db.SaveChanges();
                    return RedirectToAction("Index", "Message");
                }
            }
            // Something went wrong.
            return View(model);
        }
    }
}