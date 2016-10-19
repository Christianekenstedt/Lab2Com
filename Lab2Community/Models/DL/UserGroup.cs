using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Lab2Community.Models.DL
{
    public class UserGroup
    {
        [Key]
        public int GroupId { get; set; }
        public string Name { get; set; }
        public virtual List<User> Members { get; set; }
        public virtual List<Message> Messages { get; set; }
    }
}