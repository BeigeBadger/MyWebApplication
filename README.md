# MyWebApplication

## Repo Status
#### Master
[![Build status](https://ci.appveyor.com/api/projects/status/9fa81vbe9oedxefa/branch/master?svg=true&retina=true)](https://ci.appveyor.com/project/BeigeBadger/MyWebApplication)

[![Test status](http://flauschig.ch/batch.php?type=tests&account=BeigeBadger&slug=mywebapplication)](http://flauschig.ch/batch.php?type=tests&account=BeigeBadger&slug=mywebapplication)


## Quirks/deviations
* Caught exceptions are not printed out, this is so that we hide implementation details from the user.
* `HttpPost` is used instead of `HttpPut` and `HttpDelete` as these attributes are not supported in ASP.NET MVC 5.
* Post-Redirect-Get pattern has been implemented to prevent page reloads from resubmitting the form.
* `IGamingMachine` was renamed to `IGamingRepository` as the method stubs in here made more sense to be part of an interface, rather than being methods on the `GamingMachine` class - which serves two purposes, `GamingMachine` becomes a purely data storage model without any business logic in it, and avoids a circular dependency (assuming the repository pattern was to be used) where an instance of `GamingMachine` would need to be created in order to call `CreateGamingMachine` (on another `GamingMachine` [or iteself]) which would then call out to the repo (containing the `CreateGamingMachine` method) which takes a `GamingMachine` itself.
* Code was split out from `MyCode.cs` and organised occording to ASP.NET MVC 5 common practices (ViewModels etc).
* `Get` was implemented but is never actually used because the `PagedList.MVC` NuGet package provides much more graceful handling of paging and would be used in a real world application rather than manually implementing page.
* Column sorting has been implemented on the first three columns.
* Default data  is populated automatically into the backing list.
* `TempData` was used over `ViewBag` due to it's lifecycle lasting for more than the current request.
* Probably more that I have forgotten... I have tried to document decisions within the code itself.
