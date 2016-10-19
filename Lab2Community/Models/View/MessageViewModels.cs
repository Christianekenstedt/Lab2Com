using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lab2Community.Models.View
{
    public class ViewSenderViewModel
    {
        [Display(Name = "Filter Sender")]
        public IEnumerable<SelectListItem> Senders { get; set; }
        //Borde vara en lista så vi kan ha flera.
        [Required]
        public string SelectedSenderId { get; set; }
    }
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
        [Display(Name = "Recipient(s)")]
        public IEnumerable<SelectListItem> Recievers { get; set; }
        //Borde vara en lista så vi kan ha flera.
        [Required]
        public string[] SelectedRecieverId { get; set; }

    }

    public class CreateMessageViewModel
    {
        [Required]
        [Display(Name = "Title")]
        public string Title { get; set; }
        [Display(Name = "Message content")]
        [DataType(DataType.MultilineText)]
        public string Text { get; set; }
        
        [Display(Name = "Recipient(s)")]
        public IEnumerable<SelectListItem> Recievers { get; set; }
        //Borde vara en lista så vi kan ha flera.
        [Required]
        public string[] SelectedRecieverId { get; set; }
    }

    public class RecieverViewModel
    {
        public string RecieverId { get; set; }
        [Display(Name ="Name")]
        public string UserName { get; set; }
    }
}