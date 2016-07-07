using blqw.Web;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BuiBuiAPI
{
    class FormLogger : IHttpLogger
    {
        RichTextBox _outer;
        public FormLogger(RichTextBox outer)
        {
            _outer = outer;
        }

        private void Wirte(Color color, string type, string message)
        {
            if (_outer.TextLength > 0)
                _outer.SafeCall(() => _outer.AppendText(Environment.NewLine));
            _outer.SafeCall(() =>
            {
                var fore = _outer.SelectionColor;

                _outer.SelectionColor = Color.DodgerBlue;
                _outer.AppendText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffffff"));
                _outer.SelectionColor = color;
                _outer.AppendText(type);
                _outer.SelectionColor = fore;
                _outer.AppendText(message.InsertEveryLine(new string(' ', 34), true));
            });
        }

        public void Debug(string message)
        {
            Wirte(Color.DarkGray, " [调试] ", message);
        }

        public void Error(Exception ex)
        {
            Wirte(Color.Red, " [异常] ", ex.ToString());
        }

        public void Error(string message)
        {
            Wirte(Color.Red, " [错误] ", message);
        }

        public void Information(string message)
        {
            Wirte(Color.LimeGreen, " [提示] ", message);
        }

        public void Warning(string message)
        {
            Wirte(Color.Tomato, " [警告] ", message);
        }
    }

}
