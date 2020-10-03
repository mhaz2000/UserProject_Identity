using IdentitySample.Models;
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
        public IEnumerable<Request> GetAllRequests()
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
        public List<Request> GetRequestsByDate(DateTime Date,string userid)
        {
            
            return DB.Requests.Where(w => w.RequestTime.Year == Date.Year && w.RequestTime.Month == Date.Month && w.RequestTime.Day == Date.Day  && w.User.Id==userid).ToList();
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
    }
}