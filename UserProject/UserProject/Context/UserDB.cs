using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace UserProject
{
    public class UserDB:DbContext
    {
        public UserDB()
        {

        }
        public DbSet<Request> Requests { get; set; }
    }
}