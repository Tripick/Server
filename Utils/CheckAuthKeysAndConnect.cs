using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System;
using TripickServer.Controllers;
using TripickServer.Requests;

namespace TripickServer.Utils
{
    public class CheckAuthKeysAndConnect : IActionFilter
    {
        public async void OnActionExecuting(ActionExecutingContext context)
        {
            BaseController controller = context.Controller as BaseController;
            if (controller == null)
            {
                context.Result = new RedirectToRouteResult(pathToErrorAction("OnActionExecuting error, controller is not BaseController."));
                await context.Result.ExecuteResultAsync(context);
                return;
            }

            object parametersObj;
            context.ActionArguments.TryGetValue("request", out parametersObj);
            IRequest parameters = parametersObj as IRequest;
            if (parameters == null)
            {
                context.Result = new RedirectToRouteResult(pathToErrorAction("Invalid parameter."));
                await context.Result.ExecuteResultAsync(context);
                return;
            }
            if (parameters.AuthenticationKeys == null)
            {
                context.Result = new RedirectToRouteResult(pathToErrorAction("Authentication keys required."));
                await context.Result.ExecuteResultAsync(context);
                return;
            }
            try
            {
                bool authenticationKeysValid = controller.ConnectCurrentUser(parameters.AuthenticationKeys).Result;
                if (!authenticationKeysValid)
                {
                    context.Result = new RedirectToRouteResult(pathToErrorAction("Authentication keys invalid or expired."));
                    await context.Result.ExecuteResultAsync(context);
                    return;
                }
            }
            catch(Exception)
            {
                context.Result = new RedirectToRouteResult(pathToErrorAction("Authentication keys invalid or expired."));
                await context.Result.ExecuteResultAsync(context);
                return;
            }
        }

        private RouteValueDictionary pathToErrorAction(string error)
        {
            RouteValueDictionary redirectTargetDictionary = new RouteValueDictionary();
            redirectTargetDictionary.Add("area", "");
            redirectTargetDictionary.Add("controller", "Account");
            redirectTargetDictionary.Add("action", "ConnectionError");
            redirectTargetDictionary.Add("error", error);
            return redirectTargetDictionary;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
