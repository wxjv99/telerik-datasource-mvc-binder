using AspNetCoreSample.Entities;
using AspNetCoreSample.Models;
using AspNetCoreSample.Services;
using Kendo.Mvc.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;
using Telerik.DataSource;

namespace AspNetCoreSample.Controllers;

public class HomeController : Controller
{
    private readonly static JsonSerializerOptions JsonSerializerOptions = new(new JsonOptions().JsonSerializerOptions)
    {
        PropertyNamingPolicy = null
    };

    private readonly ILogger<HomeController> _logger;

    private readonly DataSourceService _dataSourceService;

    public HomeController(ILogger<HomeController> logger, DataSourceService dataSourceService)
    {
        _logger = logger;
        _dataSourceService = dataSourceService;
    }

    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> DataSourceAsync([DataSourceRequest] DataSourceRequest dsRequest)
    {
        return Json(await _dataSourceService.GetDataSourceAsync(dsRequest), JsonSerializerOptions);
    }

    [HttpPut]
    public async Task<IActionResult> DataSourceAsync(DataSourceModel model)
    {
        var dsr = new DataSourceResult() { Total = 1, AggregateResults = null };
        if (ModelState.IsValid)
        {
            var entity = new DataSourceEntity(model.Id, model.Name, model.Value);
            var result = await _dataSourceService.UpdateDataSourceAsync(model.Id, entity);

            if (result == null)
            {
                return StatusCode(404);
            }

            _logger.LogInformation("Data(id={Id}) has been modified.", result.Id);

            dsr.Data = new DataSourceModel[]
            {
                new DataSourceModel(result.Id, $"{result.Name}-Modify", result.Value)
            }.AsGenericEnumerable();
        }
        else
        {
            dsr.Data = Array.Empty<DataSourceModel>().AsGenericEnumerable();
            dsr.Errors = ModelState.SerializeErrors();
        }

        return Json(dsr, JsonSerializerOptions);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
