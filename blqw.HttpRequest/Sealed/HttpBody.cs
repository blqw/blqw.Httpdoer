﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Web
{
    /// <summary>
    /// 表示请求或响应的正文
    /// </summary>
    public sealed class HttpBody : HttpParamsBase<object>, IFormattable
    {
        internal HttpBody(IHttpParameterCollection @params)
            :base(@params)
        {

        }

        public override HttpParamLocation Location
        {
            get
            {
                return HttpParamLocation.Body;
            }
        }


        public byte[] GetBytes()
        {
            var parser = ContentType.GetFormat(typeof(IHttpBodyParser)) as IHttpBodyParser;
            if (parser == null)
            {
                return null;
            }
            return parser.Format(null, this, ContentType);
        }

        public HttpContentType ContentType { get; set; }

        public bool SetContentType(string contentType, Encoding defaultEncoding)
        {
            HttpContentType result;
            if (HttpContentType.TryParse(contentType, defaultEncoding, out result))
            {
                ContentType = result;
                return true;
            }
            return false;
        }

        public string ToString(string format)
        {
            return ToString(format, ContentType);
        }

        public override string ToString()
        {
            return ToString(null);
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (formatProvider == null)
            {
                formatProvider = ContentType;
            }
            var parser = formatProvider.GetFormat(typeof(IHttpBodyParser)) as ICustomFormatter;
            if (parser == null)
            {
                return null;
            }
            return parser.Format(format, this, formatProvider);
        }
        
    }
}