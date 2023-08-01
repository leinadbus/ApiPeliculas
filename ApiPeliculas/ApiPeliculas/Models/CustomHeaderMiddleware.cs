namespace ApiPeliculas.Models
{
    public class CustomHeaderMiddleware
    {
        private RequestDelegate _next;
        public CustomHeaderMiddleware(RequestDelegate next) {
            _next = next; 
        }

        public async Task InvokeAsync (HttpContext context)
        {
            if(context.User?.Identity?.IsAuthenticated == true)
            {
                context.Response.Headers.Add("autenticado", "Si");
            }

            if(context.Request.Method == "GET")
            {
                context.Response.Headers.Add("x-custom-daniel", "Diego");
            }

            if (context.Request.Method == "POST")
            {
                context.Response.Headers.Add("x-custom-IsJason", "True");
            }

            await _next(context);

            //if(context.Response.ContentType == "application/json; charset=utf-8")
            //{
            //    context.Response.Headers.Add("x-custom-IsJason", "True");

            //}
        }
    }
}
