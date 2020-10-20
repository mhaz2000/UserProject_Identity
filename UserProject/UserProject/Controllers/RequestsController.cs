using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IdentitySample.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using UserProject;

namespace UserProject.Controllers
{
    public class RequestsController : Controller
    {

        private IRequestRepository repository;
        private ApplicationDbContext db = new ApplicationDbContext();

        public RequestsController()
        {
            repository = new RequestRepository();
        }
        // GET: Requests
        public ActionResult Index( string date1, string date2, string ID,State? requestType)
        {
            IEnumerable<Request> requests = new List<Request>();
            //get current date in persian format
            ViewBag.CurrentDate = GetPersianTime.GetPersainDateTime()[0];
            string userid = string.Empty;

            if (ID == null)
                userid = User.Identity.GetUserId();
            else
                userid = ID;

            if (!string.IsNullOrEmpty(date1) && !string.IsNullOrEmpty(date2))
            {
                DateTime firstTime = TimeSetting.SetTime(date1);
                DateTime secondTime = TimeSetting.SetTime(date2);

                requests = repository.GetRequestsByDatePeriod(firstTime, secondTime, userid);
            }
            else
                requests = repository.GetRequestByUserID(userid);

            requests = requests.OrderBy(s => s.RequestTime);

            var requestViews = SetRequestView.SetRequest(requests);

            ViewBag.AllWorkingTime = AllWorkTimeCalculation.AllWorkTimeSum(requestViews);


            ////sorting by date
            //if (sortOrder == "Date")
            //    requestViews = requestViews.OrderByDescending(o => o.Date).ThenBy(o => o.ArrivalTime).ToList();
            //else if (sortOrder == "WorkingTime")
            //    requestViews = requestViews.OrderByDescending(o => o.WorkingTime).ThenBy(o => o.Name).ToList();
            //else
            //requestViews = requestViews.OrderBy(o => o.Name).ThenBy(o => o.Date).ToList();

            if (requestType == State.accepted)
                requestViews = requestViews.Where(w => w.State == State.accepted).ToList();
            else if (requestType == State.rejected)
                requestViews = requestViews.Where(w => w.State == State.rejected).ToList();
            else if(requestType == State.Processing)
                requestViews = requestViews.Where(w => w.State == State.Processing).ToList();
            else
                requestViews = requestViews.Where(w => w.State == State.accepted).ToList();


            return View(requestViews);
        }

        // GET: Requests/Details/5
        //public ActionResult Details(Guid? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Request request = repository.GetRequestByID(id);
        //    if (request == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(SetRequestView.SetRequest(request));
        //}

        // GET: Requests/Create
        public ActionResult Create()
        {
            ViewBag.CurrentDate = GetPersianTime.GetPersainDateTime()[0];
            ViewBag.CurrentTime = GetPersianTime.GetPersainDateTime()[1];
            return View();
        }

        // POST: Requests/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RequestID,Type,Time,Date")] Request request)
        {
            ViewBag.Message = string.Empty;


            //if (ModelState.IsValid)
            //{
            if (string.IsNullOrEmpty(request.Time))
            {
                request.Time = GetPersianTime.GetPersainDateTime()[1];
            }
            if (string.IsNullOrEmpty(request.Date))
            {
                request.Date = GetPersianTime.GetPersainDateTime()[0];
            }
            request.RequestTime = TimeSetting.SetTime(request.Time, request.Date);

            //checks if someone insert two request with same type in a particular date.
            var Res = repository.GetRequestsByDate(request.RequestTime, User.Identity.GetUserId());
            foreach (var index in Res)
            {
                if (index.Type == request.Type && index.State!=State.rejected)
                {
                    ViewBag.Message = "در یک تاریخ نمی توانید بیش از یک ورود یا خروج ثبت کنید!";
                    return View(request);
                }
            }

            request.State = State.Processing;
            repository.AddRequest(request, User.Identity.GetUserId());
            repository.Save();
            return RedirectToAction("Index");
            //}

            //return View(request);
        }

        // GET: Requests/Edit/5
        public ActionResult AllRequests(string name, State? state, string date1, string date2)
        {
            ViewBag.CurrentDate = GetPersianTime.GetPersainDateTime()[0];

            //droplist options
            var Users = repository.GetallUsersNames();
            SelectList list = new SelectList(Users);
            ViewBag.AllUsers = list;


            List<Request> requests = repository.GetRequestsByNameAndState(name, state);

            if (!string.IsNullOrEmpty(date1) && !string.IsNullOrEmpty(date2))
            {

                DateTime firstTime = TimeSetting.SetTime(date1);
                DateTime secondTime = TimeSetting.SetTime(date2);

                requests = requests.Where(w => (w.RequestTime.Year >= firstTime.Year && w.RequestTime.Month >= firstTime.Month && w.RequestTime.Day >= firstTime.Day)
              && (w.RequestTime.Year <= secondTime.Year && w.RequestTime.Month <= secondTime.Month && w.RequestTime.Day <= secondTime.Day)).ToList();
            }
            requests = requests.OrderByDescending(s => s.UserID).ThenBy(t => t.RequestTime).ToList();

            var result = SetRequestView.SetRequest(requests);
            ViewBag.AllWorkingTime = AllWorkTimeCalculation.AllWorkTimeSum(result);


            return View(result);
        }

        /// <summary>
        /// set a request state
        /// </summary>
        /// <param name="id"></param>
        /// <param name="State"></param>
        /// <returns></returns>
        public ActionResult SetState(DateTime date, State RequestState, string id)
        {
            System.Globalization.PersianCalendar pc = new System.Globalization.PersianCalendar();

            var request = repository.GetRequestsByDate(new DateTime(date.Year, date.Month, date.Day, pc), id);
            request = request.Where(w => w.State != State.rejected).ToList();

            foreach (var v in request)
            {
                v.State = RequestState;
            }
            repository.Save();
            return RedirectToAction("AllRequests");

        }
        //public ActionResult Edit(Guid? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Request request = repository.GetRequestByID(id);
        //    if (request == null)
        //    {
        //        return HttpNotFound();
        //    }

        //    return View(SetRequestView.SetRequest(request));
        //}

        //// POST: Requests/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "RequestID,Type,RequestTime,UserID")] Request request)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        repository.UpdateRequest(request, User.Identity.GetUserId());
        //        repository.Save();
        //        return RedirectToAction("Index");
        //    }
        //    return View(request);
        //}

        //// GET: Requests/Delete/5
        //public ActionResult Delete(Guid? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Request request = repository.GetRequestByID(id);
        //    if (request == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(SetRequestView.SetRequest(request));
        //}

        //// POST: Requests/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(Guid id)
        //{
        //    DateTime time = repository.GetDateByID(id);
        //    List<Request> request = repository.GetRequestsByDate(time,User.Identity.GetUserId());
        //    repository.DeleteRequest(request[0]);
        //    repository.DeleteRequest(request[1]);
        //    repository.Save();
        //    return RedirectToAction("Index");
        //}
        //dispose dbcontext
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                repository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
