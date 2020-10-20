using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UserProject
{
    public class AllWorkTimeCalculation
    {
        public static string AllWorkTimeSum(List<RequestView> requests)
        {
            var time=requests.Where(w=>w.State==State.accepted).Select(s => s.WorkingTimebyminute).Sum();
            return (time/ 60) + ":" + (time % 60).ToString("00");

            //int time= 0;
            //foreach (var requ in requests)
            //{
            //    //bug
            //    if (requ.State == State.rejected || requ.State == State.Processing || requ.ExitTime == "-" || requ.ArrivalTime == "-")
            //        continue;
            //    time += requ.WorkingTime;
            //}



            //return (time/ 60) + ":" + (time % 60).ToString("00");
        }
    }
}