using Lab2Community.Models.DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lab2Community.Models.BL
{
    public class Message
    {
        public int MessageId { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public User Sender { get; set; }
        public virtual List<User> RecipientUsers { get; set; }
        public virtual List<UserGroup> RecipientGroups { get; set; }
        public DateTime Timestamp { get; set; }
        public bool Read { get; set; }



        public Message(MessageDL msg)
        {
            MessageId = msg.MessageId;
            Read = msg.Read;
            Title = msg.Title;
            Text = msg.Text;
            Sender = new User(msg.Sender);
            RecipientUsers = User.FromDL(msg.RecipientUsers);
            RecipientGroups = UserGroup.FromDL(msg.RecipientGroups);
            Timestamp = msg.Timestamp;
            Read = msg.Read;
        }

        public static List<Message> FromDL(List<MessageDL> msgDL)
        {
            var msgs = new List<Message>();
            foreach(MessageDL msg in msgDL)
            {
                msgs.Add(new Message(msg));
            }
            return msgs;
        }

        public static List<Message> GetMessages()
        {
            var messages = new List<Message>();
            using (var db = new CommunityContext())
            {
                foreach(MessageDL msg in db.Messages.ToList())
                {
                    messages.Add(new Message(msg));
                }
                
            }
            return messages;
        }
    }
}