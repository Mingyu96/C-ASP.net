using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using CA_Project.Data;
using CA_Project.Models;

namespace CA_Project.Middleware
{
    public class CheckLoginStatus
    {
        private readonly RequestDelegate next;
        private readonly SessionDictionary sessionDict;

        public CheckLoginStatus(RequestDelegate next, SessionDictionary sessiondict)
        {
            this.next = next;
            sessionDict = sessiondict;
        }

        public async Task Invoke(HttpContext context)
        {
            string sessionid = context.Request.Cookies["sessionid"];
            if(sessionid == null) //no cookies at all
            {
                context.Response.Redirect("/home/index");
                return;
            }
            else
            {
                //check if time valid and sessionid is valid
                long? time = sessionDict.CheckSessionPresence(sessionid);
                if(time == null)
                {
                    context.Response.Redirect("/home/index");
                } else
                {
                    //check time validity
                    if((time + Session.timeout) < DateTimeOffset.Now.ToUnixTimeSeconds())
                    {
                        //time is not valid
                        context.Response.Redirect("/home/logout");
                    }
                }
            }
            await next(context);
        }
    }
}
