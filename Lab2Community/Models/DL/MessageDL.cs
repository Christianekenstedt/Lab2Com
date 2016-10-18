using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Lab2Community.Models.DL
{
    public class MessageDL
    {
        [Key]
        public int MessageId { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public UserDL Sender { get; set; }
        public virtual List<UserDL> RecipientUsers { get; set; }
        public virtual List<UserGroupDL> RecipientGroups { get; set; }
        public DateTime Timestamp { get; set; }
        public bool Read { get; set; }
    }
}