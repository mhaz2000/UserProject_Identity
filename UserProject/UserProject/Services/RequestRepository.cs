using IdentitySample.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UserProject
{
    public class RequestRepository : IRequestRepository
    {
        ApplicationDbContext DB = new ApplicationDbContext();

        /// <summary>
        /// Add new request
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public bool AddRequest(Request request, string userID)
        {
            try
            {
                request.UserID = userID;
                DB.Requests.Add(request);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Deletes a given request
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public bool DeleteRequest(Request request)
        {
            try
            {
                DB.Entry(request).State = System.Data.Entity.EntityState.Deleted;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Deletes a request by its id
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public bool DeleteRequest(object ID)
        {
            try
            {
                var Res = GetRequestByID(ID);
                DeleteRequest(Res);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Gets all requests
        /// </summary>
        /// <returns></returns>
        public List<Request> GetAllRequests()
        {
            return DB.Requests.ToList();
        }

        /// <summary>
        /// Gets a request by its id
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public Request GetRequestByID(object ID)
        {
            return DB.Requests.Find(ID);
        }

        /// <summary>
        /// Saves all changes
        /// </summary>
        public void Save()
        {
            DB.SaveChanges();
        }

        /// <summary>
        /// Updates a request by its userid
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public bool UpdateRequest(Request request, object userID)
        {
            try
            {
                request.UserID = (string)userID;
                DB.Entry(request).State = System.Data.Entity.EntityState.Modified;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void Dispose()
        {
            DB.Dispose();
        }

        /// <summary>
        /// Gets requests of a specific user
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public IEnumerable<Request> GetRequestByUserID(string ID)
        {
            return DB.Requests.Where(w => w.UserID == ID).ToList();
        }

        /// <summary>
        /// gets all requests in a specific date
        /// </summary>
        /// <param name="Date"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public List<Request> GetRequestsByDate(DateTime Date, string userid)
        {
            //var res = DB.Requests.Where(w=>w.UserID).ToList();
            return DB.Requests.Where(w => w.RequestTime.Year == Date.Year && w.RequestTime.Month == Date.Month && w.RequestTime.Day == Date.Day && w.UserID == userid).ToList();
        }

        /// <summary>
        /// Gets date by requestid
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public DateTime GetDateByID(Guid ID)
        {
            var res = DB.Requests.Where(w => w.RequestID == ID).FirstOrDefault();
            return res.RequestTime;
        }
        /// <summary>
        /// Get names of all users.
        /// </summary>
        /// <returns></returns>
        public List<string> GetallUsersNames()
        {
            var userStore = new UserStore<ApplicationUser>(DB);
            List<string> Names = new List<string>();
            
            foreach (var user in userStore.Users)
            {
                if (user.Email == "admin@example.com")
                    continue;

                Names.Add(user.Name);
            }
            return Names;
        }

        /// <summary>
        /// Get request by its name and state.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public List<Request> GetRequestsByNameAndState(string name, string state)
        {
            if (state == "همه")
                state = string.Empty;

            if (string.IsNullOrEmpty(state) && string.IsNullOrEmpty(name))
                return GetAllRequests();
            else if (!string.IsNullOrEmpty(name) && string.IsNullOrEmpty(state))
                return DB.Requests.Where(w => w.User.Name == name).ToList();
            else if (!string.IsNullOrEmpty(state) && string.IsNullOrEmpty(name))
                return DB.Requests.Where(w => w.State == state).ToList();
            else
                return DB.Requests.Where(w => w.State == state && w.User.Name == name).ToList();
        }

        /// <summary>
        /// Get all request between two specific date
        /// </summary>
        /// <param name="time1"></param>
        /// <param name="time2"></param>
        /// <returns></returns>
        public List<Request> GetRequestsByDatePeriod(DateTime time1, DateTime time2,string userid)
        {
            return DB.Requests.Where(w => (w.UserID == userid) && (w.RequestTime.Year >= time1.Year && w.RequestTime.Month >= time1.Month && w.RequestTime.Day >= time1.Day)
            && (w.RequestTime.Year <= time2.Year && w.RequestTime.Month <= time2.Month && w.RequestTime.Day <= time2.Day)).ToList();
        }
    }
}