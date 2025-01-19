# Telerik datasource mvc binder

[![NuGet](https://img.shields.io/nuget/dt/TelerikDS.Mvc.Binder.svg)](https://www.nuget.org/packages/TelerikDS.Mvc.Binder) 
[![NuGet](https://img.shields.io/nuget/vpre/TelerikDS.Mvc.Binder.svg)](https://www.nuget.org/packages/TelerikDS.Mvc.Binder)

This library provides a way to construct Telerik.DataSourceRequest from route parameters, so you don't need to reference and pass Kendo.Mvc.UI.DataSourceRequest in multiple layers of your project.


## Getting Started

### Installation Guide

**Step1** You should install [NuGet Package](https://www.nuget.org/packages/TelerikDS.Mvc.Binder)

In Visual Studio
```shell
Install-Package TelerikDS.Mvc.Binder
```
Or using .NET Core CLI
```shell
dotnet add package TelerikDS.Mvc.Binder
```

**Step2** Using namespace (you can write it in [GlobalUsings.cs](https://learn.microsoft.com/zh-cn/dotnet/csharp/language-reference/keywords/using-directive#the-global-modifier))

```csharp
using DataSourceRequestAttribute = TelerikDS.Mvc.Binder.DataSourceRequestAttribute;
```
(*Note*: In the old 1.x version, namespace start with ```Telerik.DataSource.Mvc.Binder.```)


### Usage

Mark ```[DataSourceRequest]``` attribute on your parameter
```csharp
public class HomeController : Controller
{
    // ...

    public async Task<IActionResult> GetDataSourceAsync([DataSourceRequest] DataSourceRequest dsRequest)
    {
        return Json(await _dataSourceService.GetDataSourceAsync(dsRequest));
    }
}
```
