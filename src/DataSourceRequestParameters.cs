namespace TelerikDS.Mvc.Binder;

/// <summary>
/// DataSourceRequest 的参数名
/// </summary>
public static class DataSourceRequestParameters
{
    /// <summary>
    /// 跳过的条数
    /// </summary>
    public static string Skip { get; }

    /// <summary>
    /// 页码
    /// </summary>
    public static string Page { get; }

    /// <summary>
    /// 页内容条数
    /// </summary>
    public static string PageSize { get; }

    /// <summary>
    /// 排序
    /// </summary>
    public static string Sort { get; }

    /// <summary>
    /// 过滤
    /// </summary>
    public static string Filter { get; }

    /// <summary>
    /// 聚合
    /// </summary>
    public static string Aggregates { get; }

    /// <summary>
    /// 分组
    /// </summary>
    public static string Group { get; }

    /// <summary>
    /// 分页分组
    /// </summary>
    public static string GroupPaging { get; }

    static DataSourceRequestParameters()
    {
        Skip = "skip";
        Page = "page";
        PageSize = "pageSize";
        Sort = "sort";
        Filter = "filter";
        Aggregates = "aggregate";
        Group = "group";
        GroupPaging = "groupPaging";
    }
}
