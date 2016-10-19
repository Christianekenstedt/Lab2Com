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
        public string Name { get; set; }
        public virtual ICollection<ApplicationUser> Members { get; set; }
        public virtual ICollection<Message> Messages { get; set; }

        public UserGroup()
        {
            Members = new List<ApplicationUser>();
            Messages = new List<Message>();
        }
    }
}