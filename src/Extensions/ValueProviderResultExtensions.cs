using System;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Telerik.DataSource.Mvc.Binder.Reflection;

namespace Telerik.DataSource.Mvc.Binder.Extensions
{
    internal static class ValueProviderResultExtensions
    {
        /// <summary>
        /// 转化为指定的目标类型
        /// </summary>
        /// <param name="type">目标类型</param>
        public static object ConvertValueTo(this ValueProviderResult result, Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }
            object value = null;
            if (result.Values.Count == 1)
            {
                value = result.FirstValue;
            }
            else if (result.Values.Count > 1)
            {
                value = result.Values.ToArray();
            }
            object result2 = null;
            try
            {
                result2 = DataSourceReflectionHelper.ConvertTo(value, type, result.Culture);
                return result2;
            }
            catch
            {
                return result2;
            }
        }
    }
}
