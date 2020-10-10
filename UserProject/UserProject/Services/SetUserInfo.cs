using IdentitySample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UserProject
{
    /// <summary>
    /// return user info table
    /// </summary>
    public class SetUserInfo
    {
        public static List<UserInfo> UserInfoSetting(List<ApplicationUser> applicationUser)
        {
            List<UserInfo> userInfos = new List<UserInfo>();
            foreach(var user in applicationUser)
            {
                if (user.Roles.Count != 0)
                    continue;
                userInfos.Add(new UserInfo() { Name = user.Name, Email = user.Email, NationalCode = user.NationalCode,ID=user.Id });
            }
            return userInfos;
        }
    }
}