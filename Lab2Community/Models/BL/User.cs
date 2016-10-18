using Lab2Community.Models.DL;
using Lab2Community.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lab2Community.Models.BL
{
    public class User
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public virtual List<Message> Messages { get; set; }
        public virtual List<UserGroup> Groups { get; set; }

        public User() { }

        public User(UserDL user)
        {
            this.UserId = user.UserId;
            this.Username = user.Username;
            this.Messages = Message.FromDL(user.Messages);
            this.Groups = UserGroup.FromDL(user.Groups);
        }

        public static List<User> FromDL(IList<UserDL> userDLs)
        {
            var users = new List<User>();

            foreach (UserDL u in userDLs)
                users.Add(new User(u));

            return users;
        }

        public static IList<User> GetUsers()
        {
            List<User> users = new List<User>();

            using(var db = new CommunityContext())
            {
                foreach(UserDL u in db.Users.ToList())
                {
                    users.Add(new User(u));
                }
            }
            return users;
        }

        public static bool AddUser(User user)
        {
            using(var db = new CommunityContext())
            {
                db.Users.Add(new UserDL { UserId = user.UserId, Username = user.Username });
                db.SaveChanges();
                return true;
            }
        }
    }

}