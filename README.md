Game List Website Example With MVC4
========================================

An example website of an ASP.NET MVC 4 website with Autofac for DI, and bootstrap for UI.

This application uses a web service for data access, and probably won't work for those without an api key.
Nonetheless the application does work under with an web service api key, and it also demonstrates my proficiency in the ASP.NET MVC 4 framework.

I used a variety of techniques and technologies to create this application.
It uses ASP.NET MVC 4 with Autofac for dependency injection.  
Bootstrap is used to improve the look and feel of the web site.
Finally application also uses Rhino Mocks and Microsoft's test library.

Some design decisions were made to show off my skills in a variety of technologies. 
 For example, if this app was not going to be further developed, I would probably not have bothered with Dependency injection.
There were a few design decisions that I would have made differently if the app were one on a larger scale.
If this application got larger and more complex, I would create a business logic/domain library project.
Then I would refactor as much of the business logic as I could into that library.
Also likely at some point I would feel the need to add a db and an orm to the application, probably Entity Framework.
This would allow the application to have user accounts, which is probably a better way to track voting than cookies.
If the application kept increasing in features and size, a continuous integration environment could be setup, 
as well as automated integration tests.

This application should be protected from XSS and CSRF attacks. 
