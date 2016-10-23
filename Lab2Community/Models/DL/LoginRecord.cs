using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Lab2Community.Models.DL
{
    /// <summary>
    /// Data entity containing information from when users logged in to the system.
    /// </summary>
    public class LoginRecord
    {
        [Key]
        public int RecordId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual DateTime TimeOfLogin { get; set; }
    }
}