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
        string Translate([Header]string token, string Text, string To);
    }
}
