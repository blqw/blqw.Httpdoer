using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Web
{
    internal class HttpFormBodyParser : HttpBodyParserBase
    {
        public static HttpFormBodyParser Instance { get; } = new HttpFormBodyParser();

        public override IEnumerable<KeyValuePair<string, object>> Deserialize(byte[] bytes, IFormatProvider formatProvider)
        {
            throw new NotImplementedException("不支持");
        }

        [ThreadStatic]
        static HttpQueryBuilder _QueryBuilder;

        public override byte[] Serialize(string format, IEnumerable<KeyValuePair<string, object>> body, IFormatProvider formatProvider)
        {
            if (_QueryBuilder == null)
            {
                _QueryBuilder = new HttpQueryBuilder();
            }
            else
            {
                _QueryBuilder.Clear();
            }

            foreach (var item in body)
            {
                _QueryBuilder.AppendObject(item.Key, item.Value);
            }

            return Encoding.ASCII.GetBytes(_QueryBuilder.ToString());
        }

    }
}
