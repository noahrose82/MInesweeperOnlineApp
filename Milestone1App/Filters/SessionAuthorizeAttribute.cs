using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Milestone1App.Filters
{
    /// <summary>
    /// A custom authorization filter that checks if a user is logged in by verifying session values. If not logged in,
    /// the user is redirected to the Login page.
    /// </summary>
    public class SessionAuthorizeAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Runs before the controller action executes. Checks for a valid UserId in session state.
        /// </summary>
        /// <param name="context">The ActionExecutingContext for the current request.</param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var userId = context.HttpContext.Session.GetInt32("UserId");

            // If userId is null, no one is logged in → force redirect to Login
            if (userId is null)
            {
                context.Result = new RedirectToActionResult("Login", "Account", null);
            }
        }
    }
}