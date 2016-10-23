using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Lab2Community.Models.DL
{
    public class UserGroup
    {
        [Key]
        public int GroupId { get; set; }
        [Required]
        [MinLength(3)]
        public string Name { get; set; }
        public virtual ICollection<ApplicationUser> Members { get; set; }
        public virtual ICollection<Message> MessagesReceived { get; set; }
        public virtual ICollection<Message> MessagesRead { get; set; }
        public virtual ICollection<Message> MessagesDeleted { get; set; }

        public UserGroup()
        {
            Members = new List<ApplicationUser>();
            MessagesReceived = new List<Message>();
            MessagesRead = new List<Message>();
            MessagesDeleted = new List<Message>();
        }
    }
}