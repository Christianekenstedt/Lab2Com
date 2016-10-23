﻿using Lab2Community.Models.DL;
using Lab2Community.Models;
using Lab2Community.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Microsoft.AspNet.Identity;

namespace Lab2Community.Controllers
{
    [Authorize]
    public class MessageController : Controller
    {

        /// <summary>
        /// Controller for the view where messages from a specific user is shown.
        /// </summary>
        /// <param name="id">The id of the user</param>
        /// <returns></returns>
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
                var userId = User.Identity.GetUserId();
                var user = db.Users.FirstOrDefault(p=>p.Id.Equals(userId));
                var messagesToUser = db.Messages.Where(
                    m => m.RecipientUsers.Any(u => u.Id.Equals(user.Id)) && 
                    !m.DeletedByUsers.Any(u=>u.Id.Equals(user.Id))&&
                    m.Sender.Id.Equals(id)).ToList();

                var messagesToUserGroup = db.Messages.Where(m => m.Sender.Id.Equals(id) &&
                m.RecipientGroups.Where(g=>g.Members.Where(u=>u.Id.Equals(userId)).Any() &&
                !m.DeletedByGroups.Contains(g)).Any()).ToList();

                var msgs = messagesToUser.Union(messagesToUserGroup).ToList();

                var messagesRead = msgs.Where(
                    m => m.ReadByUsers.Any(u => u.Id.Equals(user.Id))
                    || m.ReadByGroups.Intersect(user.Groups).Any());

                //Inte lazy?
                foreach (Message m in msgs)
                {
                    var read = false;
                    if (messagesRead.Contains(m))
                    {
                        read = true;
                    }
                    models.Add(new ShortMessageViewModel { Read = read, MessageId = m.MessageId, Sender = m.Sender.UserName, Timestamp = m.Timestamp, Title = m.Title });
                }

                return PartialView("_UsersMessages",models.OrderByDescending(m=>m.Timestamp));
            }
        }

        /// <summary>
        /// The index page when viewing messages. Initially a list of users is displayed.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        // GET: Message
        public ActionResult Index()
        {
            using (var db = new ApplicationDbContext())
            {
                List<RecieverViewModel> list = new List<RecieverViewModel>();

                var userId = User.Identity.GetUserId();
                var user = db.Users.FirstOrDefault(u => u.Id.Equals(userId));
                var messagesToUser = db.Messages.Where(m => m.RecipientUsers.Any(u => u.Id.Equals(userId)));
                var messagesToUserGroup = db.Messages.Where(m => m.RecipientGroups.Where(g => g.Members.Where(u => u.Id.Equals(userId)).Any()).Any());

                var totalMessagesToUser = messagesToUser.Union(messagesToUserGroup);

                var deletedMessages = totalMessagesToUser.Where(m => m.DeletedByUsers.Any(u=>u.Id.Equals(userId)) 
                || m.DeletedByGroups.Where(g => g.Members.Any(u => u.Id.Equals(userId))).Any());

                var readMessages = totalMessagesToUser.Where(m => m.ReadByUsers.Any(u => u.Id.Equals(userId)) ||
                m.ReadByGroups.Where(g=>g.Members.Any(u=>u.Id.Equals(userId))).Any());
  
                int numberOfReadMessages = readMessages.Count();
                int numberOfMessages = totalMessagesToUser.Count();
                int numberOfDeletedMessaged = deletedMessages.Count();

                

                foreach (ApplicationUser au in db.Users.ToList())
                {
                    list.Add(new RecieverViewModel { RecieverId = au.Id, UserName = au.UserName });
                }

                //Använd denna nya model till index-sidan för meddelanden.
                SeeSendersViewModel model = new SeeSendersViewModel
                {
                    ReadMessagesCount = numberOfReadMessages,
                    TotalMessagesCount = numberOfMessages,
                    DeletedMessagesCount = numberOfDeletedMessaged,
                    Senders = list

                };

                return View(model);
            }

        }

        /// <summary>
        /// Controller for the view where users can compose new messages.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        // GET: Message/Create
        public ActionResult Create()
        {
            var model = generateCreateMessageModel();
            return View(model);
        }

        /// <summary>
        /// A helper method to generate the viewmodels that is used when creating a new message.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Controller method for posting new messages to the system.
        /// </summary>
        /// <param name="model">The model which contains the new message properties</param>
        /// <returns></returns>
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

                    db.Messages.Add(new Message { Title = model.Title, Text = model.Text , Sender = sender, Timestamp = DateTime.Now, RecipientGroups = recipientUserGroups, RecipientUsers = recipientUsers });
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

        /// <summary>
        /// Displays details for a specific message. 
        /// </summary>
        /// <param name="id">The message to display</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Details(int? id)
        {
            if (id == null) return RedirectToAction("Index", "Message");

            using (var db = new ApplicationDbContext())
            {
                var userId = User.Identity.GetUserId();
                var user = db.Users.Find(userId);
                var message = db.Messages.Find(id);
                message.ReadByUsers.Add(user);
                message.ReadByGroups = message.ReadByGroups.Union(user.Groups).ToList();
                db.SaveChanges();
                LongMessageViewModel model = new LongMessageViewModel {MessageId = message.MessageId, Sender = message.Sender.UserName, Text = message.Text, Timestamp = message.Timestamp, Title = message.Title };
                return PartialView("_Details",model);
            }
 
        }

        /// <summary>
        /// Delete a message.
        /// </summary>
        /// <param name="id">Id of the message to delete.</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(int? id)
        {

            if (id == null) return RedirectToAction("Index", "Messsage");

            using (var db = new ApplicationDbContext())
            {
                var user_id = User.Identity.GetUserId();
                var msg = db.Messages.First(m => m.MessageId == id);
                var user = db.Users.Find(user_id);
                msg.DeletedByUsers.Add(user);
                msg.DeletedByGroups = msg.DeletedByGroups.Union(user.Groups).ToList();
                db.SaveChanges();
                return From(msg.Sender.Id);
            }

            
        }
    }
}