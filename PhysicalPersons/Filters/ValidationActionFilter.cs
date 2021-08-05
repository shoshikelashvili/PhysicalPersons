using Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhysicalPersons.Filters
{
    public class ValidationActionFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

        //Global filter for model validation and null checking
        public void OnActionExecuting(ActionExecutingContext context)
        {

            //Model validation not needed for Get and stuch methods
            if (context.HttpContext.Request.Method == "POST" || context.HttpContext.Request.Method == "PUT")
            {
                if (!context.ModelState.IsValid)
                {
                    context.Result = new UnprocessableEntityObjectResult(context.ModelState);
                }
            }
        }
    }
}
