using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UserProject
{
    public class TimeSetting
    {
        /// <summary>
        /// set time
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime SetTime(string date)
        {
            System.Globalization.PersianCalendar pc = new System.Globalization.PersianCalendar();
            string[] firstTime = date.Split('/');

            return(new DateTime(Convert.ToInt32(firstTime[0]), Convert.ToInt32(firstTime[1]), Convert.ToInt32(firstTime[2]), pc));
        }
        /// <summary>
        /// Set Time
        /// </summary>
        /// <param name="time"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime SetTime(string time, string date)
        {
            System.Globalization.PersianCalendar pc = new System.Globalization.PersianCalendar();

            string[] TimeSplit = time.Split(':');
            string[] DateSplit = date.Split('/');

            int hour = Convert.ToInt32(TimeSplit[0]);
            int minute = Convert.ToInt32(TimeSplit[1]);

            int year = Convert.ToInt32(DateSplit[0]);
            int month = Convert.ToInt32(DateSplit[1]);
            int day = Convert.ToInt32(DateSplit[2]);

            return new DateTime(year, month, day, hour, minute, 0, pc);
        }
        /// <summary>
        /// Set Time Working
        /// </summary>
        /// <param name="Arrival"></param>
        /// <param name="Exit"></param>
        /// <returns></returns>
        public static string SetWorkingTime(DateTime Arrival, DateTime Exit)
        {
            int hours = (Exit - Arrival).Hours;
            int minutes = (Exit - Arrival).Minutes;

            return $"{hours}:{minutes}";
        }
    }
}