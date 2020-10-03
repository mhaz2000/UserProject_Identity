﻿using System;
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
        public Guid ID { get; set; }
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
    }
}