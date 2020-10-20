using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web;

namespace UserProject
{
    public enum RequestType
    {
        [Display(Name = "ورود")]
        arrival,
        [Display(Name = "خروج")]
        exit
    }
    public enum State
    {
        [Display(Name = "تایید شده")]
        accepted,
        [Display(Name = "رد شده")]
        rejected,
        [Display(Name = "در انتظار تایید")]
        Processing,
        [Display(Name = "همه")]
        all
    }
    public static class Extensions
    {
        public static string DisplayName(this Enum value)
        {
            Type enumType = value.GetType();
            var enumValue = Enum.GetName(enumType, value);
            MemberInfo member = enumType.GetMember(enumValue)[0];

            var attrs = member.GetCustomAttributes(typeof(DisplayAttribute), false);
            var outString = ((DisplayAttribute)attrs[0]).Name;

            if (((DisplayAttribute)attrs[0]).ResourceType != null)
            {
                outString = ((DisplayAttribute)attrs[0]).GetName();
            }

            return outString;
        }
    }
}