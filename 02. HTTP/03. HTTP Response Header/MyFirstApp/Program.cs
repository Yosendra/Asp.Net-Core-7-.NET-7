// HTTP Response Headers
// Date             : Date & Time of the response. i.e. Tue, 15 Nov 1994 08:12:31 GMT
// Server           : Name of the server. i.e. Kestrel
// Content-Type     : MIME type of response body. i.e. text/plain, text/html, application/json, application/xml, etc
// Content-Length   : Length (bytes) of response body. i.e. 100
// Cache-Control    : Indicates number of seconds that the response can cached at the browser. i.e. max-age=60
// Set-Cookie       : Contains cookies to send to browser. i.e. x=10
// Location         : Contais url to redirect. i.e. http://www.example-redirect.com
// Access-Control-Allow-Origin : Used to enables CORS (Cross-Origin-Resource-Sharing). i.e. Access-Control-Allow-Origin: http:www.example.com

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.Run(async (context) =>
{
    // This is how we access response object through context object
    // Headers is the Response Header. Its type is dictionary.
    context.Response.Headers["My-Key"] = "My Value";
    context.Response.Headers["Content-Type"] = "text/html";

    await context.Response.WriteAsync("<h1>Hello</h1>");
    await context.Response.WriteAsync("<h2>World</h2>");
});

app.Run();
