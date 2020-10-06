﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UserProject
{
    public class SetRequestView
    {
        /// <summary>
        /// create view table
        /// </summary>
        /// <param name="requests"></param>
        /// <returns></returns>
        public static IEnumerable<RequestView> SetRequest(IEnumerable<Request> requests)
        {
            System.Globalization.PersianCalendar pc = new System.Globalization.PersianCalendar();
            List<RequestView> RequestViews = new List<RequestView>();

            List<string> dateTimes = new List<string>();

            foreach (var v in requests)
            {
                //add dates in a list
                dateTimes.Add(v.RequestTime.Date.ToString());

                //Check to see if there is a duplicate date or not
                if (dateTimes.Count() == dateTimes.Distinct().Count())
                {
                    //create table without exit time
                    if (v.Type == "ورود")
                        RequestViews.Add(new RequestView()
                        {
                            WorkingTime = "-",
                            ID = v.RequestID,
                            Name = v.User.Name,
                            Date = pc.GetYear(v.RequestTime) + "/" + pc.GetMonth(v.RequestTime).ToString("00") + "/" + pc.GetDayOfMonth(v.RequestTime).ToString("00"),
                            ArrivalTime = pc.GetHour(v.RequestTime).ToString("00") + ":" + pc.GetMinute(v.RequestTime).ToString("00"),
                            ExitTime = "-"
                        });
                    //create table without arrival time
                    else if (v.Type == "خروج")
                        RequestViews.Add(new RequestView()
                        {
                            WorkingTime = "-",
                            ID = v.RequestID,
                            Name = v.User.Name,
                            Date = pc.GetYear(v.RequestTime) + "/" + pc.GetMonth(v.RequestTime).ToString("00") + "/" + pc.GetDayOfMonth(v.RequestTime).ToString("00"),
                            ArrivalTime = "-",
                            ExitTime = pc.GetHour(v.RequestTime).ToString("00") + ":" + pc.GetMinute(v.RequestTime).ToString("00")
                        });
                }
                else //if there is a duplicate date, sets exit or arrival time
                {
                    string date = pc.GetYear(v.RequestTime).ToString() + "/" + pc.GetMonth(v.RequestTime).ToString("00") + "/" + pc.GetDayOfMonth(v.RequestTime).ToString("00");
                    var Res = RequestViews.Where(w => w.Date == date && w.Name==v.User.Name).FirstOrDefault();
                    if (Res.ExitTime == "-")
                    {
                        Res.ExitTime = pc.GetHour(v.RequestTime).ToString("00") + ":" + pc.GetMinute(v.RequestTime).ToString("00");
                        string[] Arrival = Res.ArrivalTime.Split(':');
                        string[] Exit = Res.ExitTime.Split(':');

                        if (Convert.ToInt32(Arrival[1]) > Convert.ToInt32(Exit[1]))
                        {
                            Res.WorkingTime = (Convert.ToInt32(Exit[0]) - Convert.ToInt32(Arrival[0]) - 1).ToString("00") + ":" + (Convert.ToInt32(Exit[1]) - Convert.ToInt32(Arrival[1]) + 60).ToString("00");
                        }
                        else
                        {
                            Res.WorkingTime = (Convert.ToInt32(Exit[0]) - Convert.ToInt32(Arrival[0])).ToString("00") + ":" + (Convert.ToInt32(Exit[1]) - Convert.ToInt32(Arrival[1])).ToString("00");
                        }
                    }
                    else
                    {
                        Res.ArrivalTime = pc.GetHour(v.RequestTime).ToString("00") + ":" + pc.GetMinute(v.RequestTime).ToString("00");
                        string[] Arrival = Res.ArrivalTime.Split(':');
                        string[] Exit = Res.ExitTime.Split(':');

                        if (Convert.ToInt32(Arrival[1]) > Convert.ToInt32(Exit[1]))
                        {
                            Res.WorkingTime = (Convert.ToInt32(Exit[0]) - Convert.ToInt32(Arrival[0]) - 1).ToString("00") + ":" + (Convert.ToInt32(Exit[1]) - Convert.ToInt32(Arrival[1]) + 60).ToString("00");
                        }
                        else
                        {
                            Res.WorkingTime = (Convert.ToInt32(Exit[0]) - Convert.ToInt32(Arrival[0])).ToString("00") + ":" + (Convert.ToInt32(Exit[1]) - Convert.ToInt32(Arrival[1])).ToString("00");
                        }
                    }
                    dateTimes.Clear();
                }
            }
            return RequestViews;
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

            requests = requests.OrderBy(o=>o.RequestTime).ToList();
            return new RequestView()
            {
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