using Airport.Application.Data;

namespace Airport.Application.Middleware
{
    public class DBMiddleware
    {
        private readonly RequestDelegate _next;
        public DBMiddleware(RequestDelegate next)
        {
            // инициализация базы данных 
            _next = next;
        }
        public Task Invoke(HttpContext context, Context context1)
        {
            if (!(context.Session.Keys.Contains("starting")))
            {
                Initializer.Initialize(context1);
                context.Session.SetString("starting", "Yes");
            }
            return _next.Invoke(context);
        }
    }
}
