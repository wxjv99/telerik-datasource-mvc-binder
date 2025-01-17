using Telerik.DataSource.Mvc.Binder.Reflection;

namespace Telerik.DataSource.Mvc.Binder;

/// <summary>
/// 数据源描述器对象的反序列化器
/// </summary>
internal class DataSourceDescriptorDeserializer
{
    /// <summary>
    /// 分隔符
    /// </summary>
    private const char ColumnDelimiter = '~';

    /// <summary>
    /// 依照 <paramref name="descriptorFullName"/> 指定的描述器名称来反序列化描述器列表
    /// </summary>
    /// <param name="descriptorFullName">描述器的类型全称, 可选值为 <see cref="DataSourceReflectionConsts"/> 中的常量</param>
    /// <param name="from">用以反序列化的数据源</param>
    /// <returns>反序列化描述器列表</returns>
    public static dynamic Deserialize(string descriptorFullName, string? from)
    {
        // 创建一个具有指定的描述器类型的空列表
        var list = DataSourceReflectionHelper.ConstructDescriptorList(descriptorFullName);
        if (string.IsNullOrEmpty(from))
        {
            return list;
        }
        // 使用分隔符将同种描述类型的多组数据分隔开
        var array = from.Split(ColumnDelimiter, StringSplitOptions.RemoveEmptyEntries);
        foreach (string source in array)
        {
            // 构造描述器
            var item = DataSourceReflectionHelper.ConstructDescriptor(descriptorFullName);
            // 反序列化当前的一组数据为描述器赋值
            DataSourceReflectionHelper.DeserializeDescriptor(descriptorFullName, item, source);
            list.Add(item);
        }
        return list;
    }
}
