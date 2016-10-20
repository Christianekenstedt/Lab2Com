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
                return RedirectToAction("Index", "Message");
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

                return PartialView("PartialUsersMessages",models);
            }
        }

        [HttpGet]
        // GET: Message
        public ActionResult Index()
        {
            using (var db = new ApplicationDbContext())
            {
                List<RecieverViewModel> list = new List<RecieverViewModel>();
                foreach (ApplicationUser au in db.Users.ToList())
                {
                    list.Add(new RecieverViewModel { RecieverId = au.Id, UserName = au.UserName });
                }
                return View(list);
            }

        }
        [HttpGet]
        // GET: Message/Create
        public ActionResult Create()
        {
            var model = generateCreateMessageModel();
            return View(model);
        }

        private CreateMessageViewModel generateCreateMessageModel()
        {
            using (var db = new ApplicationDbContext())
            {
                List<RecieverViewModel> recieverList = new List<RecieverViewModel>();
                List<ShortViewUserGroupViewModel> groupList = new List<ShortViewUserGroupViewModel>();
                foreach (ApplicationUser au in db.Users.ToList())
                {
                    recieverList.Add(new RecieverViewModel { RecieverId = au.Id, UserName = au.UserName });
                }
                foreach (UserGroup g in db.UserGroups)
                {
                    groupList.Add(new ShortViewUserGroupViewModel { GroupId = g.GroupId, Name = g.Name });
                }

                CreateMessageViewModel model = new CreateMessageViewModel
                {
                    Recievers = new MultiSelectList(recieverList, "RecieverId", "UserName"),
                    ReceiverGroups = new MultiSelectList(groupList, "GroupId", "Name"),
                    Response = " ",
                    SelectedRecieverId = new string[recieverList.Count()],
                    SelectedGroupId = new int[groupList.Count()]
                };
                return model;
            }
        }

        [HttpPost]
        // POST: Message/Create
        public ActionResult Create(CreateMessageViewModel model)
        {
            if (ModelState.IsValid)
            {
                List<ApplicationUser> recipientUsers = new List<ApplicationUser>();
                List<UserGroup> recipientUserGroups = new List<UserGroup>();
                
                using (var db = new ApplicationDbContext())
                {
                    var sender = db.Users.Find(User.Identity.GetUserId());
                    string responseMsg = "Message was sent to ";

                    if(model.SelectedRecieverId != null)
                        recipientUsers = db.Users.Where(u => model.SelectedRecieverId.Contains(u.Id)).ToList();
                    if(model.SelectedGroupId != null)
                        recipientUserGroups = db.UserGroups.Where(g => model.SelectedGroupId.Contains(g.GroupId)).ToList();

                    foreach (var usr in recipientUsers)
                        responseMsg += usr.UserName + ", ";

                    foreach (var grp in recipientUserGroups)
                        responseMsg += grp.Name + ", ";

                    db.Messages.Add(new Message { Title = model.Title, Text = model.Text , Sender = sender, Timestamp = DateTime.Now, Read = false, RecipientGroups = recipientUserGroups, RecipientUsers = recipientUsers });
                    db.SaveChanges();

                    responseMsg += " at " + DateTime.Now;

                    var newMsgModel = generateCreateMessageModel();

                    newMsgModel.Response = responseMsg;
                    
                    return View(newMsgModel);
                }
            }
            // Something went wrong.
            return View("Error");
        }

        [HttpGet]
        public ActionResult Details(int? id)
        {
            if (id == null) return RedirectToAction("Index", "Message");

            using (var db= new ApplicationDbContext())
            {
                
                var message = db.Messages.Find(id);
                LongMessageViewModel model = new LongMessageViewModel {MessageId = message.MessageId, Sender = message.Sender.UserName, Text = message.Text, Timestamp = message.Timestamp, Title = message.Title };
                return PartialView("PartialDetails",model);
            }
 
        }

        [HttpPost]
        public ActionResult Delete(int? id)
        {

            if (id == null) return RedirectToAction("Index", "Messsage");

            using (var db = new ApplicationDbContext())
            {
                var user_id = User.Identity.GetUserId();
                var msg = db.Messages.First(m => m.MessageId == id);
                //db.Users.FirstOrDefault(u => u.Id.Equals(user_id)).MessagesReceived.Remove(msg);
                msg.Deleted = true;
                db.SaveChanges();
            }

            return View();
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