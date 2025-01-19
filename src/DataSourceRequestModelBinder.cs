using Microsoft.AspNetCore.Mvc.ModelBinding;
using TelerikDS.Mvc.Binder.Reflection;

namespace TelerikDS.Mvc.Binder;

/// <summary>
/// 构建并绑定值到 DataSourceRequest 的模型绑定器
/// </summary>
public class DataSourceRequestModelBinder : IModelBinder
{
    public virtual Task BindModelAsync(ModelBindingContext bindingContext)
    {
        dynamic model = CreateDataSourceRequest(bindingContext.ValueProvider, bindingContext.ModelName);
        bindingContext.Result = ModelBindingResult.Success(model);
        return Task.CompletedTask;
    }

    /// <summary>
    /// 使用 路由中的参数 创建 DataSourceRequest
    /// </summary>
    /// <returns>DataSourceRequest对象</returns>
    private static dynamic CreateDataSourceRequest(IValueProvider valueProvider, string modelName)
    {
        dynamic request = DataSourceReflectionHelper.ConstructDataSourceRequest();
        TryGetValue(valueProvider, modelName, DataSourceRequestParameters.Page, (int currentPage) =>
        {
            request.Page = currentPage;
        });
        TryGetValue(valueProvider, modelName, DataSourceRequestParameters.PageSize, (int pageSize) =>
        {
            request.PageSize = pageSize;
        });
        TryGetValue(valueProvider, modelName, DataSourceRequestParameters.GroupPaging, (bool groupPaging) =>
        {
            request.GroupPaging = groupPaging;
        });
        TryGetValue(valueProvider, modelName, DataSourceRequestParameters.Skip, (int skip) =>
        {
            request.Skip = skip;
        });
        TryGetValue(valueProvider, modelName, DataSourceRequestParameters.Sort, (string sort) =>
        {
            request.Sorts = DataSourceDescriptorDeserializer.Deserialize(DataSourceReflectionConsts.SortDescriptorFullName, sort);
        });
        TryGetValue(valueProvider, modelName, DataSourceRequestParameters.Filter, (string filter) =>
        {
            request.Filters = DataSourceReflectionHelper.CreateFilterDescriptor(filter);
        });
        TryGetValue(valueProvider, modelName, DataSourceRequestParameters.Group, (string group) =>
        {
            request.Groups = DataSourceDescriptorDeserializer.Deserialize(DataSourceReflectionConsts.GroupDescriptorFullName, group);
        });
        TryGetValue(valueProvider, modelName, DataSourceRequestParameters.Aggregates, (string aggregates) =>
        {
            request.Aggregates = DataSourceDescriptorDeserializer.Deserialize(DataSourceReflectionConsts.AggregateDescriptorFullName, aggregates);
        });
        return request;
    }

    /// <summary>
    /// 使用 自定义的参数 创建 DataSourceRequest
    /// </summary>
    /// <param name="page">页码</param>
    /// <param name="pageSize">页内容条数</param>
    /// <param name="sorts">排序</param>
    /// <param name="filters">过滤</param>
    /// <returns>DataSourceRequest 对象</returns>
    public static dynamic CreateDataSourceRequest(int page, int pageSize, string? sorts, string? filters) => CreateDataSourceRequest(page, pageSize, sorts, filters, null, null);

    /// <summary>
    /// 使用 自定义的参数 创建 DataSourceRequest
    /// </summary>
    /// <param name="page">页码</param>
    /// <param name="pageSize">页内容条数</param>
    /// <param name="sorts">排序</param>
    /// <param name="filters">过滤</param>
    /// <param name="groups">分组</param>
    /// <param name="aggregates">聚合</param>
    /// <returns>DataSourceRequest 对象</returns>
    public static dynamic CreateDataSourceRequest(int page, int pageSize, string? sorts, string? filters, string? groups, string? aggregates)
    {
        dynamic request = DataSourceReflectionHelper.ConstructDataSourceRequest();
        request.Page = Math.Max(page, 1);
        request.PageSize = Math.Max(pageSize, 0);
        if(!string.IsNullOrEmpty(sorts))
        {
            request.Sorts = DataSourceDescriptorDeserializer.Deserialize(DataSourceReflectionConsts.SortDescriptorFullName, sorts);
        };
        if(!string.IsNullOrEmpty(filters))
        {
            request.Filters = DataSourceReflectionHelper.CreateFilterDescriptor(filters);
        };
        if(!string.IsNullOrEmpty(groups))
        {
            request.Groups = DataSourceDescriptorDeserializer.Deserialize(DataSourceReflectionConsts.GroupDescriptorFullName, groups);
        };
        if(!string.IsNullOrEmpty(aggregates))
        {
            request.Aggregates = DataSourceDescriptorDeserializer.Deserialize(DataSourceReflectionConsts.AggregateDescriptorFullName, aggregates);
        };
        return request;
    }

    /// <summary>
    /// 尝试从路由中获取指定目标值
    /// </summary>
    /// <typeparam name="T">目标类型</typeparam>
    /// <param name="valueProvider">值提供者</param>
    /// <param name="modelName">模型名称</param>
    /// <param name="key">目标值名称</param>
    /// <param name="action">赋值方法</param>
    private static void TryGetValue<T>(IValueProvider valueProvider, string modelName, string key, Action<T> action)
    {
        if (!string.IsNullOrEmpty(modelName))
        {
            key = modelName + "-" + key;
        }
        var value = valueProvider.GetValue(key);
        if (value.FirstValue != null)
        {
            var obj = ConvertValue<T>(value.FirstValue);
            if (obj != null)
            {
                action((T)obj);
            }
        }
    }

    /// <summary>
    /// 将字符值转化为指定的目标类型的值
    /// </summary>
    /// <typeparam name="T">目标类型</typeparam>
    /// <param name="value">源字符串</param>
    /// <returns>目标值</returns>
    private static object? ConvertValue<T>(string value)
    {
        switch (Type.GetTypeCode(typeof(T)))
        {
            case TypeCode.String:
                return value;
            case TypeCode.Int32:
                {
                    if (int.TryParse(value, out var intValue))
                    {
                        return intValue;
                    }
                    return null;
                }
            case TypeCode.Boolean:
                {
                    if (bool.TryParse(value, out var booleanValue))
                    {
                        return booleanValue;
                    }
                    return null;
                }
            default:
                return null;
        }
    }
}
