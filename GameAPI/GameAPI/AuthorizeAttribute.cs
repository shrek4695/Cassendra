using DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameAPI
{
    public class AuthorizeAttribute : ResultFilterAttribute, IActionFilter
    {
        
        Database dbobject = new Database();
        public void OnActionExecuted(ActionExecutedContext context)
        {
            //throw new NotImplementedException();
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var apiKey = context.HttpContext.Request.Headers["apikey"].ToString();
            if (string.IsNullOrEmpty(apiKey))
            {
                throw new UnauthorizedAccessException("Api Key not passed");
            }
            else
            {
               if(!dbobject.CheckTokenValidation(apiKey))
                    throw new UnauthorizedAccessException("Invalid Api Key");
            }
        }
    }
}