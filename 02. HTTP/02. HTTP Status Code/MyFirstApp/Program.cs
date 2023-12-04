//  1xx : Informational     101 Switching Protocols (i.e. HTTP to HTTPS)
//
//  2xx : Success           200 OK
//
//  3xx : Redirection       302 Found               (i.e. /view-courses redirected to /courses/view)
//                          304 Not Modified        (If the file is already in the browser cache, the response status will be this)
//
//  4xx : Client error      400 Bad Request         (Indicates some of the data that is sent as a part of the request is incorrect or some necessary information is not included in the request)
//                          401 Unauthorized        (Incorrect email, password, OTP code)
//                          404 Not Found           (Incorrect url)
//
//  5xx : Server error      500 Internal Server Error   (Server side runtime error i.e. Runtime Exception)

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.Run(async (context) =>
{
    context.Response.StatusCode = 400;
    await context.Response.WriteAsync("Hello ");     // This is for writing the responds body
    await context.Response.WriteAsync("World");      // This is for writing the responds body
});

app.Run();
