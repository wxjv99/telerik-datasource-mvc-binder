using AspNetCoreSample.Entities;
using Telerik.DataSource;
using Telerik.DataSource.Extensions;

namespace AspNetCoreSample.Services;

public class DataSourceService
{
    private readonly static Random _random = new();
    private readonly static DataSourceEntity[] _dataSource;

    static DataSourceService()
    {
        _random = new();
        _dataSource= Enumerable.Range(1, 40)
            .Select(x => new DataSourceEntity(x, $"Item{x:D2}", _random.Next(5, 20)))
            .ToArray();
    }

    public Task<DataSourceResult> GetDataSourceAsync(DataSourceRequest request)
    {
        return _dataSource.AsQueryable().ToDataSourceResultAsync(request);
    }

    public Task<DataSourceEntity?> UpdateDataSourceAsync(int id, DataSourceEntity input)
    {
        var entity = _dataSource.FirstOrDefault(p => p.Id == id);
        if (entity != null)
        {
            entity.Name = input.Name;
            entity.Value = input.Value;
        }
        return Task.FromResult(entity);
    }
}

