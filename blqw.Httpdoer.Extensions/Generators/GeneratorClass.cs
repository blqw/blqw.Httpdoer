using blqw.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Web.Extensions
{
    static class GeneratorClass
    {
        const string CODE_CLASS = "public class _{0} : Httpdoer, ICloneable, {1}";
        const string CODE_CLONE = "        public object Clone() {{ return new _{0}(); }}";
        const string CODE_TRACKING = "";
        static int _Identity = 0;


        public static ICloneable GetObject(Type interfaceType)
        {
            if (interfaceType == null)
            {
                throw new ArgumentNullException(nameof(interfaceType));
            }
            if (interfaceType.IsInterface == false)
            {
                throw new ArgumentOutOfRangeException(nameof(interfaceType), "必须是接口");
            }
            var usingTypes = new List<Type>()
            {
                interfaceType,
                typeof(HttpRequest),
                typeof(int),
                typeof(System.Net.Http.HttpMessageInvoker),
                typeof(System.Net.IWebProxy),
                typeof(System.Net.CookieContainer),
                typeof(System.Diagnostics.TraceSource)
            };
            var id = System.Threading.Interlocked.Increment(ref _Identity);
            var className = Guid.NewGuid().ToString("n") + id;
            var buffer = new StringBuilder();
            buffer.AppendFormat(CODE_CLASS, className, interfaceType.GetFriendlyName());
            buffer.AppendLine();
            buffer.AppendLine("{");
            buffer.AppendFormat(CODE_CLONE, className);
            buffer.AppendLine();

            foreach (var method in GetAllMethods(interfaceType))
            {
                if (method.GetCustomAttribute<HttpVerbAttribute>() != null)
                {
                    var m = new GeneratorMethod(method);
                    buffer.AppendLine(m.ToString());
                    usingTypes.Add(m.ReturnType);
                    usingTypes.AddRange(m.Params.Select(it => it.ParamType));
                }
            }

            buffer.AppendLine("}");
            var code = buffer.ToString();

            return (ICloneable)DyCompiler.CompileObject(code, usingTypes.ToArray());
        }

        private static IEnumerable<MethodInfo> GetAllMethods(Type interfaceType)
        {
            foreach (var method in interfaceType.GetMethods())
            {
                yield return method;
            }
            foreach (var i in interfaceType.GetInterfaces())
            {
                foreach (var method in i.GetMethods())
                {
                    yield return method;
                }
            }
        }
    }
}
