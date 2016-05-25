﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Web
{
    public interface IHttpParameterCollection : IEnumerable<HttpParamValue>
    {
        object GetValue(string name, HttpParamLocation location);
        void SetValue(HttpParamValue value);
        void SetValue(string name, object value, HttpParamLocation location);
    }
}