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
        [InverseProperty("MessagesSent")]
        public virtual ApplicationUser Sender { get; set; }
        [InverseProperty("MessagesReceived")]
        public virtual ICollection<ApplicationUser> RecipientUsers { get; set; }
        public virtual ICollection<UserGroup> RecipientGroups { get; set; }
        public DateTime Timestamp { get; set; }
        public bool Read { get; set; }
        public bool Deleted { get; set; }

        public Message()
        {
            RecipientUsers = new List<ApplicationUser>();
            RecipientGroups = new List<UserGroup>();
        }
    }
}