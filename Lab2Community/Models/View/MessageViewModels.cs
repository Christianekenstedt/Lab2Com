using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lab2Community.Models.View
{
    public class ShortMessageViewModel
    {
        public int MessageId { get; set; }
        public string Sender { get; set; }
        public DateTime Timestamp { get; set; }
        public string Title { get; set; }
        public bool Read { get; set; }

    }
}