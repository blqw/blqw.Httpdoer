using blqw.Web;
using blqw.Web.Generator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo
{
    public interface ITranslate : ITranslateAsync
    {
        [HttpGet("Bing/MicrosoftTranslator/v1/Translate")]
        string Translate([Header("Authorization")]string token, string Text, string To);
    }

    public interface ITranslateAsync
    {
        [HttpGet("Bing/MicrosoftTranslator/v1/Translate")]
        Task<string> TranslateAsync([Header]string Authorization, string Text, string To);
    }
}
