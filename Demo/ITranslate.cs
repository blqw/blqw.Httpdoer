using blqw.Web;
using blqw.Web.Generator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo
{
    public interface ITranslate
    {
        [HttpGet("Bing/MicrosoftTranslator/v1/Translate")]
        //[ContentType(HttpContentTypes.Form)]
        string Translate([Header("Authorization")]string token, string Text, string To);
    }

    public class JKFLDHKJFSF : HttpRequest, ITranslate
    {
        string ITranslate.Translate(string token, string Text, string To)
        {
            Method = HttpRequestMethod.Get;
            Path = "Bing/MicrosoftTranslator/v1/Translate";
            
            Params["Text"] = Text;
            Params["To"] = To;
            Headers["Authorization"] = token;
            return Httpdoer.GetString(this);
        }
    }

}
