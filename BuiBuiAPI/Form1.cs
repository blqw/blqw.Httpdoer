using blqw.Web;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
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
            //自定义绘制列表
            listFavorite.ItemHeight = listHistories.ItemHeight = 30;
            listFavorite.DrawMode = listHistories.DrawMode = DrawMode.OwnerDrawVariable;
            listHistories.DrawItem += listHistories_DrawItem;
            listFavorite.DrawItem += listHistories_DrawItem;
            //添加默认的HttpMethod
            cbbHttpMethod.Items.AddRange(typeof(HttpRequestMethod).GetFields(BindingFlags.Public | BindingFlags.Static).Select(it => it.Name).Where(it => it != "Custom").ToArray());
            cbbHttpMethod.Text = "Get";
            //添加默认ContentType
            cbbContentType.Items.AddRange(typeof(HttpContentType).GetFields(BindingFlags.Static | BindingFlags.Public).Select(it => (string)(HttpContentType)it.GetValue(null)).Where(it => !string.IsNullOrWhiteSpace(it)).ToArray());
            //进制脚本错误提示
            webResponseView.ScriptErrorsSuppressed = true;
            //绑定标准头菜单
            foreach (var menus in contextMenuStrip1.Items.OfType<ToolStripMenuItem>())
            {
                foreach (var item in menus.DropDownItems.Cast<ToolStripMenuItem>())
                {
                    var inserter = new SystemHeaderInserter(gridParams)
                    {
                        Name = menus.Text,
                        Value = item.Text,
                    };
                    item.Click += inserter.Click;
                }
            }
        }


        //绘制列表
        private void listHistories_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;
            e.DrawBackground();
            e.DrawFocusRectangle();
            e.Graphics.DrawString(((ListBox)sender).Items[e.Index].ToString(), e.Font, new SolidBrush(e.ForeColor), new RectangleF(e.Bounds.X, e.Bounds.Y, 3000, e.Bounds.Height));
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
            tipListbox.ToolTipTitle = txt;
            tipListbox.SetToolTip(listbox, txt + "?");
            listbox.Tag = DateTime.Now.AddMilliseconds(30);
        }

        //提交请求
        private async void btnBui_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtURL.Text))
            {
                txtURL.Focus();
                return;
            }
            try
            {
                btnBui.Enabled = false;
                await SendRequest();
                tabMain.SelectedTab = pageResponse;
            }
            catch (NotImplementedException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), ex.Message);
            }
            finally
            {
                btnBui.Enabled = true;
            }
        }

        //显示Cookie
        private void ShowResponseCookie(IHttpResponse response)
        {
            txtResponseCookies.Text = response?.Headers?.GetValues("Set-Cookie").Join("; ");
            gridResponseCookies.DataSource =
                response?.Cookies?.Cast<Cookie>()
                        .Select(it => new
                        {
                            it.Name,
                            it.Value,
                            it.Domain,
                            Expires = it.Expired ? it.Expires.ToString("yyyy-MM-dd HH:mm:ss") : "-",
                            it.Path,
                            it.HttpOnly,
                            it.Secure
                        })
                        .ToList();

        }

        //写入参数
        private void WriteParams(Httpdoer doer)
        {
            foreach (DataGridViewRow row in gridParams.Rows)
            {
                if (row.IsNewRow) continue;
                var name = row.Cells[colParamsName.Name].Value as string ?? "";
                var location = row.Cells[colParamsLocation.Name].Value as string;
                var value = row.Cells[colParamsValue.Name].Value as string;
                switch (location)
                {
                    case null:
                    case "Auto":
                        doer.Params.Add(name, value);
                        break;
                    case "Query":
                        doer.Query.Add(name, value);
                        break;
                    case "Body":
                        doer.Body.Add(name, value);
                        break;
                    case "Header":
                        doer.Headers.Add(name, value);
                        break;
                    case "Path":
                        doer.PathParams.Add(name, value);
                        break;
                    default:
                        break;
                }
            }
        }

        // 在界面上显示头
        private void ShowResponseHeaders(IHttpResponse response)
        {
            if (response == null) return;
            var list = new ArrayList();
            if (response.Headers != null)
            {
                foreach (var item in response.Headers)
                {
                    var arr = item.Value as IEnumerable;
                    if (arr != null && arr is string == false)
                    {
                        foreach (var value in arr)
                        {
                            list.Add(new { Name = item.Key, Value = value });
                        }
                    }
                    else
                    {
                        list.Add(new { Name = item.Key, Value = item.Value });
                    }
                }
            }
            gridResponseHeaders.DataSource = list;
        }

        //显示插入菜单
        private void btnInsertMenu_Click(object sender, EventArgs e)
        {
            var ctl = sender as Control;
            if (ctl == null) return;
            contextMenuStrip1.Show(ctl, 0, ctl.Height);
        }

        private void cbbHttpMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            var txt = (sender as Control)?.Text.ToUpperInvariant();
            switch (txt)
            {
                case "GET":
                case "DELETE":
                    cbbContentType.Text = "";
                    break;
                case "POST":
                    if (string.IsNullOrWhiteSpace(cbbContentType.Text))
                    {
                        cbbContentType.Text = "application/x-www-form-urlencoded";
                    }
                    break;
                default:
                    break;
            }

        }

        private void cbbContentType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace((sender as Control)?.Text) == false)
            {
                switch (cbbHttpMethod.Text.ToUpperInvariant())
                {
                    case "GET":
                    case "DELETE":
                        cbbHttpMethod.Text = "Post";
                        break;
                    default:
                        break;
                }
            }
        }
        #region SendRequest


        private CookieContainer _cookies = new CookieContainer();
        protected virtual async Task SendRequest()
        {
            await Task.Delay(1);
            var request = new Httpdoer(txtURL.Text)
            {
                UseCookies = true,
                Timeout = TimeSpan.FromSeconds(decimal.ToDouble(numTimeout.Value)),
            };
            //设置Cookie
            if (ckbKeepCookie.Checked)
            {
                request.Cookies = _cookies;
            }
            request.Headers.KeepAlive = ckbKeepAlive.Checked;
            request.Loggers.Add(new BuibuiLogger(rtxtLogs));
            request.HttpMethod = cbbHttpMethod.Text;
            if (!string.IsNullOrWhiteSpace(cbbContentType.Text))
                request.Body.ContentType = cbbContentType.Text;
            //写入参数
            WriteParams(request);

            var response = request.Send();
            //var response = await request.SendAsync();
            //返回正文
            rtxtResponseBody.Text = response?.Body?.ToString();
            txtRequestRaw.Text = response?.RequestData.Raw;
            txtResponseRaw.Text = response?.ResponseRaw;
            //返回视图
            var format = response?.Body?.ContentType.Format?.ToLowerInvariant();
            if (format == "html" || format == "htm")
                webResponseView.DocumentText = rtxtResponseBody.Text;
            else
                webResponseView.DocumentText = string.Empty;
            //返回头
            ShowResponseHeaders(response);
            //显示Cookie
            ShowResponseCookie(response);
            _cookies.Add(response.Cookies);
            if (response.Exception != null)
            {
                throw new NotImplementedException(response.Exception.Message, response.Exception);
            }
        }

        #endregion

    }
}
