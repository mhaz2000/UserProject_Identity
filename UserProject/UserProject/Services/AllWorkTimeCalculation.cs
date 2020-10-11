using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UserProject
{
    public class AllWorkTimeCalculation
    {
        public static string AllWorkTimeSum(IEnumerable<RequestView> requests)
        {
            int hours = 0;
            int minutes = 0;
            foreach (var requ in requests)
            {
                //bug
                if (requ.State == "رد شده" || requ.State == "در انتظار تایید")
                    continue;
                string[] time = requ.WorkingTime.Split(':');
                hours += Convert.ToInt32(time[0]);
                minutes += Convert.ToInt32(time[1]);
            }
            return (hours + minutes / 60) + ":" + (minutes % 60).ToString("00");
        }
    }
}