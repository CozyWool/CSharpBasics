using System;
using System.Linq;
using System.Reflection;

namespace Documentation;

public class Specifier<T> : ISpecifier
{
    private readonly Type _type = typeof(T);
    private const BindingFlags Flags = BindingFlags.Public | BindingFlags.Instance;

    public string GetApiDescription() => _type.GetCustomAttribute<ApiDescriptionAttribute>()?.Description;

    public string[] GetApiMethodNames() => _type.GetMethods(Flags)
                                                .Where(x => x.GetCustomAttribute<ApiMethodAttribute>() is not null)
                                                .Select(x => x.Name)
                                                .ToArray();

    public string GetApiMethodDescription(string methodName) =>
        _type.GetMethod(methodName, Flags)?.GetCustomAttribute<ApiDescriptionAttribute>()?.Description;


    public string[] GetApiMethodParamNames(string methodName) =>
        _type.GetMethod(methodName, Flags)?.GetParameters().Select(x => x.Name).ToArray();

    public string GetApiMethodParamDescription(string methodName, string paramName) =>
        GetParameterInfo(methodName, paramName)
          ?.GetCustomAttribute<ApiDescriptionAttribute>()
          ?.Description;

    private ParameterInfo GetParameterInfo(string methodName, string paramName) =>
        _type.GetMethod(methodName, Flags)
            ?.GetParameters()
             .FirstOrDefault(x => x.Name == paramName);

    public ApiParamDescription GetApiMethodParamFullDescription(string methodName, string paramName)
    {
        var method = _type.GetMethod(methodName, Flags);
        if (method is null)
        {
            return CreateApiParamDescription(methodName, paramName, null);
        }

        var param = paramName is null ? method.ReturnParameter : GetParameterInfo(methodName, paramName);
        return CreateApiParamDescription(methodName, paramName, param);
    }

    private ApiParamDescription CreateApiParamDescription(string methodName, string paramName, ParameterInfo param) =>
        new()
        {
            ParamDescription =
                new CommonDescription(paramName, GetApiMethodParamDescription(methodName, paramName)),
            Required = param?.GetCustomAttribute<ApiRequiredAttribute>()?.Required ?? false,
            MinValue = param?.GetCustomAttribute<ApiIntValidationAttribute>()?.MinValue,
            MaxValue = param?.GetCustomAttribute<ApiIntValidationAttribute>()?.MaxValue
        };

    public ApiMethodDescription GetApiMethodFullDescription(string methodName)
    {
        var method = _type.GetMethod(methodName, Flags);
        if (method?.GetCustomAttribute<ApiMethodAttribute>() is null)
        {
            return null;
        }

        return new ApiMethodDescription
               {
                   MethodDescription = new CommonDescription(methodName, GetApiMethodDescription(methodName)),
                   ParamDescriptions = method.GetParameters()
                                             .Select(param => GetApiMethodParamFullDescription(methodName, param.Name))
                                             .ToArray(),
                   ReturnDescription =
                       method.ReturnType != typeof(void)
                           ? GetApiMethodParamFullDescription(methodName, null)
                           : null
               };
    }
}