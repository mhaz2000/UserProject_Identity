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
        public ActionResult Index(string sortOrder, string searchString)
        {
            ViewBag.ID = User.Identity.GetUserId();
            ViewBag.CurrentDate = GetPersianTime.GetPersainDateTime()[0];
            var userid = User.Identity.GetUserId();

            var requests = repository.GetRequestByUserID(userid);
            requests = requests.OrderBy(s => s.RequestTime);

            var requestViews = SetRequestView.SetRequest(requests);

            if (!string.IsNullOrEmpty(searchString))
                requestViews = requestViews.Where(w => w.Date == searchString).ToList();


            if (sortOrder == "Date")
                requestViews = requestViews.OrderByDescending(o => o.Date).ThenBy(o => o.ArrivalTime).ToList();
            else if (sortOrder == "WorkingTime")
                requestViews = requestViews.OrderByDescending(o => o.WorkingTime).ThenBy(o => o.Name).ToList();
            else
                requestViews = requestViews.OrderBy(o => o.Name).ThenBy(o => o.Date).ToList();

            return View(requestViews);
        }

        // GET: Requests/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Request request = repository.GetRequestByID(id);
            if (request == null)
            {
                return HttpNotFound();
            }
            return View(SetRequestView.SetRequest(request));
        }

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
            //چک کردن اینکه آیا در هر روز بیش از دو ورود نزد و سفارشی کردن کاربر و اضافه کردن اسم و فامیل
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(request.Time))
                {
                    request.Time = GetPersianTime.GetPersainDateTime()[1];
                }
                if (string.IsNullOrEmpty(request.Date))
                {
                    request.Date = GetPersianTime.GetPersainDateTime()[0];
                }
                request.RequestTime = TimeSetting.SetTime(request.Time, request.Date);

                var Res = repository.GetRequestsByDate(request.RequestTime, User.Identity.GetUserId());
                foreach (var index in Res)
                {
                    if (index.Type == request.Type)
                    {
                        ViewBag.Message = "در یک تاریخ نمی توانید بیش از یک ورود یا خروج ثبت کنید!";
                        return View(request);
                    }
                }

                repository.AddRequest(request, User.Identity.GetUserId());
                repository.Save();
                return RedirectToAction("Index");
            }

            return View(request);
        }

        // GET: Requests/Edit/5
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
