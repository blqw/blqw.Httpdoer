using blqw;
using blqw.Web;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
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
            RefreshHistory();
            RefreshFavorite();
            cbbEncoding.DataSource = Encoding.GetEncodings().Select(it => it.GetEncoding()).OrderBy(it=>it.WebName).ToList();
            cbbEncoding.DisplayMember = "WebName";
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
            var history = (HistoryData)listbox.Items[index];
            tipListbox.ToolTipTitle = history.URL;
            tipListbox.SetToolTip(listbox, history.RequestRaw);
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
                response?.Cookies?.Cast<System.Net.Cookie>()
                        .Select(it => new Cookie
                        {
                            Name = it.Name,
                            Value = it.Value,
                            Domain = it.Domain,
                            Expires = it.Expired ? it.Expires.ToString("yyyy-MM-dd HH:mm:ss") : "-",
                            Path = it.Path,
                            HttpOnly = it.HttpOnly,
                            Secure = it.Secure
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
            var list = new List<Header>();
            if (response.Headers != null)
            {
                foreach (var item in response.Headers)
                {
                    var arr = item.Value as IEnumerable;
                    if (arr != null && arr is string == false)
                    {
                        foreach (var value in arr)
                        {
                            list.Add(new Header { Name = item.Key, Value = value + "" });
                        }
                    }
                    else
                    {
                        list.Add(new Header { Name = item.Key, Value = item.Value + "" });
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

        //选择请求方法
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

        //选择正文类型
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

        //刷新历史记录或收藏列表
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            if (tabControl2.SelectedTab == pageHistory)
            {
                RefreshHistory();
            }
            else
            {
                RefreshFavorite();
            }
        }

        //点击历史记录或收藏
        private void listFavorite_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                var list = sender as ListBox;
                if (list == null) return;
                var history = list.SelectedItem as HistoryData;
                ShowHistory(history);
            }
        }

        #region SendRequest

        private CookieContainer _cookies = new CookieContainer();
        protected virtual async Task SendRequest()
        {
            await Task.Delay(1);
            rtxtLogs.Clear();
            var request = new Httpdoer(txtURL.Text)
            {
                UseCookies = true,
                Timeout = TimeSpan.FromSeconds(decimal.ToDouble(numTimeout.Value)),
            };
            request.Trackings.Add(new BuibuiTracking());
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

            //var response = request.Send();
            var response = await request.SendAsync();
            //返回正文
            rtxtResponseBody.Tag = response?.Body?.ResponseBody;
            //设置编码
            cbbEncoding.Text = response?.Body?.ContentType.Charset?.WebName;

            txtRequestRaw.Text = response?.RequestData.Raw;
            txtResponseRaw.Text = response?.ResponseRaw;
            //显示视图
            ShowView();
            //返回头
            ShowResponseHeaders(response);
            //显示Cookie
            ShowResponseCookie(response);
            if (response.Cookies != null)
                _cookies.Add(response.Cookies);

            if (response.Exception != null)
            {
                request.Error(response.Exception);
                SaveHistory();
                throw new NotImplementedException(response.Exception.Message, response.Exception);
            }
            SaveHistory();
        }

        private void ShowView()
        {
            webResponseView.DocumentText = rtxtResponseBody.Text;
        }

        #endregion

        //在界面上历史记录
        private void ShowHistory(HistoryData history)
        {
            if (history == null) return;
            cbbContentType.Text = history.ContentType;
            ckbKeepAlive.Checked = history.KeepAlive;
            rtxtLogs.Rtf = history.LogsRTF;
            cbbHttpMethod.Text = history.Method;
            txtRequestRaw.Text = history.RequestRaw;
            rtxtResponseBody.Rtf = history.ResponseBody;
            ShowView();
            txtResponseCookies.Text = history.ResponseCookieRaw;
            txtResponseRaw.Text = history.ResponseRaw;
            numTimeout.Value = history.Timeout;
            txtURL.Text = history.URL;
            ckbKeepCookie.Checked = history.KeepCookie;
            gridParams.Rows.Clear();
            foreach (var p in history.Params)
            {
                gridParams.Rows.Add(p.Name, p.Location, p.Value);
            }
            gridParams.DataSource = history.ResponseHeaders;
            gridResponseCookies.DataSource = history.ResponseCookies;
        }

        //保存历史记录
        private void SaveHistory()
        {
            var history = new HistoryData
            {
                ContentType = cbbContentType.Text,
                KeepAlive = ckbKeepAlive.Checked,
                LogsRTF = rtxtLogs.Rtf,
                Method = cbbHttpMethod.Text,
                RequestRaw = txtRequestRaw.Text,
                ResponseBody = rtxtResponseBody.Rtf,
                ResponseCookieRaw = txtResponseCookies.Text,
                ResponseRaw = txtResponseRaw.Text,
                Timeout = (int)numTimeout.Value,
                URL = txtURL.Text,
                KeepCookie = ckbKeepCookie.Checked,
                Params = gridParams.Rows.Cast<DataGridViewRow>().Where(r => !r.IsNewRow)
                                .Select(r => new Param
                                {
                                    Name = r.Cells[0].Value + "",
                                    Location = r.Cells[1].Value?.ToString(),
                                    Value = r.Cells[2].Value + "",
                                }).ToList(),
                ResponseHeaders = (List<Header>)gridParams.DataSource,
                ResponseCookies = (List<Cookie>)gridResponseCookies.DataSource,
            };
            if (Directory.Exists("History/") == false)
            {
                Directory.CreateDirectory("History");
            }
            var path = $"History/{DateTime.Now.Ticks}";
            var json = history.ToJsonString();
            File.WriteAllText(path, json);
            RefreshHistory();
        }

        //刷新历史记录
        private void RefreshHistory()
        {
            if (Directory.Exists("History/") == false) return;
            listHistories.DataSource = Directory.GetFiles("History/").OrderByDescending(it => Path.GetFileName(it).To<long>(0)).Take((int)numMaxHistory.Value).Select(it =>
                    {
                        var history = Json.ToObject<HistoryData>(File.ReadAllText(it));
                        history.FilePath = it;
                        return history;
                    }).ToList();
            listHistories.DisplayMember = "Display";
        }

        //刷新搜藏夹
        private void RefreshFavorite()
        {
            if (Directory.Exists("Favorite/") == false) return;
            listFavorite.DataSource = Directory.GetFiles("Favorite/").OrderByDescending(it => Path.GetFileName(it).To<long>(0)).Take((int)numMaxHistory.Value).Select(it =>
            {
                var history = Json.ToObject<HistoryData>(File.ReadAllText(it));
                history.FilePath = it;
                return history;
            }).ToList();
            listHistories.DisplayMember = "Display";
        }

        //点击解析参数的按钮
        private void ParseParams_Click(object sender, EventArgs e)
        {
            var frm = new Form2();
            if (frm.ShowDialog(this) == DialogResult.OK)
            {
                var str = frm.txtParamString.Text;
                try
                {
                    var nv = HttpUtility.ParseQueryString(str);
                    var location = frm.cbbLocation.Text;
                    for (int i = 0, length = nv.Count; i < length; i++)
                    {
                        var name = nv.GetKey(i);
                        var value = nv[i];
                        gridParams.Rows.Add(name, location, value);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "解析失败");
                }
            }
        }

        //点击收藏按钮
        private void Favoring_Click(object sender, EventArgs e)
        {
            var history = listHistories.SelectedItem as HistoryData;
            if (history == null) return;
            if (Directory.Exists("Favorite/") == false)
            {
                Directory.CreateDirectory("Favorite");
            }
            var newpath = $"Favorite/{DateTime.Now.Ticks}";
            if (File.Exists(newpath)) return;
            File.Copy(history.FilePath, newpath);
            RefreshFavorite();
        }

        //点击取消收藏按钮
        private void UnFavoring_Click(object sender, EventArgs e)
        {
            var history = listFavorite.SelectedItem as HistoryData;
            if (history == null) return;
            File.Delete(history.FilePath);
            RefreshFavorite();
        }

        //右键点击历史/收藏列表
        private void listHistories_MouseDown(object sender, MouseEventArgs e)
        {
            var list = sender as ListBox;
            if (list == null) return;
            var index = list.IndexFromPoint(e.Location);
            if (index >= 0)
            {
                list.SelectedIndex = index;
            }
        }

        //在参数列表中按下键盘delete按钮
        private void gridParams_KeyDown(object sender, KeyEventArgs e)
        {
            if (gridParams.SelectedCells.Count == 0)
            {
                return;
            }
            switch (e.KeyData)
            {
                case Keys.Delete:
                    if (gridParams.SelectedCells[0].OwningRow.IsNewRow == false)
                    {
                        gridParams.Rows.Remove(gridParams.SelectedCells[0].OwningRow);
                    }
                    break;
                //case Keys.V | Keys.Control:
                //    if (Clipboard.ContainsText())
                //    {
                //        gridParams.SelectedCells[0].Value = Clipboard.GetText();
                //    }
                //    break;
                default:
                    //gridParams.BeginEdit(true);
                    break;
            }
        }

        private void gridParams_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == 3 && gridParams.Rows[e.RowIndex].IsNewRow == false)
            {
                gridParams.Rows.RemoveAt(e.RowIndex);
            }
        }

        //更换编码格式
        private void cbbEncoding_SelectedIndexChanged(object sender, EventArgs e)
        {
            var body = rtxtResponseBody.Tag as byte[];
            if (body != null)
            {
                rtxtResponseBody.Text = (cbbEncoding.SelectedValue as Encoding ?? Encoding.Default).GetString(body);
            }
        }
    }
}
