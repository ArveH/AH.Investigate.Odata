using System.Reflection;
using Microsoft.OData.UriParser;

namespace Pg.Api;

public static class CustomUriFunction
{
    public static void AddPadRight()
    {
        FunctionSignatureWithReturnType padrightStringEdmFunction =
            new FunctionSignatureWithReturnType(
                EdmCoreModel.Instance.GetString(true),
                EdmCoreModel.Instance.GetString(true),
                EdmCoreModel.Instance.GetInt32(false));
 
        MethodInfo padRightStringMethodInfo = typeof(string).GetMethod("PadRight", new Type[] { typeof(int) })
            ?? throw new ArgumentException("");
        const string padrightMethodName = "padright";
        ODataUriFunctions.RemoveCustomUriFunction(padrightMethodName, padrightStringEdmFunction, padRightStringMethodInfo);
        ODataUriFunctions.AddCustomUriFunction(padrightMethodName, padrightStringEdmFunction, padRightStringMethodInfo);
    }

    public static void StartsWith()
    {
        const string methodName = "StartsWith";

        FunctionSignatureWithReturnType edmFunction =
            new FunctionSignatureWithReturnType(
                EdmCoreModel.Instance.GetBoolean(false),
                EdmCoreModel.Instance.GetString(true),
                EdmCoreModel.Instance.GetString(true));
 
        MethodInfo methodInfo = typeof(string).GetMethod(methodName, new[] { typeof(string) })
                                              ?? throw new ArgumentException($"Couldn't get MethodInfo for '{methodName}'");
        ODataUriFunctions.RemoveCustomUriFunction(methodName, edmFunction, methodInfo);
        ODataUriFunctions.AddCustomUriFunction(methodName, edmFunction, methodInfo);
    }

}

public static class CustomStringFunctions
{
    public static bool StartsWith(string text, string prefix)
    {
        return text.PadRight(10).StartsWith(prefix);
    }
}