using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

public class SessionTimeoutMiddleware
{
    private readonly RequestDelegate _next;

    public SessionTimeoutMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        // Check if request path is not the timeout page
        if (!context.Request.Path.Equals("/Timeout", StringComparison.OrdinalIgnoreCase))
        {
            // Check if session is available and not empty
            if (context.Session != null && !IsSessionEmpty(context.Session))
            {
                await _next(context);
                return;
            }
        }

        // Redirect to timeout page
        context.Response.Redirect("/Timeout");
    }

    private bool IsSessionEmpty(ISession session)
    {
        // Check if session is empty
        foreach (var key in session.Keys)
        {
            if (key != null)
            {
                return false;
            }
        }
        return true;
    }
}
