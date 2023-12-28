
// Add Country - xUnit Test
//
// DTO :                    -----> Controller / xUnit Test ----->   DTO:
//
// CountryAddRequest {                                              CountryResponse {
//   Name {get; set;}                                                 Id {get; set;}
// }                                                                  Name {get; set;}
//                                                                  }
// 
// (Doesn't contain id because the
//  id is generated in business logic,
//  not part of the request)
// 
// CountryService.cs
//   public CountryResponse AddCountry(CountryAddRequest? countryAddRequest) {
//   
//   } 
//
// TDD : Test Driven Development
//   Write unit-test first, before the implementation
//
// DTO : Data Transfer Object
//   Used for sending the request and receiving the response.
//   Here request doesn't mean the browser request, but the request
//   between the controller and service.
//   So whenever your class is being supplied as argument
//   or being received as return value between the service method and controller,
//   then it called as Data Transfer Object (DTO)
//   Generally the word request will be used for argument,
//   and the word response will be used for return type.
// 
// Look at: Entities project, Country.cs, CountryAddRequest.cs

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
app.UseStaticFiles();
app.MapControllers();
app.Run();
