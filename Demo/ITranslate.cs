using blqw.Web;
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
        string Translate([Header]string Authorization, string Text, string To);
    }

    public class JKFLDHKJFSF : HttpRequest, ITranslate
    {
        string ITranslate.Translate(string Authorization, string Text, string To)
        {
            Method = HttpRequestMethod.Get;
            Path = "Bing/MicrosoftTranslator/v1/Translate";
            Params["Text"] = Text;
            Params["To"] = To;
            Headers["Authorization"] = Authorization;
            return Httpdoer.GetString(this);
        }
    }

}
