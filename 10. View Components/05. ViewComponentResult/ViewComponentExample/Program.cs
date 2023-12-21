
// ViewComponentResult
//   can represent the content of a view component.
//   Generally useful to fetch view component's content into the browser
//   by making an asynchronous request (XMLHttpRequest / fetch request) from browser
//
//   asynchronous request                               Controller
//   ----------------------------------------------->     Action
//
//                    HTTP response
//              (content of View Component)
//            <------------------------------return ViewComponentResult object
//
// Look at HomeController action->LoadFriendList, index.cshtml, friends.js                                   

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseStaticFiles();
app.MapControllers();

app.Run();
