using blqw.Web;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BuiBuiAPI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            listHistories.ItemHeight = 30;
            listHistories.DrawMode = DrawMode.OwnerDrawVariable;
            listHistories.DrawItem += ListHistories_DrawItem;
            cbbHttpMethod.Items.AddRange(typeof(HttpRequestMethod).GetFields(BindingFlags.Public | BindingFlags.Static).Select(it => it.Name).Where(it => it != "Custom").ToArray());
            cbbHttpMethod.Text = "Get";
        }

        //绘制列表
        private void ListHistories_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            e.DrawFocusRectangle();
            e.Graphics.DrawString(((ListBox)sender).Items[e.Index].ToString(), e.Font, new SolidBrush(e.ForeColor), e.Bounds);
        }

        //切换辅助面板的左右位置
        private void ckbDock_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as CheckBox)?.Checked == true)
            {
                panelSide.Dock = DockStyle.Left;
            }
            else
            {
                panelSide.Dock = DockStyle.Right;
            }
        }

        //鼠标提示
        private void listHistories_MouseMove(object sender, MouseEventArgs e)
        {
            var listbox = sender as ListBox;
            if (listbox == null) return;
            if (listbox.Tag != null &&
                ((DateTime)listbox.Tag - DateTime.Now).Ticks > 0)
            {
                return;
            }
            var index = listbox.IndexFromPoint(e.Location);
            if (index < 0 || index >= listbox.Items.Count) return;
            var txt = listbox.Items[index]?.ToString();
            tipHistory.ToolTipTitle = txt;
            tipHistory.SetToolTip(listbox, txt + "?");
            listbox.Tag = DateTime.Now.AddMilliseconds(30);
        }

    }
}
