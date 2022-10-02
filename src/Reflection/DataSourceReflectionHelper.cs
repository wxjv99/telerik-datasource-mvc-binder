using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

namespace Telerik.DataSource.Mvc.Binder.Reflection
{
    /// <summary>
    /// 使用反射调用DataSource相关方法的辅助器
    /// </summary>
    public static class DataSourceReflectionHelper
    {
        private static MethodInfo ModelBindingHelperConvertToMethod { get; }

        private static ConstructorInfo DataSourceRequestConstructor { get; }

        private static MethodInfo FilterDescriptorFactoryCreateMethod { get; }

        private static ConstructorInfo SortDescriptorConstructor { get; }
        private static ConstructorInfo SortDescriptorListConstructor { get; }
        private static MethodInfo SortDescriptorDeserializeMethod { get; }

        private static ConstructorInfo GroupDescriptorConstructor { get; }
        private static ConstructorInfo GroupDescriptorListConstructor { get; }
        private static MethodInfo GroupDescriptorDeserializeMethod { get; }

        private static ConstructorInfo AggregateDescriptorConstructor { get; }
        private static ConstructorInfo AggregateDescriptorListConstructor { get; }
        private static MethodInfo AggregateDescriptorDeserializeMethod { get; }

        static DataSourceReflectionHelper()
        {
            var emptyTypeList = Array.Empty<Type>();

            var modelBindingAssembly = Assembly.Load("Microsoft.AspNetCore.Mvc.Core");

            var types = new Type[] { typeof(object), typeof(Type), typeof(CultureInfo) };
            var modelBindingHelperType = modelBindingAssembly.GetType("Microsoft.AspNetCore.Mvc.ModelBinding.ModelBindingHelper");
            ModelBindingHelperConvertToMethod = modelBindingHelperType.GetMethod("ConvertTo", BindingFlags.Public | BindingFlags.Static, null, types, null);


            var dataSourceAssembly = Assembly.Load("Telerik.DataSource");

            var dataSourceRequestType = dataSourceAssembly.GetType(DataSourceReflectionConsts.DataSourceRequestFullName);
            DataSourceRequestConstructor = dataSourceRequestType.GetConstructor(emptyTypeList);

            var filterDescriptorFactoryType = dataSourceAssembly.GetType(DataSourceReflectionConsts.FilterDescriptorFactoryFullName);
            FilterDescriptorFactoryCreateMethod = filterDescriptorFactoryType.GetMethod("Create");

            var sortDescriptorType = dataSourceAssembly.GetType(DataSourceReflectionConsts.SortDescriptorFullName);
            SortDescriptorConstructor = sortDescriptorType.GetConstructor(emptyTypeList);
            SortDescriptorListConstructor = typeof(List<>).MakeGenericType(sortDescriptorType).GetConstructor(emptyTypeList);
            SortDescriptorDeserializeMethod = sortDescriptorType.GetMethod("Deserialize");

            var groupDescriptorType = dataSourceAssembly.GetType(DataSourceReflectionConsts.GroupDescriptorFullName);
            GroupDescriptorConstructor = groupDescriptorType.GetConstructor(emptyTypeList);
            GroupDescriptorListConstructor = typeof(List<>).MakeGenericType(groupDescriptorType).GetConstructor(emptyTypeList);
            GroupDescriptorDeserializeMethod = groupDescriptorType.GetMethod("Deserialize");

            var aggregateDescriptorType = dataSourceAssembly.GetType(DataSourceReflectionConsts.AggregateDescriptorFullName);
            AggregateDescriptorConstructor = aggregateDescriptorType.GetConstructor(emptyTypeList);
            AggregateDescriptorListConstructor = typeof(List<>).MakeGenericType(aggregateDescriptorType).GetConstructor(emptyTypeList);
            AggregateDescriptorDeserializeMethod = aggregateDescriptorType.GetMethod("Deserialize");
        }

        /// <summary>
        /// 构建 DataSourceRequest 对象
        /// </summary>
        /// <returns>DataSourceRequest 对象</returns>
        public static dynamic ConstructDataSourceRequest() => DataSourceRequestConstructor.Invoke(null);

        /// <summary>
        /// 使用指定的数据源创建过滤描述器
        /// </summary>
        /// <param name="source">数据源</param>
        /// <returns>FilterDescriptor 对象</returns>
        public static dynamic CreateFilterDescriptor(string source) => FilterDescriptorFactoryCreateMethod.Invoke(null, new object[] { source });

        /// <summary>
        /// 依照 <paramref name="descriptorFullName"/> 指定的描述器名称来构建描述器列表
        /// </summary>
        /// <param name="descriptorFullName">描述器的类型全称, 可选值为 <see cref="DataSourceReflectionConsts"/> 中的常量</param>
        /// <returns>Descriptor List</returns>
        public static dynamic ConstructDescriptorList(string descriptorFullName) => (descriptorFullName switch
        {
            DataSourceReflectionConsts.SortDescriptorFullName => SortDescriptorListConstructor,
            DataSourceReflectionConsts.GroupDescriptorFullName => GroupDescriptorListConstructor,
            DataSourceReflectionConsts.AggregateDescriptorFullName => AggregateDescriptorListConstructor,
            _ => throw new ArgumentException("Unsupported type", nameof(descriptorFullName))
        }).Invoke(null);

        /// <summary>
        /// 依照 <paramref name="descriptorFullName"/> 指定的描述器名称来构建描述器
        /// </summary>
        /// <param name="descriptorFullName">描述器的类型全称, 可选值为 <see cref="DataSourceReflectionConsts"/> 中的常量</param>
        /// <returns>Descriptor 对象</returns>
        public static dynamic ConstructDescriptor(string descriptorFullName) => (descriptorFullName switch
        {
            DataSourceReflectionConsts.SortDescriptorFullName => SortDescriptorConstructor,
            DataSourceReflectionConsts.GroupDescriptorFullName => GroupDescriptorConstructor,
            DataSourceReflectionConsts.AggregateDescriptorFullName => AggregateDescriptorConstructor,
            _ => throw new ArgumentException("Unsupported type", nameof(descriptorFullName))
        }).Invoke(null);

        /// <summary>
        /// 依照 <paramref name="descriptorFullName"/> 指定的描述器名称来调用描述器对象的反序列化方法
        /// </summary>
        /// <param name="descriptorFullName">描述器的类型全称, 可选值为 <see cref="DataSourceReflectionConsts"/> 中的常量</param>
        /// <param name="descriptor">是 <paramref name="descriptorFullName"/> 指定的描述器名称的描述器实例</param>
        /// <param name="source">用以反序列化的数据源</param>
        public static void DeserializeDescriptor(string descriptorFullName, dynamic descriptor, string source) => (descriptorFullName switch
        {
            DataSourceReflectionConsts.SortDescriptorFullName => SortDescriptorDeserializeMethod,
            DataSourceReflectionConsts.GroupDescriptorFullName => GroupDescriptorDeserializeMethod,
            DataSourceReflectionConsts.AggregateDescriptorFullName => AggregateDescriptorDeserializeMethod,
            _ => throw new ArgumentException("Unsupported type", nameof(descriptorFullName))
        }).Invoke(descriptor, new object[] { source });

        public static object ConvertTo(object value, Type type, CultureInfo culture)
        {
            return ModelBindingHelperConvertToMethod.Invoke(null, new object[] { value, type, culture });
        }
    }
}
