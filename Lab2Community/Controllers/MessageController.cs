using Lab2Community.Models.DL;
using Lab2Community.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lab2Community.Controllers
{
    public class MessageController : Controller
    {
        // GET: Message
        public ActionResult Index()
        {
            var db = new CommunityContext();
            var models = new List<ShortMessageViewModel>();

            /*foreach(Message m in Message.GetMessages())
            {
                models.Add(new ShortMessageViewModel {MessageId = m.MessageId, Sender = m.Sender.Username, Read = m.Read, Timestamp = m.Timestamp, Title = m.Title });
            }*/
            
            //Inte lazy?
            foreach(Message m in db.Messages.ToList())
            {
                models.Add(new ShortMessageViewModel { MessageId = m.MessageId, Sender = m.Sender.Username, Read = m.Read, Timestamp = m.Timestamp, Title = m.Title});
            }

            return View(models);
        }

        // POST: Message/Create
        public ActionResult Create(CreateMessageViewModel model)
        {
            if (ModelState.IsValid)
            {
                var db = new CommunityContext();
                //TODO:
                db.Messages.Add(new Message { Title = model.Title, Text = model.Text });
                db.SaveChanges();
                return RedirectToAction("Index", "Message");
            }


            // Something went wrong.
            return View(model);
        }
    }
}