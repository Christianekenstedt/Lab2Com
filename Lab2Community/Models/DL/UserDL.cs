using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Lab2Community.Models.DL
{
    public class UserDL
    {
        [Key]
        public string UserId { get; set; }
        public string Username { get; set; }
        public virtual List<MessageDL> Messages { get; set; }
        public virtual List<UserGroupDL> Groups { get; set; }
        public int NumberOfLogins { get; set; }
    }
}