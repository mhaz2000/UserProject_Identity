using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UserProject
{
    public class SetRequestView
    {
        //it shows day of week by given date
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

        public static List<RequestView> SetRequest(IEnumerable<Request> requests)
        {
            List<RequestView> requestViews = new List<RequestView>();

            var people = requests.Select(s => s.UserID).Distinct().ToList();//user with different ID
            foreach (var personID in people)
            {
                var dates = requests.Where(w=>w.UserID==personID).Select(s => s.RequestTime.Date).Distinct().ToList();//Different date
                foreach (var date in dates)
                    requestViews.AddRange(getRequestViews(requests.Where(w => w.RequestTime.Date == date).ToList(), date,personID));
            }
            return requestViews;
        }

        public static List<RequestView> getRequestViews(List<Request> requests, DateTime date,string personID)
        {
            System.Globalization.PersianCalendar pc = new System.Globalization.PersianCalendar();
            string CommonDate = pc.GetYear(date) + "/" + pc.GetMonth(date).ToString("00") + "/" + pc.GetDayOfMonth(date).ToString("00");

            List<RequestView> requestViews = new List<RequestView>();

            //all accepted request
            List<Request> accepted = requests.Where(w => w.State == State.accepted && w.UserID== personID).ToList();
            if (accepted.Count != 0)
                requestViews.Add(MakeRequestView(accepted, date, CommonDate));

            //all rejected request
            List<Request> rejected = requests.Where(w => w.State == State.rejected && w.UserID == personID).ToList();
            if (rejected.Count != 0)
                requestViews.Add(MakeRequestView(rejected, date, CommonDate));

            //all processing request
            List<Request> processing = requests.Where(w => w.State == State.Processing && w.UserID == personID).ToList();
            if (processing.Count != 0)
                requestViews.Add(MakeRequestView(processing, date, CommonDate));

            return requestViews;
        }

        public static RequestView MakeRequestView(List<Request> requests, DateTime date, string CommonDate)
        {
            System.Globalization.PersianCalendar pc = new System.Globalization.PersianCalendar();

            Request ArrivalRequest = requests.Where(w => w.Type == RequestType.arrival).FirstOrDefault();
            string arrival = ArrivalRequest is null ? "-" : pc.GetHour(ArrivalRequest.RequestTime).ToString("00") + ":" + pc.GetMinute(ArrivalRequest.RequestTime).ToString("00");
            Request ExitRequest = requests.Where(w => w.Type == RequestType.exit).FirstOrDefault();
            string exit = ExitRequest is null ? "-" : pc.GetHour(ExitRequest.RequestTime).ToString("00") + ":" + pc.GetMinute(ExitRequest.RequestTime).ToString("00");

            //calculate working time
            int workTimebyminute;
            string workTime;
            if (arrival != "-" && exit != "-")
            {
                workTimebyminute = (ExitRequest.RequestTime - ArrivalRequest.RequestTime).Hours * 60 + (ExitRequest.RequestTime - ArrivalRequest.RequestTime).Minutes;
                workTime = (ExitRequest.RequestTime - ArrivalRequest.RequestTime).Hours.ToString("00") + ":" + (ExitRequest.RequestTime - ArrivalRequest.RequestTime).Minutes.ToString("00");
            }
            else
            {
                workTime = "-";
                workTimebyminute = 0;
            }

            //return new requestview
            if(!(ArrivalRequest is null))
                return (new RequestView(ArrivalRequest.RequestID, ArrivalRequest.UserID, ArrivalRequest.User.Name, arrival, exit, CommonDate, workTime, workTimebyminute
                    , ArrivalRequest.State, SetDayOfWeek(date), ArrivalRequest.User.NationalCode));
            else
                return (new RequestView(ExitRequest.RequestID, ExitRequest.UserID, ExitRequest.User.Name, arrival, exit, CommonDate, workTime, workTimebyminute
                    , ExitRequest.State, SetDayOfWeek(date), ExitRequest.User.NationalCode));
        }










        ///// <summary>
        ///// create view table
        ///// </summary>
        ///// <param name="requests"></param>
        ///// <returns></returns>
        //public static List<RequestView> SetRequest(IEnumerable<Request> requests)
        //{
        //    int hours = 0;
        //    int minutes = 0;

        //    System.Globalization.PersianCalendar pc = new System.Globalization.PersianCalendar();

        //    List<RequestView> RequestViews = new List<RequestView>();
        //    List<string> dateTimes = new List<string>();


        //    foreach (var v in requests)
        //    {
        //        string dayOfWeek = SetDayOfWeek(v.RequestTime);

        //        //add dates in a list
        //        dateTimes.Add(v.RequestTime.Date.ToString());

        //        //Check to see if there is a duplicate date or not
        //        if (dateTimes.Count() == dateTimes.Distinct().Count())
        //        {
        //            //create table without exit time
        //            if (v.Type == RequestType.arrival)
        //            {
        //                RequestViews.Add(new RequestView(v.RequestID, v.UserID, v.User.Name,
        //                    pc.GetHour(v.RequestTime).ToString("00") + ":" + pc.GetMinute(v.RequestTime).ToString("00"), "-",
        //                    pc.GetYear(v.RequestTime) + "/" + pc.GetMonth(v.RequestTime).ToString("00") + "/" + pc.GetDayOfMonth(v.RequestTime).ToString("00"),
        //                    "-", EnumToString.StateDiscovery(v.State), dayOfWeek, v.User.NationalCode));
        //            }
        //            //create table without arrival time
        //            else
        //            {
        //                RequestViews.Add(new RequestView(v.RequestID, v.UserID, v.User.Name, "-",
        //                    pc.GetHour(v.RequestTime).ToString("00") + ":" + pc.GetMinute(v.RequestTime).ToString("00"),
        //                    pc.GetYear(v.RequestTime) + "/" + pc.GetMonth(v.RequestTime).ToString("00") + "/" + pc.GetDayOfMonth(v.RequestTime).ToString("00"),
        //                    "-", EnumToString.StateDiscovery(v.State), dayOfWeek, v.User.NationalCode));
        //            }
        //        }
        //        else //if there is a duplicate date, sets exit or arrival time
        //        {
        //            string date = pc.GetYear(v.RequestTime).ToString() + "/" + pc.GetMonth(v.RequestTime).ToString("00") + "/" + pc.GetDayOfMonth(v.RequestTime).ToString("00");
        //            RequestView Res = RequestViews.Where(w => w.Date == date && w.NationalCode == v.User.NationalCode).FirstOrDefault();

        //            if (v.State == State.rejected)
        //                Res = RequestViews.Where(w => w.Date == date && w.NationalCode == v.User.NationalCode && w.State == "رد شده").FirstOrDefault();
        //            else
        //                Res = RequestViews.Where(w => w.Date == date && w.NationalCode == v.User.NationalCode && w.State != "رد شده").FirstOrDefault();

        //            if ((Res is null && v.Type == RequestType.arrival) || (Res.State != EnumToString.StateDiscovery(v.State) && v.Type == RequestType.arrival))
        //            {
        //                RequestViews.Add(new RequestView(v.RequestID, v.UserID, v.User.Name,
        //                   pc.GetHour(v.RequestTime).ToString("00") + ":" + pc.GetMinute(v.RequestTime).ToString("00"), "-",
        //                   pc.GetYear(v.RequestTime) + "/" + pc.GetMonth(v.RequestTime).ToString("00") + "/" + pc.GetDayOfMonth(v.RequestTime).ToString("00"),
        //                   "-", EnumToString.StateDiscovery(v.State), dayOfWeek, v.User.NationalCode));
        //                continue;
        //            }
        //            else if ((Res is null && v.Type == RequestType.exit) || (Res.State != EnumToString.StateDiscovery(v.State) && v.Type == RequestType.exit))
        //            {
        //                RequestViews.Add(new RequestView(v.RequestID, v.UserID, v.User.Name, "-",
        //                    pc.GetHour(v.RequestTime).ToString("00") + ":" + pc.GetMinute(v.RequestTime).ToString("00"),
        //                    pc.GetYear(v.RequestTime) + "/" + pc.GetMonth(v.RequestTime).ToString("00") + "/" + pc.GetDayOfMonth(v.RequestTime).ToString("00"),
        //                    "-", EnumToString.StateDiscovery(v.State), dayOfWeek, v.User.NationalCode));
        //                continue;
        //            }

        //            if (Res.ExitTime == "-")
        //                Res.ExitTime = pc.GetHour(v.RequestTime).ToString("00") + ":" + pc.GetMinute(v.RequestTime).ToString("00");
        //            else
        //                Res.ArrivalTime = pc.GetHour(v.RequestTime).ToString("00") + ":" + pc.GetMinute(v.RequestTime).ToString("00");

        //            string[] Arrival = Res.ArrivalTime.Split(':');
        //            string[] Exit = Res.ExitTime.Split(':');

        //            if (Convert.ToInt32(Arrival[1]) > Convert.ToInt32(Exit[1]))
        //            {
        //                hours = Convert.ToInt32(Exit[0]) - Convert.ToInt32(Arrival[0]) - 1;
        //                minutes = Convert.ToInt32(Exit[1]) - Convert.ToInt32(Arrival[1] + 60);

        //                Res.WorkingTime = (Convert.ToInt32(Exit[0]) - Convert.ToInt32(Arrival[0]) - 1).ToString("00") + ":" + (Convert.ToInt32(Exit[1]) - Convert.ToInt32(Arrival[1]) + 60).ToString("00");
        //            }
        //            else
        //            {
        //                hours = Convert.ToInt32(Exit[0]) - Convert.ToInt32(Arrival[0]);
        //                minutes = Convert.ToInt32(Exit[1]) - Convert.ToInt32(Arrival[1]);

        //                Res.WorkingTime = (Convert.ToInt32(Exit[0]) - Convert.ToInt32(Arrival[0])).ToString("00") + ":" + (Convert.ToInt32(Exit[1]) - Convert.ToInt32(Arrival[1])).ToString("00");
        //            }

        //        }
        //    }
        //    return RequestViews.OrderByDescending(o => o.Date).ToList();
        //}



        ///// <summary>
        ///// Set a request
        ///// </summary>
        ///// <param name="request"></param>
        ///// <returns></returns>
        //public static RequestView SetRequest(Request request)
        //{
        //    System.Globalization.PersianCalendar pc = new System.Globalization.PersianCalendar();
        //    RequestRepository repository = new RequestRepository();


        //    DateTime time = request.RequestTime;
        //    List<Request> requests = repository.GetRequestsByDate(time, request.UserID);

        //    requests = requests.OrderBy(o => o.RequestTime).ToList();
        //    return new RequestView()
        //    {
        //        NationalCode = request.User.NationalCode,
        //        UserID = request.UserID,
        //        DayOfWeek = SetDayOfWeek(request.RequestTime),
        //        State = EnumToString.StateDiscovery(request.State),
        //        WorkingTime = "-",
        //        ID = request.RequestID,
        //        Name = request.User.Name,
        //        Date = pc.GetYear(requests[0].RequestTime) + "/" + pc.GetMonth(requests[0].RequestTime).ToString("00") + "/" + pc.GetDayOfMonth(requests[0].RequestTime).ToString("00"),
        //        ArrivalTime = pc.GetHour(requests[0].RequestTime).ToString("00") + ":" + pc.GetMinute(requests[0].RequestTime).ToString("00"),
        //        ExitTime = pc.GetHour(requests[1].RequestTime).ToString("00") + ":" + pc.GetMinute(requests[1].RequestTime).ToString("00")
        //    };
        //}

    }
}