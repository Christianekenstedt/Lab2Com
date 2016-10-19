using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Lab2Community.Models.View
{
    public class ShortMessageViewModel
    {
        public int MessageId { get; set; }
        [Display(Name = "From")]
        public string Sender { get; set; }
        [Display(Name = "Time")]
        public DateTime Timestamp { get; set; }
        [Display(Name = "Title")]
        public string Title { get; set; }
        [Display(Name = "Read")]
        public bool Read { get; set; }

    }

    public class CreateMessageViewModel
    {
        [Required]
        [Display(Name = "Title")]
        public string Title { get; set; }
        [Display(Name = "Message content")]
        [DataType(DataType.MultilineText)]
        public string Text { get; set; }
        /*[Required]
        [Display(Name = "Recipient")]
        public ICollection<System.Web.Mvc.SelectListItem> Recievers { get; set; }
        */
    }
}