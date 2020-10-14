using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using IdentitySample.Models;
using System.ComponentModel;

namespace UserProject
{
    public class Request
    {
        public Request()
        {
            RequestID = Guid.NewGuid();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid RequestID { get; set; }
        [Required]
        [Display(Name = "نوع درخواست")]
        public RequestType Type { get; set; }
        [Required]
        public DateTime RequestTime { get; set; }

        //public string State { get; set; }
        [Required]
        [Display(Name = "وضعیت")]
        public State State { get; set; }

        [Display(Name = "تاریخ")]
        [NotMapped]
        public string Date { get; set; }
        [Display(Name = "ساعت")]
        [NotMapped]
        public string Time { get; set; }


        //ForeignKey
        public string UserID { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
    public enum RequestType
    {
        [Display(Name = "ورود")]
        arrival,
        [Display(Name ="خروج")]
        exit
    }
    public enum State { accepted, rejected, Processing, all }

}