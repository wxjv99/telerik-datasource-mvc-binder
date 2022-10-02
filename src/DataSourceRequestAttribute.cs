using Microsoft.AspNetCore.Mvc;

namespace Telerik.DataSource.Mvc.Binder
{
    /// <summary>
    /// 用以使用 <see cref="DataSourceRequestModelBinder"/> 来构建并向 DataSourceRequest 绑定值的属性
    /// </summary>
    public class DataSourceRequestAttribute : ModelBinderAttribute
    {
        public DataSourceRequestAttribute()
        {
            base.BinderType = typeof(DataSourceRequestModelBinder);
        }
    }
}
