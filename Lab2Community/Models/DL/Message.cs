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
        [InverseProperty("MessagesRead")]
        public virtual ICollection<ApplicationUser> ReadByUsers { get; set; }
        [InverseProperty("MessagesDeleted")]
        public virtual ICollection<ApplicationUser> DeletedByUsers { get; set; }
        [InverseProperty("MessagesReceived")]
        public virtual ICollection<UserGroup> RecipientGroups { get; set; }
        [InverseProperty("MessagesRead")]
        public virtual ICollection<UserGroup> ReadByGroups { get; set; }
        [InverseProperty("MessagesDeleted")]
        public virtual ICollection<UserGroup> DeletedByGroups { get; set; }
        public DateTime Timestamp { get; set; }

        public Message()
        {
            RecipientUsers = new List<ApplicationUser>();
            RecipientGroups = new List<UserGroup>();
            ReadByUsers = new List<ApplicationUser>();
            DeletedByUsers = new List<ApplicationUser>();
            ReadByGroups = new List<UserGroup>();
            DeletedByGroups = new List<UserGroup>(); 
        }
    }
}