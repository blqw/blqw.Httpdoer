using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace blqw.Web.Extensions
{
    struct GeneratorMethod
    {
        public GeneratorMethod(MethodInfo method)
        {
            if (method == null)
            {
                throw new ArgumentNullException(nameof(method));
            }
            else if (method.IsGenericMethodDefinition)
            {
                throw new ArgumentException("不能是泛型定义方法", nameof(method));
            }
            var verb = method.GetCustomAttribute<HttpVerbAttribute>();
            HttpMethod = verb?.Method ?? HttpRequestMethod.Get;
            Template = verb?.Template ?? "";
            ReturnType = method.ReturnType;
            IsAsync = typeof(Task).IsAssignableFrom(ReturnType);
            Params = method.GetParameters().Select(it => new GeneratorParam(it)).ToArray();
            MethodName = method.Name;
            InterfaceType = method.DeclaringType;
            TrackingType = method.GetCustomAttribute<TrackingAttribute>()?.Type
           ?? method.DeclaringType.GetCustomAttribute<TrackingAttribute>()?.Type;
        }

        public Type ReturnType { get; private set; }
        public Type InterfaceType { get; private set; }
        public string MethodName { get; private set; }
        public GeneratorParam[] Params { get; private set; }

        public bool IsAsync { get; private set; }
        public string Template { get; private set; }
        public HttpRequestMethod HttpMethod { get; private set; }
        public Type TrackingType { get; private set; }

        const string CODE_TEMPLATE = @"
        %async% %ResultType% %InterfaceName%.%MethodName%(%MethodArgs%)
        {
            try
            {
                if(%Tracking% != null)
                    Trackings.Add(%Tracking%);
                Method = HttpRequestMethod.%HttpMethod%;
                Path = ""%Template%"";
                %Params%;
                %return% %await% HttpRequestExtensions.%Invoker%(this);
            }
            finally
            {
                %RemoveParams%;
            }   
        }
";

        static readonly Regex _Regex = new Regex(@"%\w+%", RegexOptions.Compiled);

        public override string ToString()
        {
            return _Regex.Replace(CODE_TEMPLATE, MatchEvaluator);
        }

        string MatchEvaluator(Match m)
        {

            switch (m.Value.Substring(1, m.Value.Length - 2))
            {
                case "Tracking":
                    return TrackingType == null ? "null" : $"new {TrackingType.GetFriendlyName()}()";
                case "async":
                    return IsAsync ? "async" : "";
                case "ResultType":
                    return ReturnType.GetFriendlyName();
                case "InterfaceName":
                    return InterfaceType.GetFriendlyName();
                case "MethodName":
                    return MethodName;
                case "MethodArgs":
                    return string.Join(", ", Params.Select(GetMethodArgumentDefinition));
                case "HttpMethod":
                    return HttpMethod.ToString();
                case "Template":
                    return Template.Replace("\n", "%0A")
                                   .Replace("\r", "%0D")
                                   .Replace("\"", "%22");
                case "Params":
                    return string.Join(";", Params.Select(GetSetParamDefinition));
                case "return":
                    return ReturnType != null && ReturnType != typeof(void) && ReturnType != typeof(Task) ? "return" : "";
                case "await":
                    return IsAsync ? "await" : "";
                case "Invoker":
                    var type = ReturnType;
                    var @async = "";
                    if (IsAsync)
                    {
                        if (type == typeof(Task))
                        {
                            type = null;
                        }
                        else
                        {
                            type = type.GetGenericArguments()[0];
                        }
                        @async = "Async";
                    }
                    if (type == typeof(string))
                    {
                        return "GetString" + "";
                    }
                    else if (type == typeof(byte[]))
                    {
                        return "GetBytes" + "";
                    }
                    else if (type != null && type != typeof(void))
                    {
                        return "Send" + "";
                    }
                    return $"GetObject{""}<{type.GetFriendlyName()}>";
                case "RemoveParams":
                    return string.Join(";", Params.Select(GetRemoveParamDefinition));
                default:
                    return "";
            }
        }

        public static string GetMethodArgumentDefinition(GeneratorParam p)
        {
            return $"{p.ParamType.GetFriendlyName()} {p.VarName}";
        }

        public static string GetSetParamDefinition(GeneratorParam p)
        {
            return $@"SetParam(""{p.ParamName}"", {p.VarName}, HttpParamLocation.{p.Location.ToString()})";
        }

        public static string GetRemoveParamDefinition(GeneratorParam p)
        {
            return $@"SetParam(""{p.ParamName}"", null, HttpParamLocation.{p.Location.ToString()})";
        }
    }
}
