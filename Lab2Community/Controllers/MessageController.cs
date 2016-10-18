using Lab2Community.Models.BL;
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
            var models = new List<ShortMessageViewModel>();

            foreach(Message m in Message.GetMessages())
            {
                models.Add(new ShortMessageViewModel {MessageId = m.MessageId, Sender = m.Sender.Username, Read = m.Read, Timestamp = m.Timestamp, Title = m.Title });
            }

            return View(models);
        }
    }
}