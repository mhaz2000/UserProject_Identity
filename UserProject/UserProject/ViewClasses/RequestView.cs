using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace UserProject
{
    /// <summary>
    /// This class uses for showing requests
    /// </summary>
    public class RequestView
    {
        public RequestView()
        {

        }
        public RequestView(Guid ID,string UserID,string Name,string Arrivaltime,string ExitTime,string Date,string WorkingTime, int WorkingTimebyminute, State State,string DayOfWeek,string NationalCode)
        {
            this.ID = ID;
            this.UserID = UserID;
            this.Name = Name;
            this.ArrivalTime = Arrivaltime;
            this.ExitTime = ExitTime;
            this.Date = Date;
            this.WorkingTime = WorkingTime;
            this.WorkingTimebyminute = WorkingTimebyminute;
            this.State = State;
            this.DayOfWeek = DayOfWeek;
            this.NationalCode = NationalCode;
        }
        public Guid ID { get; set; }
        public string UserID { get; set; }
        [Display(Name ="نام و نام خانوادگی")]
        public string Name { get; set; }
        [Display(Name ="ساعت ورود")]
        public string ArrivalTime { get; set; }
        [Display(Name ="ساعت خروج")]
        public string ExitTime { get; set; }
        [Display(Name ="تاریخ")]
        public string Date { get; set; }
        [Display(Name ="ساعت کار")]
        public string WorkingTime { get; set; }

        public int WorkingTimebyminute { get; set; }
        [Display(Name="وضعیت درخواست")]
        public State State { get; set; }
        [Display(Name="روز هفته")]
        public string DayOfWeek { get; set; }
        [Display(Name ="کد ملی")]
        public string NationalCode { get; set; }
    }
}