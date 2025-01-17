using System.Reflection;

namespace Telerik.DataSource.Mvc.Binder.Extensions;

internal static class ReflectionExtensions
{
    /// <summary>
    /// 强制加载程序集，未找到时会抛出错误
    /// </summary>
    /// <param name="assemblyString">程序集名称</param>
    public static Assembly ForceLoadAssembly(string assemblyString)
    {
        return Assembly.Load(assemblyString) ??
            throw new Exception(string.Format("Not found assembly \"{0}\".", assemblyString));
    }

    /// <summary>
    /// 强制获取类型，未找到时会抛出错误
    /// </summary>
    /// <param name="assembly">程序集</param>
    /// <param name="typeName">类型名称</param>
    public static Type ForceGetType(this Assembly assembly, string typeName)
    {
        return assembly.GetType(typeName) ??
            throw new Exception(string.Format("Not found type \"{0}\" in assembly \"{1}\".", typeName, assembly.GetName().Name));
    }

    /// <summary>
    /// 强制获取构造函数，未找到时会抛出错误
    /// </summary>
    /// <param name="type">类型</param>
    /// <param name="paramTypes">参数类型数组</param>
    public static ConstructorInfo ForceGetConstructor(this Type type, Type[] paramTypes)
    {
        return type.GetConstructor(paramTypes) ??
            throw new Exception(string.Format("Not found constructor in type \"{0}\".", type.FullName));
    }

    /// <summary>
    /// 强制获取方法，未找到时会抛出错误
    /// </summary>
    /// <param name="type">类型</param>
    /// <param name="methodName">方法名</param>
    public static MethodInfo ForceGetMethod(this Type type, string methodName)
    {
        return type.GetMethod(methodName) ??
            throw new Exception(string.Format("Not found method \"{0}\" in type \"{1}\".", methodName, type.FullName));
    }
}
