﻿namespace Airport.Application.Data
{
    public class DbInitializerMiddleware
    {
        private readonly RequestDelegate _next;
        public DbInitializerMiddleware(RequestDelegate next)
        {
            _next = next;

        }
        public Task Invoke(HttpContext context, IServiceProvider serviceProvider, Context dbContext)
        {

            Initializer.Initialize(dbContext);

            return _next.Invoke(context);
        }
    }
}
