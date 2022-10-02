using System;
using Telerik.DataSource.Mvc.Binder.Reflection;

namespace Telerik.DataSource.Mvc.Binder
{
    /// <summary>
    /// 数据源描述器对象的反序列化器
    /// </summary>
    public class DataSourceDescriptorDeserializer
    {
        /// <summary>
        /// 分隔符
        /// </summary>
        private const string ColumnDelimiter = "~";

        /// <summary>
        /// 依照 <paramref name="descriptorFullName"/> 指定的描述器名称来反序列化描述器列表
        /// </summary>
        /// <param name="descriptorFullName">描述器的类型全称, 可选值为 <see cref="DataSourceReflectionConsts"/> 中的常量</param>
        /// <param name="from">用以反序列化的数据源</param>
        /// <returns>反序列化描述器列表</returns>
        public static dynamic Deserialize(string descriptorFullName, string from)
        {
            var list = DataSourceReflectionHelper.ConstructDescriptorList(descriptorFullName);
            if (string.IsNullOrEmpty(from))
            {
                return list;
            }
            string[] array = from.Split(ColumnDelimiter.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            foreach (string source in array)
            {
                var item = DataSourceReflectionHelper.ConstructDescriptor(descriptorFullName);
                DataSourceReflectionHelper.DeserializeDescriptor(descriptorFullName, item, source);
                list.Add(item);
            }
            return list;
        }
    }
}
