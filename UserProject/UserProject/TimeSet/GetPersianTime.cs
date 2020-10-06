using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UserProject
{
    public class GetPersianTime
    {
        /// <summary>
        /// Gets current time in persion type
        /// </summary>
        /// <returns></returns>
        public static string[] GetPersainDateTime()
        {
            string[] times = new string[2];
            System.Globalization.PersianCalendar pc = new System.Globalization.PersianCalendar();
            DateTime date = DateTime.Now;
            times[0] = pc.GetYear(date) + "/" + pc.GetMonth(date).ToString("00") + "/" + pc.GetDayOfMonth(date).ToString("00");
            times[1] = pc.GetHour(date).ToString("00") + ":" + pc.GetMinute(date).ToString("00");
            return times;
        }
    }
}
