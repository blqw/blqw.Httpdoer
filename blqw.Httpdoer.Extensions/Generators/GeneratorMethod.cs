﻿using System;
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
            ContentType = method.GetCustomAttribute<ContentTypeAttribute>()?.ContentType
           ?? method.DeclaringType.GetCustomAttribute<ContentTypeAttribute>()?.ContentType;

        }
        public string ContentType { get; }
        public Type ReturnType { get; }
        public Type InterfaceType { get; }
        public string MethodName { get; }
        public GeneratorParam[] Params { get; }

        public bool IsAsync { get; }
        public string Template { get; }
        public HttpRequestMethod HttpMethod { get; }
        public Type TrackingType { get; }

        const string CODE_TEMPLATE = @"
        %async% %ResultType% %InterfaceName%.%MethodName%(%MethodArgs%)
        {
            try
            {
                if(%Tracking% != null)
                    Trackings.Add(%Tracking%);
                Method = HttpRequestMethod.%HttpMethod%;
                Path = ""%Template%"";
                Body.ContentType = %ContentType%;
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
                    return string.Join("\r\n                ", Params.Select(GetSetParamDefinition));
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
                        return "GetString" + @async;
                    }
                    else if (type == typeof(byte[]))
                    {
                        return "GetBytes" + @async;
                    }
                    else if (type == null || type == typeof(void) || type == typeof(IHttpResponse))
                    {
                        return "Send" + @async;
                    }
                    return $"GetObject{@async}<{type.GetFriendlyName()}>";
                case "RemoveParams":
                    return string.Join(";", Params.Select(GetRemoveParamDefinition));
                case "ContentType":
                    return ContentType == null ? "null" : $"\"{ContentType}\"";
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
            var name = p.ParamName;
            if (name == null)
            {
                name = "null";
            }
            else
            {
                name = $"\"{name}\"";
            }
            var value = p.VarName;
            if (p.Format != null)
            {
                return $@"var {value}_fmt = {value} as IFormattable;
                if ({value}_fmt == null)
                {{
                    SetParam({name}, {value}, HttpParamLocation.{p.Location.ToString()});
                }}
                else
                {{
                    SetParam({name}, {value}_fmt.ToString(""{p.Format}"", null), HttpParamLocation.{p.Location.ToString()});
                }}";
            }
            return $@"SetParam({name}, {value}, HttpParamLocation.{p.Location.ToString()});";
        }

        public static string GetRemoveParamDefinition(GeneratorParam p)
        {
            var name = p.ParamName;
            if (name == null)
            {
                name = "null";
            }
            else
            {
                name = $"\"{name}\"";
            }
            return $@"SetParam({name}, null, HttpParamLocation.{p.Location.ToString()})";
        }
    }
}
