using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UserProject
{
    public class SetRequestView
    {

        public static string SetDayOfWeek(DateTime date)
        {
            string dayOfWeek = "";

            switch (date.DayOfWeek.ToString())
            {
                case "Saturday":
                    dayOfWeek = "شنبه";
                    break;
                case "Sunday":
                    dayOfWeek = "یک شنبه";
                    break;
                case "Monday":
                    dayOfWeek = "دو شنبه";
                    break;
                case "Tuesday":
                    dayOfWeek = "سه شنبه";
                    break;
                case "Wednesday":
                    dayOfWeek = "چهار شنبه";
                    break;
                case "Thursday":
                    dayOfWeek = "پنج شنبه";
                    break;
                case "Friday":
                    dayOfWeek = "جمعه";
                    break;
            }
            return dayOfWeek;
        }



        /// <summary>
        /// create view table
        /// </summary>
        /// <param name="requests"></param>
        /// <returns></returns>
        public static List<RequestView> SetRequest(IEnumerable<Request> requests)
        {
            int hours = 0;
            int minutes = 0;

            System.Globalization.PersianCalendar pc = new System.Globalization.PersianCalendar();

            List<RequestView> RequestViews = new List<RequestView>();
            List<string> dateTimes = new List<string>();


            foreach (var v in requests)
            {
                string dayOfWeek = SetDayOfWeek(v.RequestTime);

                //add dates in a list
                dateTimes.Add(v.RequestTime.Date.ToString());

                //Check to see if there is a duplicate date or not
                if (dateTimes.Count() == dateTimes.Distinct().Count())
                {
                    //create table without exit time
                    if (v.Type == RequestType.arrival)
                    {
                        RequestViews.Add(new RequestView(v.RequestID, v.UserID, v.User.Name,
                            pc.GetHour(v.RequestTime).ToString("00") + ":" + pc.GetMinute(v.RequestTime).ToString("00"), "-",
                            pc.GetYear(v.RequestTime) + "/" + pc.GetMonth(v.RequestTime).ToString("00") + "/" + pc.GetDayOfMonth(v.RequestTime).ToString("00"),
                            "-", EnumToString.StateDiscovery(v.State), dayOfWeek, v.User.NationalCode));
                    }
                    //create table without arrival time
                    else 
                    {
                        RequestViews.Add(new RequestView(v.RequestID, v.UserID, v.User.Name, "-",
                            pc.GetHour(v.RequestTime).ToString("00") + ":" + pc.GetMinute(v.RequestTime).ToString("00"),
                            pc.GetYear(v.RequestTime) + "/" + pc.GetMonth(v.RequestTime).ToString("00") + "/" + pc.GetDayOfMonth(v.RequestTime).ToString("00"),
                            "-", EnumToString.StateDiscovery(v.State), dayOfWeek, v.User.NationalCode));
                    }
                }
                else //if there is a duplicate date, sets exit or arrival time
                {
                    string date = pc.GetYear(v.RequestTime).ToString() + "/" + pc.GetMonth(v.RequestTime).ToString("00") + "/" + pc.GetDayOfMonth(v.RequestTime).ToString("00");
                    RequestView Res = RequestViews.Where(w => w.Date == date && w.NationalCode == v.User.NationalCode).FirstOrDefault();

                    if (v.State == State.rejected)
                        Res = RequestViews.Where(w => w.Date == date && w.NationalCode == v.User.NationalCode && w.State == "رد شده").FirstOrDefault();
                    else
                        Res = RequestViews.Where(w => w.Date == date && w.NationalCode == v.User.NationalCode && w.State != "رد شده").FirstOrDefault();

                    if((Res is null && v.Type==RequestType.arrival) || (Res.State!=EnumToString.StateDiscovery(v.State) && v.Type == RequestType.arrival))
                    {
                        RequestViews.Add(new RequestView(v.RequestID, v.UserID, v.User.Name,
                           pc.GetHour(v.RequestTime).ToString("00") + ":" + pc.GetMinute(v.RequestTime).ToString("00"), "-",
                           pc.GetYear(v.RequestTime) + "/" + pc.GetMonth(v.RequestTime).ToString("00") + "/" + pc.GetDayOfMonth(v.RequestTime).ToString("00"),
                           "-", EnumToString.StateDiscovery(v.State), dayOfWeek, v.User.NationalCode));
                        continue;
                    }
                    else if((Res is null && v.Type == RequestType.exit) || (Res.State != EnumToString.StateDiscovery(v.State) && v.Type == RequestType.exit))
                    {
                        RequestViews.Add(new RequestView(v.RequestID, v.UserID, v.User.Name, "-",
                            pc.GetHour(v.RequestTime).ToString("00") + ":" + pc.GetMinute(v.RequestTime).ToString("00"),
                            pc.GetYear(v.RequestTime) + "/" + pc.GetMonth(v.RequestTime).ToString("00") + "/" + pc.GetDayOfMonth(v.RequestTime).ToString("00"),
                            "-", EnumToString.StateDiscovery(v.State), dayOfWeek, v.User.NationalCode));
                        continue;
                    }

                    if (Res.ExitTime == "-")
                        Res.ExitTime = pc.GetHour(v.RequestTime).ToString("00") + ":" + pc.GetMinute(v.RequestTime).ToString("00");
                    else
                        Res.ArrivalTime = pc.GetHour(v.RequestTime).ToString("00") + ":" + pc.GetMinute(v.RequestTime).ToString("00");

                    string[] Arrival = Res.ArrivalTime.Split(':');
                    string[] Exit = Res.ExitTime.Split(':');

                    if (Convert.ToInt32(Arrival[1]) > Convert.ToInt32(Exit[1]))
                    {
                        hours = Convert.ToInt32(Exit[0]) - Convert.ToInt32(Arrival[0]) - 1;
                        minutes = Convert.ToInt32(Exit[1]) - Convert.ToInt32(Arrival[1] + 60);

                        Res.WorkingTime = (Convert.ToInt32(Exit[0]) - Convert.ToInt32(Arrival[0]) - 1).ToString("00") + ":" + (Convert.ToInt32(Exit[1]) - Convert.ToInt32(Arrival[1]) + 60).ToString("00");
                    }
                    else
                    {
                        hours = Convert.ToInt32(Exit[0]) - Convert.ToInt32(Arrival[0]);
                        minutes = Convert.ToInt32(Exit[1]) - Convert.ToInt32(Arrival[1]);

                        Res.WorkingTime = (Convert.ToInt32(Exit[0]) - Convert.ToInt32(Arrival[0])).ToString("00") + ":" + (Convert.ToInt32(Exit[1]) - Convert.ToInt32(Arrival[1])).ToString("00");
                    }

                }
            }
            return RequestViews.OrderByDescending(o => o.Date).ToList();
        }



        /// <summary>
        /// Set a request
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static RequestView SetRequest(Request request)
        {
            System.Globalization.PersianCalendar pc = new System.Globalization.PersianCalendar();
            RequestRepository repository = new RequestRepository();


            DateTime time = request.RequestTime;
            List<Request> requests = repository.GetRequestsByDate(time, request.UserID);

            requests = requests.OrderBy(o => o.RequestTime).ToList();
            return new RequestView()
            {
                NationalCode = request.User.NationalCode,
                UserID = request.UserID,
                DayOfWeek = SetDayOfWeek(request.RequestTime),
                State = EnumToString.StateDiscovery(request.State),
                WorkingTime = "-",
                ID = request.RequestID,
                Name = request.User.Name,
                Date = pc.GetYear(requests[0].RequestTime) + "/" + pc.GetMonth(requests[0].RequestTime).ToString("00") + "/" + pc.GetDayOfMonth(requests[0].RequestTime).ToString("00"),
                ArrivalTime = pc.GetHour(requests[0].RequestTime).ToString("00") + ":" + pc.GetMinute(requests[0].RequestTime).ToString("00"),
                ExitTime = pc.GetHour(requests[1].RequestTime).ToString("00") + ":" + pc.GetMinute(requests[1].RequestTime).ToString("00")
            };
        }

    }
}