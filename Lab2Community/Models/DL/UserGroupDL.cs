using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Lab2Community.Models.DL
{
    public class UserGroupDL
    {
        [Key]
        public int GroupId { get; set; }
        public string Name { get; set; }
        public virtual List<UserDL> Members { get; set; }
        public virtual List<MessageDL> Messages { get; set; }
    }
}