using DataAccess;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GameAPI
{
    public class LoggingAttribute:ResultFilterAttribute, IResultFilter, IExceptionFilter
    {
        Logger logobject = new Logger();
        private string request { get; set; }
        private String  response, exceptions, comment;

        public void OnException(ExceptionContext context)
        {
            request = context.RouteData.Values["action"].ToString();
            response = "Failure";
            exceptions = context.Exception.Message.ToString();
            comment = "Error Occured";
            logobject.LogMessage(request, response, exceptions, comment);
            var responseWriter = new StreamWriter(context.HttpContext.Response.Body);
            responseWriter.Write(exceptions);
        }
        public override void OnResultExecuted(ResultExecutedContext context)
        {
            request = context.RouteData.Values["action"].ToString();
            response = "Success";
            exceptions = "";
            comment = "Successful Execution";
            logobject.LogMessage(request, response, exceptions, comment);
        }
        public override void OnResultExecuting(ResultExecutingContext context)
        {
                
        }
    }
}
