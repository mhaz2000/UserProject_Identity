using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserProject
{
    interface IRequestRepository : IDisposable
    {
        List<Request> GetAllRequests();
        List<Request> GetRequestsByDate(DateTime Date, string userid);
        Request GetRequestByID(object ID);
        IEnumerable<Request> GetRequestByUserID(string ID);
        DateTime GetDateByID(Guid ID);
        bool AddRequest(Request request, string userID);
        bool UpdateRequest(Request request, object userID);
        bool DeleteRequest(Request request);
        bool DeleteRequest(object ID);
        List<string> GetallUsersNames();
        List<Request> GetRequestsByName(string name);
        void Save();
    }
}
