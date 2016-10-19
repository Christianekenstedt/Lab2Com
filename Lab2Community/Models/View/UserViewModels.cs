using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lab2Community.Models.View
{
    public class UserViewModel
    {
        public string UserId { get; set; }
        public string Username { get; set; }
    }

    public class StatisticsViewModel
    {
        public int UnreadMessagesCount { get; set; }
        public int LoginsLastMonth { get; set; }
        public DateTime LastLogin { get; set; }
    }
}