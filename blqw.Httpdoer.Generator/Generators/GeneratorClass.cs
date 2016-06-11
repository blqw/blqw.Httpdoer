using blqw.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Web.Generator
{
    static class GeneratorClass
    {
        const string CODE_CLASS = "public class _{0}{1} : HttpRequest, ICloneable, {2}";
        const string CODE_CLONE = "public object Clone() { return new _{0}{1}(); }";
        static int _Identity = 0;
        public static string GetCodes(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }
            if (type.IsInterface == false)
            {
                throw new ArgumentOutOfRangeException(nameof(type), "必须是接口");
            }
            var buffer = new StringBuilder();
            var id = System.Threading.Interlocked.Increment(ref _Identity);
            buffer.AppendFormat(CODE_CLASS, Guid.NewGuid().ToString("n"), id, type.GetFriendlyName());
            buffer.AppendLine();
            buffer.AppendLine("{");
            buffer.AppendFormat(CODE_CLONE, Guid.NewGuid().ToString("n"), id);
            buffer.AppendLine();

            foreach (var method in type.GetMethods())
            {
                var m = new GeneratorMethod(method);
                buffer.AppendLine(m.ToString());
            }

            buffer.AppendLine("}");
            return buffer.ToString();
        }

        static IConvertible GetService(Type interfaceType)
        {
            if (interfaceType == null)
            {
                throw new ArgumentNullException(nameof(interfaceType));
            }
            if (interfaceType.IsInterface == false)
            {
                throw new ArgumentOutOfRangeException(nameof(interfaceType), "必须是接口");
            }
            var code = GeneratorClass.GetCodes(interfaceType);
            var @object = DyCompiler.CompileObject(code, interfaceType, typeof(HttpRequest), typeof(ICloneable));
            //var code = n
        }

    }
}
