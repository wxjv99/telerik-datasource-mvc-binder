using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Telerik.DataSource.Mvc.Binder.Extensions;
using Telerik.DataSource.Mvc.Binder.Reflection;

namespace Telerik.DataSource.Mvc.Binder
{
    /// <summary>
    /// 构建并绑定值到 DataSourceRequest 的模型绑定器
    /// </summary>
    public class DataSourceRequestModelBinder : IModelBinder
    {
        public virtual Task BindModelAsync(ModelBindingContext bindingContext)
        {
            return Task.Run(() =>
            {
                dynamic model = CreateDataSourceRequest(bindingContext.ModelMetadata, bindingContext.ValueProvider, bindingContext.ModelName);
                bindingContext.Result = ModelBindingResult.Success(model);
            });
        }

        /// <summary>
        /// 使用 路由中的参数 创建 DataSourceRequest
        /// </summary>
        /// <returns>DataSourceRequest对象</returns>
        private static dynamic CreateDataSourceRequest(ModelMetadata modelMetadata, IValueProvider valueProvider, string modelName)
        {
            dynamic request = DataSourceReflectionHelper.ConstructDataSourceRequest();
            TryGetValue(modelMetadata, valueProvider, modelName, DataSourceRequestParameters.Sort, delegate(string sort)
            {
                request.Sorts = DataSourceDescriptorDeserializer.Deserialize(DataSourceReflectionConsts.SortDescriptorFullName, sort);
            });
            TryGetValue(modelMetadata, valueProvider, modelName, DataSourceRequestParameters.Page, delegate(int currentPage)
            {
                request.Page = currentPage;
            });
            TryGetValue(modelMetadata, valueProvider, modelName, DataSourceRequestParameters.PageSize, delegate(int pageSize)
            {
                request.PageSize = pageSize;
            });
            TryGetValue(modelMetadata, valueProvider, modelName, DataSourceRequestParameters.GroupPaging, delegate(bool groupPaging)
            {
                request.GroupPaging = groupPaging;
            });
            TryGetValue(modelMetadata, valueProvider, modelName, DataSourceRequestParameters.Skip, delegate(int skip)
            {
                request.Skip = skip;
            });
            TryGetValue(modelMetadata, valueProvider, modelName, DataSourceRequestParameters.Filter, delegate(string filter)
            {
                request.Filters = DataSourceReflectionHelper.CreateFilterDescriptor(filter);
            });
            TryGetValue(modelMetadata, valueProvider, modelName, DataSourceRequestParameters.Group, delegate(string group)
            {
                request.Groups = DataSourceDescriptorDeserializer.Deserialize(DataSourceReflectionConsts.GroupDescriptorFullName, group);
            });
            TryGetValue(modelMetadata, valueProvider, modelName, DataSourceRequestParameters.Aggregates, delegate(string aggregates)
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
        public static dynamic CreateDataSourceRequest(int page, int pageSize, string sorts, string filters) => CreateDataSourceRequest(page, pageSize, sorts, filters, null, null);

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
        public static dynamic CreateDataSourceRequest(int page, int pageSize, string sorts, string filters, string groups, string aggregates)
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
        /// 尝试从路由中获取指定值
        /// </summary>
        private static void TryGetValue<T>(ModelMetadata modelMetadata, IValueProvider valueProvider, string modelName, string key, Action<T> action)
        {
            if (!string.IsNullOrEmpty(modelMetadata.BinderModelName))
            {
                key = modelName + "-" + key;
            }
            ValueProviderResult value = valueProvider.GetValue(key);
            if (value.FirstValue != null)
            {
                object obj = value.ConvertValueTo(typeof(T));
                if (obj != null)
                {
                    action((T)obj);
                }
            }
        }
    }
}
