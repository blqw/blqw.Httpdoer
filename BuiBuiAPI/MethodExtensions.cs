using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BuiBuiAPI
{
    static class MethodExtensions
    {
        public static void SafeCall(this Control ctl, Action action)
        {
            if (ctl?.InvokeRequired == true)
            {
                ctl.Invoke(action);
            }
            else
            {
                action();
            }
        }

        public static T SafeCall<T>(this Control ctl, Func<T> func)
        {
            if (ctl?.InvokeRequired == true)
            {
                return (T)ctl.Invoke(func);
            }
            else
            {
                return func();
            }
        }

        [ThreadStatic]
        static StringBuilder _Buffer;
        public static string InsertEveryLine(this string text, string linePre, bool ignoreFirstLine)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return ignoreFirstLine ? text + "" : linePre + text;
            }
            if (_Buffer == null)
                _Buffer = new StringBuilder();
            else
                _Buffer.Clear();
            using (var reader = new System.IO.StringReader(text))
            {
                var s = reader.ReadLine();
                if (ignoreFirstLine == false)
                    _Buffer.Append(linePre);
                _Buffer.Append(s);
                while ((s = reader.ReadLine()) != null)
                {
                    _Buffer.AppendLine();
                    _Buffer.Append(linePre);
                    _Buffer.Append(s);
                }
            }
            return _Buffer.ToString();
        }

        public static string Join(this IEnumerable<string> value, string separator)
        {
            if (value == null) return "";
            return string.Join(separator, value);
        }
    }
}
