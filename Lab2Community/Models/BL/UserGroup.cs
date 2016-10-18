using Lab2Community.Models.DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lab2Community.Models.BL
{
    public class UserGroup
    {
        public int GroupId { get; set; }
        public string Name { get; set; }
        public virtual List<User> Members { get; set; }
        public virtual List<Message> Messages { get; set; }

        public UserGroup(UserGroupDL grpDL)
        {
            this.GroupId = grpDL.GroupId;
            this.Name = grpDL.Name;
            this.Members = User.FromDL(grpDL.Members);
            this.Messages = Message.FromDL(grpDL.Messages);
        }

        public static List<UserGroup> FromDL(List<UserGroupDL> grpDLs)
        {
            var groups = new List<UserGroup>();
            foreach(UserGroupDL ug in grpDLs)
            {
                groups.Add(new UserGroup(ug));
            }

            return groups;
        }
    }
}