using blqw.Web;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BuiBuiAPI
{
    class BuibuiListener : TraceListener
    {
        RichTextBox _outer;
        public BuibuiListener(RichTextBox outer)
        {
            _outer = outer;
        }

        private void Write(Color color, string type, string message)
        {
            if (_outer.TextLength > 0)
                _outer.SafeCall(() => _outer.AppendText(Environment.NewLine));
            _outer.SafeCall(() =>
            {
                var fore = _outer.SelectionColor;

                _outer.SelectionColor = Color.DodgerBlue;
                _outer.AppendText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffffff"));
                _outer.SelectionColor = color;
                _outer.AppendText(" ");
                _outer.AppendText(type);
                _outer.AppendText(" ");
                _outer.SelectionColor = fore;
                _outer.AppendText(message.InsertEveryLine(new string(' ', 34), true));
            });
        }
        
        /// <summary>在派生类中被重写时，向在该派生类中所创建的侦听器写入指定消息。</summary>
        /// <param name="message">要写入的消息。</param>
        public override void Write(string message) => Write(Color.DarkGray, " [调试] ", message);

        /// <summary>在派生类中被重写时，向在该派生类中所创建的侦听器写入消息，后跟行结束符。</summary>
        /// <param name="message">要写入的消息。</param>
        public override void WriteLine(string message) => Write(Color.DarkGray, " [调试] ", message);


        private static readonly Dictionary<TraceEventType, Color> _Colors = new Dictionary<TraceEventType, Color>()
        {
            [TraceEventType.Critical] = Color.DarkRed,
            [TraceEventType.Error] = Color.Red,
            [TraceEventType.Warning] = Color.Tomato,
            [TraceEventType.Information] = Color.LimeGreen,
            [TraceEventType.Verbose] = Color.DarkGray,
            [TraceEventType.Start] = Color.LightGray,
            [TraceEventType.Stop] = Color.LightGray,
            [TraceEventType.Suspend] = Color.LightGray,
            [TraceEventType.Resume] = Color.LightGray,
            [TraceEventType.Transfer] = Color.LightGray,
        };

        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, object data)
        {
            Color color;
            if (_Colors.TryGetValue(eventType, out color) == false)
            {
                color = Color.DarkGray;
            }
            Write(color, $"[{eventType}]", data?.ToString());
        }

        /// <summary>向特定于侦听器的输出中写入跟踪信息、数据对象的数组和事件信息。</summary>
        /// <param name="eventCache">包含当前进程 ID、线程 ID 以及堆栈跟踪信息的 <see cref="T:System.Diagnostics.TraceEventCache" /> 对象。</param>
        /// <param name="source">标识输出时使用的名称，通常为生成跟踪事件的应用程序的名称。</param>
        /// <param name="eventType">
        /// <see cref="T:System.Diagnostics.TraceEventType" /> 值之一，指定引发跟踪的事件类型。</param>
        /// <param name="id">事件的数值标识符。</param>
        /// <param name="data">要作为数据发出的对象数组。</param>
        /// <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
        ///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
        /// </PermissionSet>
        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, params object[] data)
        {
            foreach (var o in data)
            {
                base.TraceData(eventCache, source, eventType, id, o);
            }
        }
        
    }

}
