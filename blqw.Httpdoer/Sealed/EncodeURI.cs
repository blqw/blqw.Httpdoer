using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Web
{
    public static class URIEx
    {
        public static string Encode(string str)
        {
            if (str == null || str.Length == 0)
            {
                return str;
            }
            var length = str.Length;
            str = Uri.EscapeDataString(str);
            if (str.Length == length)
            {
                return str;
            }
            str = Replace(str);
            return str;
        }

        private unsafe static string Replace(string str)
        {
            var length = str.Length;
            SBBuffer buffer;
            fixed (char* p = str)
            {
                int s = 0;
                for (int i = 0; i < length;)
                {
                    var c = p[i];
                    if (c == '%')
                    {
                        if (p[i + 1] == '2')
                        {
                            switch (p[i + 2])
                            {
                                case '1':
                                    if (s < i)
                                        buffer.Append(p, s, i - s);
                                    buffer.Append('!');
                                    s = i + 3;
                                    break;
                                case '7':
                                    if (s < i)
                                        buffer.Append(p, s, i - s);
                                    buffer.Append('\'');
                                    s = i + 3;
                                    break;
                                case '8':
                                    if (s < i)
                                        buffer.Append(p, s, i - s);
                                    buffer.Append('(');
                                    s = i + 3;
                                    break;
                                case '9':
                                    if (s < i)
                                        buffer.Append(p, s, i - s);
                                    buffer.Append(')');
                                    s = i + 3;
                                    break;
                                case 'A':
                                    if (s < i)
                                        buffer.Append(p, s, i - s);
                                    buffer.Append('*');
                                    s = i + 3;
                                    break;
                                default:
                                    break;
                            }
                        }
                        i += 3;
                    }
                    else
                    {
                        i++;
                    }
                }
                if (s != 0)
                {
                    if (s < length)
                    {
                        buffer.Append(p, s, length - s);
                    }
                    str = buffer.ToString();
                    buffer.Clear();
                }
                return str;
            }
        }

        private static readonly char[] HexUpperChars = {
                                   '0', '1', '2', '3', '4', '5', '6', '7',
                                   '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };

        public static Uri GetFullURL(Uri baseUrl, string path)
        {
            if (path == null && baseUrl == null)
            {
                return null;
            }

            Uri url;
            if (baseUrl == null)
            {
                if (Uri.TryCreate(path, UriKind.Absolute, out url) == false)
                {
                    throw new UriFormatException($"{nameof(path)}错误:不是url");
                }
                return url;
            }
            if (string.IsNullOrWhiteSpace(path))
            {
                return baseUrl;
            }
            if (Uri.TryCreate(baseUrl, path, out url))
            {
                return url;
            }
            throw new UriFormatException($"{nameof(baseUrl)} + {nameof(path)}错误:不是url");
        }


        struct SBBuffer
        {
            [ThreadStatic]
            static StringBuilder _sb;
            unsafe delegate StringBuilder AppendHandler(StringBuilder sb, char* p, int count);
            static readonly AppendHandler _append = CreateHandler();

            private unsafe static AppendHandler CreateHandler()
            {
                var method = typeof(StringBuilder)
                    .GetMethod("Append", (System.Reflection.BindingFlags)(-1), null, new[] { typeof(char*), typeof(int) }, null);
                var dm = new DynamicMethod("", typeof(StringBuilder), new Type[] { typeof(StringBuilder), typeof(char*), typeof(int) }, method.DeclaringType, true);

                var il = dm.GetILGenerator();
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldarg_1);
                il.Emit(OpCodes.Ldarg_2);
                il.Emit(OpCodes.Callvirt, method);
                il.Emit(OpCodes.Ret);
                return (AppendHandler)dm.CreateDelegate(typeof(AppendHandler));
            }
            public unsafe void Append(char* point, int start, int length)
            {
                if (_sb == null) _sb = new StringBuilder();
                _append(_sb, point + start, length);
            }
            public override string ToString()
            {
                return _sb == null ? "" : _sb.ToString();
            }


            public void Append(char c)
            {
                if (_sb == null) _sb = new System.Text.StringBuilder();
                _sb.Append(c);
            }

            public void Clear()
            {
                if (_sb != null)
                    _sb.Clear();
            }
        }
    }
}
