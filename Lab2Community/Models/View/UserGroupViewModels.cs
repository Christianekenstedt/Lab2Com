using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lab2Community.Models.View
{
    public class ShortViewUserGroupViewModel
    {

        public int GroupId { get; set; }

        public string Name { get; set; }


    }

    public class CreateUserGroupViewModel
    {
        public string Name { get; set; }
    }

    public class UserGroupDetailsViewModel
    {
        public int GroupId { get; set; }
        public string Name { get; set; }
        public IEnumerable<ApplicationUser> Members { get; set; }

    }

}