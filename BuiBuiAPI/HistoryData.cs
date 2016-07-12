using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuiBuiAPI
{
    public class HistoryData
    {
        public string URL { get; set; }
        public string Method { get; set; }
        public bool KeepAlive { get; set; }
        public bool KeepCookie { get; set; }
        public int Timeout { get; set; }
        public string ContentType { get; set; }
        public List<Param> Params { get; set; }
        public string RequestRaw { get; set; }
        public string ResponseRaw { get; set; }
        public string ResponseBody { get; set; }
        public List<Header> ResponseHeaders { get; set; }
        public List<Cookie> ResponseCookies { get; set; }
        public string ResponseCookieRaw { get; set; }
        public string LogsRTF { get; set; }
        
        public override string ToString()
        {
            return URL + Environment.NewLine + Method;
        }
    }

    public class Param
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public string Value { get; set; }
    }

    public class Header
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }

    public class Cookie
    {
        public string Domain { get; set; }
        public string Expires { get; set; }
        public bool HttpOnly { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public bool Secure { get; set; }
        public string Value { get; set; }
    }
}
