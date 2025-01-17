using System.Reflection;
using Telerik.DataSource.Mvc.Binder.Extensions;

namespace Telerik.DataSource.Mvc.Binder.Reflection;

/// <summary>
/// 使用反射调用DataSource相关方法的辅助器
/// </summary>
public static class DataSourceReflectionHelper
{
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
        var dataSourceAssembly = ReflectionExtensions.ForceLoadAssembly(DataSourceReflectionConsts.AssemblyName);

        var dataSourceRequestType = dataSourceAssembly.ForceGetType(DataSourceReflectionConsts.DataSourceRequestFullName);
        DataSourceRequestConstructor = dataSourceRequestType.ForceGetConstructor([]);

        var filterDescriptorFactoryType = dataSourceAssembly.ForceGetType(DataSourceReflectionConsts.FilterDescriptorFactoryFullName);
        FilterDescriptorFactoryCreateMethod = filterDescriptorFactoryType.ForceGetMethod("Create");

        var sortDescriptorType = dataSourceAssembly.ForceGetType(DataSourceReflectionConsts.SortDescriptorFullName);
        SortDescriptorConstructor = sortDescriptorType.ForceGetConstructor([]);
        SortDescriptorListConstructor = typeof(List<>).MakeGenericType(sortDescriptorType).ForceGetConstructor([]);
        SortDescriptorDeserializeMethod = sortDescriptorType.ForceGetMethod("Deserialize");

        var groupDescriptorType = dataSourceAssembly.ForceGetType(DataSourceReflectionConsts.GroupDescriptorFullName);
        GroupDescriptorConstructor = groupDescriptorType.ForceGetConstructor([]);
        GroupDescriptorListConstructor = typeof(List<>).MakeGenericType(groupDescriptorType).ForceGetConstructor([]);
        GroupDescriptorDeserializeMethod = groupDescriptorType.ForceGetMethod("Deserialize");

        var aggregateDescriptorType = dataSourceAssembly.ForceGetType(DataSourceReflectionConsts.AggregateDescriptorFullName);
        AggregateDescriptorConstructor = aggregateDescriptorType.ForceGetConstructor([]);
        AggregateDescriptorListConstructor = typeof(List<>).MakeGenericType(aggregateDescriptorType).ForceGetConstructor([]);
        AggregateDescriptorDeserializeMethod = aggregateDescriptorType.ForceGetMethod("Deserialize");
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
    public static dynamic? CreateFilterDescriptor(string source) => FilterDescriptorFactoryCreateMethod.Invoke(null, [source]);

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
}
