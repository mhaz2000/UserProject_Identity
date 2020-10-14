using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UserProject
{
    public class EnumToString
    {
        /// <summary>
        /// Discover request state by its state
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public static string StateDiscovery(State state)
        {
            switch (state)
            {
                case State.accepted:
                    return "تایید شده";
                case State.rejected:
                    return "رد شده";
                default:
                    return "در انتظار تایید";
            }
        }

        /// <summary>
        /// Discover request type by its type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string TypeDiscovery(RequestType type)
        {
            return type == RequestType.arrival ? "ورود" : "خروج";
        }
    }
}