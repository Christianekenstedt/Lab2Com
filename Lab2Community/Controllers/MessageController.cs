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
                    string responseMsg = "Message was sent to ";
                    foreach (string id in model.SelectedRecieverId)
                    {
                        ApplicationUser usr = db.Users.Find(id);
                        recipientUsers.Add(usr);
                        responseMsg += usr.UserName + ", ";
                    }
                    DateTime timestamp= DateTime.Now;
                    db.Messages.Add(new Message { Title = model.Title, Text = model.Text , Sender = sender, Timestamp = timestamp, Read = false, RecipientGroups = new List<UserGroup>(), RecipientUsers = recipientUsers });
                    db.SaveChanges();
                    

                    responseMsg += " at " + timestamp.ToString();

                    List<RecieverViewModel> recieverList = new List<RecieverViewModel>();
                    foreach (ApplicationUser au in db.Users.ToList())
                    {
                        recieverList.Add(new RecieverViewModel { RecieverId = au.Id, UserName = au.UserName });
                    }
                    return View("Create", new CreateMessageViewModel { Recievers = new MultiSelectList(recieverList, "RecieverId", "UserName"), Response = responseMsg, SelectedRecieverId = { }, Text = null, Title = null });
                }
            }
            // Something went wrong.
            return View(model);
        }

        [HttpGet]
        public ActionResult Details(int? id)
        {
            if (id == null) return RedirectToAction("List", "Message");

            using (var db= new ApplicationDbContext())
            {
                
                var message = db.Messages.Find(id);
                LongMessageViewModel model = new LongMessageViewModel {MessageId = message.MessageId, Sender = message.Sender.UserName, Text = message.Text, Timestamp = message.Timestamp, Title = message.Title };
                return View(model);
            }
 
        }

        [HttpGet]
        public ActionResult Delete(int? id)
        {

            if (id == null) return RedirectToAction("List", "Messsage");

            using (var db = new ApplicationDbContext())
            {
                var user_id = User.Identity.GetUserId();
                var msg = db.Messages.First(m => m.MessageId == id);
                db.Users.FirstOrDefault(u => u.Id.Equals(user_id)).MessagesReceived.Remove(msg);
                db.SaveChanges();
            }

            return RedirectToAction("List", "Message");
        }

        [HttpGet]
        public ActionResult OpenMessage(int messageId)
        {
            using (var db = new ApplicationDbContext())
            {
                var message = db.Messages.FirstOrDefault(m => m.MessageId == messageId);
                var model = new TextMessageViewModel { Text = message.Text };
                message.Read = true;
                db.SaveChanges();
                return Json(model, JsonRequestBehavior.AllowGet);
            }
        }
    }
}