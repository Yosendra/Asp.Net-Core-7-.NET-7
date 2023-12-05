// HTTP Request Method
// GET      : Request to retrieve information (page, entity object or a static file)
// POST     : Sends an entity object to server, generally it will be inserted into the database
// PUT      : Sends an entity object to server, generally updates all properties (full update) in the database
// PATCH    : Sends an entity object to server, generally updates few properties (patial update) in the database
// DELETE   : Request to delete an entity in the database

using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Use Postman to send POST request, in 'raw' format of request body
// Here we try fill the request body with query string eventhough it is not GET request.
//  firstName=scott&age=20
app.Run(async (context) =>
{
    // context.Request.Body is a 'Stream' object, so we use 'Stream' class to read the request body
    using StreamReader reader = new(context.Request.Body);
    string requestBody = await reader.ReadToEndAsync();

    // We use this helper class to parse the query string into dictionary object
    // There is a differrece between the string and StringValues. StringValues support multiple value.
    // If we fill the request body with 'firstName=scott&age=20&age=21', the dictionary object with 'age' key will contain list value of age, i.e. age=20,21
    Dictionary<string, StringValues> queryDict = QueryHelpers.ParseQuery(requestBody);

    if (queryDict.ContainsKey("firstName"))
    {
        string firstName = queryDict["firstName"][0];
        await context.Response.WriteAsync(firstName);
    }
});

app.Run();
