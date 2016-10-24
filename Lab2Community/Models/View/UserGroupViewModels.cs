using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [Display(Name="Name")]
        [Required]
        [MinLength(3)]
        public string Name { get; set; }
    }

    public class UserGroupDetailsViewModel
    {
        public int GroupId { get; set; }
        public string Name { get; set; }
        public IEnumerable<UserViewModel> Members { get; set; }

    }
}