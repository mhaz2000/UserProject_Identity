using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web;

namespace UserProject
{
    public class DroplistOption
    {
        // Return non-empty name specified in a [Display] attribute for the given field, if any; field's name otherwise
        private static string GetDisplayName(FieldInfo field)
        {
            DisplayAttribute display = field.GetCustomAttribute<DisplayAttribute>(inherit: false);
            if (display != null)
            {
                string name = display.GetName();
                if (!String.IsNullOrEmpty(name))
                {
                    return name;
                }
            }

            return field.Name;
        }
    }
}