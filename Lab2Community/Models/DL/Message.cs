using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Lab2Community.Models.DL
{
    public class Message
    {
        [Key]
        public int MessageId { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public User Sender { get; set; }
        public virtual List<User> RecipientUsers { get; set; }
        public virtual List<UserGroup> RecipientGroups { get; set; }
        public DateTime Timestamp { get; set; }
        public bool Read { get; set; }
    }
}