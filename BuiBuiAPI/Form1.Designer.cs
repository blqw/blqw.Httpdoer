﻿namespace BuiBuiAPI
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.panelSide = new System.Windows.Forms.Panel();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.pageHistory = new System.Windows.Forms.TabPage();
            this.listHistories = new System.Windows.Forms.ListBox();
            this.contextMenuStrip3 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.收藏ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.删除ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pageFavorite = new System.Windows.Forms.TabPage();
            this.listFavorite = new System.Windows.Forms.ListBox();
            this.contextMenuStrip4 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.取消收藏ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ckbDock = new System.Windows.Forms.CheckBox();
            this.numMaxHistory = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.tabMain = new System.Windows.Forms.TabControl();
            this.pageParams = new System.Windows.Forms.TabPage();
            this.tabRequestData = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.gridParams = new System.Windows.Forms.DataGridView();
            this.colParamsName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colParamsLocation = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.colParamsValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRemove = new System.Windows.Forms.DataGridViewButtonColumn();
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.解析Get参数ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pageCustomRequestData = new System.Windows.Forms.TabPage();
            this.txtCustomRequestData = new System.Windows.Forms.TextBox();
            this.pageRequest = new System.Windows.Forms.TabPage();
            this.txtRequestRaw = new System.Windows.Forms.TextBox();
            this.pageResponse = new System.Windows.Forms.TabPage();
            this.tabResponse = new System.Windows.Forms.TabControl();
            this.pageResponseBody = new System.Windows.Forms.TabPage();
            this.cbbEncoding = new System.Windows.Forms.ComboBox();
            this.rtxtResponseBody = new System.Windows.Forms.RichTextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.pageResponseView = new System.Windows.Forms.TabPage();
            this.webResponseView = new System.Windows.Forms.WebBrowser();
            this.pageResponseHeaders = new System.Windows.Forms.TabPage();
            this.gridResponseHeaders = new System.Windows.Forms.DataGridView();
            this.colResponseHeaderName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colResponseHeaderValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pageResponseCookie = new System.Windows.Forms.TabPage();
            this.gridResponseCookies = new System.Windows.Forms.DataGridView();
            this.colResponseCookieName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colResponseCookieValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colResponseCookieDomain = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colResponseCookiePath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colResponseCookieExpires = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colResponseCookieHttpOnly = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colResponseCookieHttps = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtResponseCookies = new System.Windows.Forms.TextBox();
            this.pageResponseRaw = new System.Windows.Forms.TabPage();
            this.txtResponseRaw = new System.Windows.Forms.TextBox();
            this.pageLogs = new System.Windows.Forms.TabPage();
            this.rtxtLogs = new System.Windows.Forms.RichTextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.ckbKeepAlive = new System.Windows.Forms.CheckBox();
            this.panel6 = new System.Windows.Forms.Panel();
            this.ckbKeepCookie = new System.Windows.Forms.CheckBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.numTimeout = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.cbbContentType = new System.Windows.Forms.ComboBox();
            this.btnShowInsertMenu = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.cbbHttpMethod = new System.Windows.Forms.ComboBox();
            this.btnBui = new System.Windows.Forms.Button();
            this.txtURL = new System.Windows.Forms.TextBox();
            this.tipListbox = new System.Windows.Forms.ToolTip(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.acceptToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.texthtmlapplicationxhtmlxmlapplicationxmlq09imagewebpq08ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.acceptEncodingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gzipDeflateSdchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.acceptLanguageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.zhCNzhq08ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.userAgentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mozilla50WindowsNT100WOW64AppleWebKit53736KHTMLLikeGeckoChrome5102704103Safari53736ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panelSide.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.pageHistory.SuspendLayout();
            this.contextMenuStrip3.SuspendLayout();
            this.pageFavorite.SuspendLayout();
            this.contextMenuStrip4.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxHistory)).BeginInit();
            this.tabMain.SuspendLayout();
            this.pageParams.SuspendLayout();
            this.tabRequestData.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridParams)).BeginInit();
            this.contextMenuStrip2.SuspendLayout();
            this.pageCustomRequestData.SuspendLayout();
            this.pageRequest.SuspendLayout();
            this.pageResponse.SuspendLayout();
            this.tabResponse.SuspendLayout();
            this.pageResponseBody.SuspendLayout();
            this.pageResponseView.SuspendLayout();
            this.pageResponseHeaders.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridResponseHeaders)).BeginInit();
            this.pageResponseCookie.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridResponseCookies)).BeginInit();
            this.pageResponseRaw.SuspendLayout();
            this.pageLogs.SuspendLayout();
            this.panel2.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTimeout)).BeginInit();
            this.panel3.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelSide
            // 
            this.panelSide.BackColor = System.Drawing.SystemColors.Control;
            this.panelSide.Controls.Add(this.btnRefresh);
            this.panelSide.Controls.Add(this.tabControl2);
            this.panelSide.Controls.Add(this.panel1);
            this.panelSide.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelSide.Location = new System.Drawing.Point(762, 0);
            this.panelSide.Name = "panelSide";
            this.panelSide.Size = new System.Drawing.Size(200, 566);
            this.panelSide.TabIndex = 0;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(137, 51);
            this.btnRefresh.Margin = new System.Windows.Forms.Padding(2);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(56, 23);
            this.btnRefresh.TabIndex = 5;
            this.btnRefresh.Text = "刷新";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.pageHistory);
            this.tabControl2.Controls.Add(this.pageFavorite);
            this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl2.Location = new System.Drawing.Point(0, 52);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(200, 514);
            this.tabControl2.TabIndex = 3;
            // 
            // pageHistory
            // 
            this.pageHistory.Controls.Add(this.listHistories);
            this.pageHistory.Location = new System.Drawing.Point(4, 22);
            this.pageHistory.Name = "pageHistory";
            this.pageHistory.Padding = new System.Windows.Forms.Padding(3);
            this.pageHistory.Size = new System.Drawing.Size(192, 488);
            this.pageHistory.TabIndex = 0;
            this.pageHistory.Text = "历史";
            this.pageHistory.UseVisualStyleBackColor = true;
            // 
            // listHistories
            // 
            this.listHistories.ContextMenuStrip = this.contextMenuStrip3;
            this.listHistories.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listHistories.FormattingEnabled = true;
            this.listHistories.IntegralHeight = false;
            this.listHistories.ItemHeight = 12;
            this.listHistories.Location = new System.Drawing.Point(3, 3);
            this.listHistories.Name = "listHistories";
            this.listHistories.Size = new System.Drawing.Size(186, 482);
            this.listHistories.TabIndex = 0;
            this.tipListbox.SetToolTip(this.listHistories, "11122233\r\n");
            this.listHistories.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listFavorite_MouseClick);
            this.listHistories.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listHistories_MouseDown);
            this.listHistories.MouseMove += new System.Windows.Forms.MouseEventHandler(this.listHistories_MouseMove);
            // 
            // contextMenuStrip3
            // 
            this.contextMenuStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.收藏ToolStripMenuItem,
            this.删除ToolStripMenuItem});
            this.contextMenuStrip3.Name = "contextMenuStrip3";
            this.contextMenuStrip3.Size = new System.Drawing.Size(101, 48);
            // 
            // 收藏ToolStripMenuItem
            // 
            this.收藏ToolStripMenuItem.Name = "收藏ToolStripMenuItem";
            this.收藏ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.收藏ToolStripMenuItem.Text = "收藏";
            this.收藏ToolStripMenuItem.Click += new System.EventHandler(this.Favoring_Click);
            // 
            // 删除ToolStripMenuItem
            // 
            this.删除ToolStripMenuItem.Name = "删除ToolStripMenuItem";
            this.删除ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.删除ToolStripMenuItem.Text = "删除";
            this.删除ToolStripMenuItem.Click += new System.EventHandler(this.DeleteHistory);
            // 
            // pageFavorite
            // 
            this.pageFavorite.Controls.Add(this.listFavorite);
            this.pageFavorite.Location = new System.Drawing.Point(4, 22);
            this.pageFavorite.Name = "pageFavorite";
            this.pageFavorite.Padding = new System.Windows.Forms.Padding(3);
            this.pageFavorite.Size = new System.Drawing.Size(192, 488);
            this.pageFavorite.TabIndex = 1;
            this.pageFavorite.Text = "收藏";
            this.pageFavorite.UseVisualStyleBackColor = true;
            // 
            // listFavorite
            // 
            this.listFavorite.ContextMenuStrip = this.contextMenuStrip4;
            this.listFavorite.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listFavorite.FormattingEnabled = true;
            this.listFavorite.IntegralHeight = false;
            this.listFavorite.ItemHeight = 12;
            this.listFavorite.Location = new System.Drawing.Point(3, 3);
            this.listFavorite.Name = "listFavorite";
            this.listFavorite.Size = new System.Drawing.Size(186, 482);
            this.listFavorite.TabIndex = 1;
            this.listFavorite.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listFavorite_MouseClick);
            this.listFavorite.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listHistories_MouseDown);
            this.listFavorite.MouseMove += new System.Windows.Forms.MouseEventHandler(this.listHistories_MouseMove);
            // 
            // contextMenuStrip4
            // 
            this.contextMenuStrip4.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip4.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.取消收藏ToolStripMenuItem});
            this.contextMenuStrip4.Name = "contextMenuStrip4";
            this.contextMenuStrip4.Size = new System.Drawing.Size(125, 26);
            // 
            // 取消收藏ToolStripMenuItem
            // 
            this.取消收藏ToolStripMenuItem.Name = "取消收藏ToolStripMenuItem";
            this.取消收藏ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.取消收藏ToolStripMenuItem.Text = "取消收藏";
            this.取消收藏ToolStripMenuItem.Click += new System.EventHandler(this.UnFavoring_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.ckbDock);
            this.panel1.Controls.Add(this.numMaxHistory);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(200, 52);
            this.panel1.TabIndex = 4;
            // 
            // ckbDock
            // 
            this.ckbDock.AutoSize = true;
            this.ckbDock.Location = new System.Drawing.Point(16, 18);
            this.ckbDock.Name = "ckbDock";
            this.ckbDock.Size = new System.Drawing.Size(48, 16);
            this.ckbDock.TabIndex = 0;
            this.ckbDock.Text = "靠左";
            this.ckbDock.UseVisualStyleBackColor = true;
            this.ckbDock.CheckedChanged += new System.EventHandler(this.ckbDock_CheckedChanged);
            // 
            // numMaxHistory
            // 
            this.numMaxHistory.Location = new System.Drawing.Point(145, 17);
            this.numMaxHistory.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.numMaxHistory.Name = "numMaxHistory";
            this.numMaxHistory.Size = new System.Drawing.Size(52, 21);
            this.numMaxHistory.TabIndex = 1;
            this.numMaxHistory.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numMaxHistory.Value = new decimal(new int[] {
            99,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(88, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "列表显示";
            // 
            // tabMain
            // 
            this.tabMain.Controls.Add(this.pageParams);
            this.tabMain.Controls.Add(this.pageRequest);
            this.tabMain.Controls.Add(this.pageResponse);
            this.tabMain.Controls.Add(this.pageLogs);
            this.tabMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabMain.Location = new System.Drawing.Point(0, 102);
            this.tabMain.Name = "tabMain";
            this.tabMain.SelectedIndex = 0;
            this.tabMain.Size = new System.Drawing.Size(762, 464);
            this.tabMain.TabIndex = 8;
            // 
            // pageParams
            // 
            this.pageParams.Controls.Add(this.tabRequestData);
            this.pageParams.Location = new System.Drawing.Point(4, 22);
            this.pageParams.Name = "pageParams";
            this.pageParams.Padding = new System.Windows.Forms.Padding(3);
            this.pageParams.Size = new System.Drawing.Size(754, 438);
            this.pageParams.TabIndex = 0;
            this.pageParams.Text = "Params";
            this.pageParams.UseVisualStyleBackColor = true;
            // 
            // tabRequestData
            // 
            this.tabRequestData.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tabRequestData.Controls.Add(this.tabPage1);
            this.tabRequestData.Controls.Add(this.pageCustomRequestData);
            this.tabRequestData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabRequestData.Location = new System.Drawing.Point(3, 3);
            this.tabRequestData.Name = "tabRequestData";
            this.tabRequestData.SelectedIndex = 0;
            this.tabRequestData.Size = new System.Drawing.Size(748, 432);
            this.tabRequestData.TabIndex = 2;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.gridParams);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(740, 403);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "列表";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // gridParams
            // 
            this.gridParams.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gridParams.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.gridParams.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.gridParams.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridParams.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colParamsName,
            this.colParamsLocation,
            this.colParamsValue,
            this.colRemove});
            this.gridParams.ContextMenuStrip = this.contextMenuStrip2;
            this.gridParams.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridParams.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridParams.Location = new System.Drawing.Point(3, 3);
            this.gridParams.Name = "gridParams";
            this.gridParams.RowTemplate.Height = 23;
            this.gridParams.Size = new System.Drawing.Size(734, 397);
            this.gridParams.TabIndex = 1;
            this.gridParams.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridParams_CellContentClick);
            // 
            // colParamsName
            // 
            this.colParamsName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colParamsName.FillWeight = 20F;
            this.colParamsName.HeaderText = "参数名";
            this.colParamsName.MinimumWidth = 100;
            this.colParamsName.Name = "colParamsName";
            this.colParamsName.Width = 150;
            // 
            // colParamsLocation
            // 
            this.colParamsLocation.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle1.NullValue = "Auto";
            this.colParamsLocation.DefaultCellStyle = dataGridViewCellStyle1;
            this.colParamsLocation.FillWeight = 1F;
            this.colParamsLocation.HeaderText = "参数位置";
            this.colParamsLocation.Items.AddRange(new object[] {
            "Auto",
            "Header",
            "Path",
            "Query",
            "Body"});
            this.colParamsLocation.Name = "colParamsLocation";
            this.colParamsLocation.Width = 80;
            // 
            // colParamsValue
            // 
            this.colParamsValue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colParamsValue.FillWeight = 80F;
            this.colParamsValue.HeaderText = "参数值";
            this.colParamsValue.MinimumWidth = 200;
            this.colParamsValue.Name = "colParamsValue";
            // 
            // colRemove
            // 
            this.colRemove.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.NullValue = "×";
            this.colRemove.DefaultCellStyle = dataGridViewCellStyle2;
            this.colRemove.HeaderText = "";
            this.colRemove.Name = "colRemove";
            this.colRemove.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colRemove.Text = "×";
            this.colRemove.Width = 25;
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.解析Get参数ToolStripMenuItem});
            this.contextMenuStrip2.Name = "contextMenuStrip2";
            this.contextMenuStrip2.Size = new System.Drawing.Size(125, 26);
            // 
            // 解析Get参数ToolStripMenuItem
            // 
            this.解析Get参数ToolStripMenuItem.Name = "解析Get参数ToolStripMenuItem";
            this.解析Get参数ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.解析Get参数ToolStripMenuItem.Text = "解析参数";
            this.解析Get参数ToolStripMenuItem.Click += new System.EventHandler(this.ParseParams_Click);
            // 
            // pageCustomRequestData
            // 
            this.pageCustomRequestData.Controls.Add(this.txtCustomRequestData);
            this.pageCustomRequestData.Location = new System.Drawing.Point(4, 25);
            this.pageCustomRequestData.Name = "pageCustomRequestData";
            this.pageCustomRequestData.Padding = new System.Windows.Forms.Padding(3);
            this.pageCustomRequestData.Size = new System.Drawing.Size(740, 403);
            this.pageCustomRequestData.TabIndex = 1;
            this.pageCustomRequestData.Text = "自定义";
            this.pageCustomRequestData.UseVisualStyleBackColor = true;
            this.pageCustomRequestData.Enter += new System.EventHandler(this.Selectd_CustomPostData);
            // 
            // txtCustomRequestData
            // 
            this.txtCustomRequestData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtCustomRequestData.Location = new System.Drawing.Point(3, 3);
            this.txtCustomRequestData.Multiline = true;
            this.txtCustomRequestData.Name = "txtCustomRequestData";
            this.txtCustomRequestData.Size = new System.Drawing.Size(734, 397);
            this.txtCustomRequestData.TabIndex = 0;
            // 
            // pageRequest
            // 
            this.pageRequest.Controls.Add(this.txtRequestRaw);
            this.pageRequest.Location = new System.Drawing.Point(4, 22);
            this.pageRequest.Name = "pageRequest";
            this.pageRequest.Padding = new System.Windows.Forms.Padding(5);
            this.pageRequest.Size = new System.Drawing.Size(754, 438);
            this.pageRequest.TabIndex = 2;
            this.pageRequest.Text = "Request";
            this.pageRequest.UseVisualStyleBackColor = true;
            // 
            // txtRequestRaw
            // 
            this.txtRequestRaw.BackColor = System.Drawing.SystemColors.Window;
            this.txtRequestRaw.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtRequestRaw.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtRequestRaw.Location = new System.Drawing.Point(5, 5);
            this.txtRequestRaw.Multiline = true;
            this.txtRequestRaw.Name = "txtRequestRaw";
            this.txtRequestRaw.ReadOnly = true;
            this.txtRequestRaw.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtRequestRaw.Size = new System.Drawing.Size(744, 428);
            this.txtRequestRaw.TabIndex = 0;
            this.txtRequestRaw.WordWrap = false;
            // 
            // pageResponse
            // 
            this.pageResponse.Controls.Add(this.tabResponse);
            this.pageResponse.Location = new System.Drawing.Point(4, 22);
            this.pageResponse.Name = "pageResponse";
            this.pageResponse.Size = new System.Drawing.Size(754, 438);
            this.pageResponse.TabIndex = 3;
            this.pageResponse.Text = "Response";
            this.pageResponse.UseVisualStyleBackColor = true;
            // 
            // tabResponse
            // 
            this.tabResponse.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tabResponse.Controls.Add(this.pageResponseBody);
            this.tabResponse.Controls.Add(this.pageResponseView);
            this.tabResponse.Controls.Add(this.pageResponseHeaders);
            this.tabResponse.Controls.Add(this.pageResponseCookie);
            this.tabResponse.Controls.Add(this.pageResponseRaw);
            this.tabResponse.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabResponse.Location = new System.Drawing.Point(0, 0);
            this.tabResponse.Name = "tabResponse";
            this.tabResponse.SelectedIndex = 0;
            this.tabResponse.Size = new System.Drawing.Size(754, 438);
            this.tabResponse.TabIndex = 0;
            // 
            // pageResponseBody
            // 
            this.pageResponseBody.Controls.Add(this.cbbEncoding);
            this.pageResponseBody.Controls.Add(this.rtxtResponseBody);
            this.pageResponseBody.Controls.Add(this.label6);
            this.pageResponseBody.Location = new System.Drawing.Point(4, 25);
            this.pageResponseBody.Name = "pageResponseBody";
            this.pageResponseBody.Padding = new System.Windows.Forms.Padding(5);
            this.pageResponseBody.Size = new System.Drawing.Size(746, 409);
            this.pageResponseBody.TabIndex = 0;
            this.pageResponseBody.Text = "正文内容";
            this.pageResponseBody.UseVisualStyleBackColor = true;
            // 
            // cbbEncoding
            // 
            this.cbbEncoding.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbEncoding.FormattingEnabled = true;
            this.cbbEncoding.Location = new System.Drawing.Point(35, 5);
            this.cbbEncoding.Margin = new System.Windows.Forms.Padding(2);
            this.cbbEncoding.Name = "cbbEncoding";
            this.cbbEncoding.Size = new System.Drawing.Size(112, 20);
            this.cbbEncoding.TabIndex = 1;
            this.cbbEncoding.SelectedIndexChanged += new System.EventHandler(this.cbbEncoding_SelectedIndexChanged);
            // 
            // rtxtResponseBody
            // 
            this.rtxtResponseBody.BackColor = System.Drawing.SystemColors.Window;
            this.rtxtResponseBody.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtxtResponseBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtxtResponseBody.Location = new System.Drawing.Point(5, 26);
            this.rtxtResponseBody.Name = "rtxtResponseBody";
            this.rtxtResponseBody.ReadOnly = true;
            this.rtxtResponseBody.Size = new System.Drawing.Size(736, 378);
            this.rtxtResponseBody.TabIndex = 0;
            this.rtxtResponseBody.Text = "";
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.SystemColors.Control;
            this.label6.Dock = System.Windows.Forms.DockStyle.Top;
            this.label6.Location = new System.Drawing.Point(5, 5);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(736, 21);
            this.label6.TabIndex = 2;
            this.label6.Text = "编码";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pageResponseView
            // 
            this.pageResponseView.Controls.Add(this.webResponseView);
            this.pageResponseView.Location = new System.Drawing.Point(4, 25);
            this.pageResponseView.Name = "pageResponseView";
            this.pageResponseView.Size = new System.Drawing.Size(746, 409);
            this.pageResponseView.TabIndex = 2;
            this.pageResponseView.Text = "视图";
            this.pageResponseView.UseVisualStyleBackColor = true;
            // 
            // webResponseView
            // 
            this.webResponseView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webResponseView.Location = new System.Drawing.Point(0, 0);
            this.webResponseView.MinimumSize = new System.Drawing.Size(20, 20);
            this.webResponseView.Name = "webResponseView";
            this.webResponseView.Size = new System.Drawing.Size(746, 409);
            this.webResponseView.TabIndex = 0;
            // 
            // pageResponseHeaders
            // 
            this.pageResponseHeaders.Controls.Add(this.gridResponseHeaders);
            this.pageResponseHeaders.Location = new System.Drawing.Point(4, 25);
            this.pageResponseHeaders.Name = "pageResponseHeaders";
            this.pageResponseHeaders.Padding = new System.Windows.Forms.Padding(3);
            this.pageResponseHeaders.Size = new System.Drawing.Size(746, 409);
            this.pageResponseHeaders.TabIndex = 1;
            this.pageResponseHeaders.Text = "响应头";
            this.pageResponseHeaders.UseVisualStyleBackColor = true;
            // 
            // gridResponseHeaders
            // 
            this.gridResponseHeaders.AllowUserToAddRows = false;
            this.gridResponseHeaders.AllowUserToDeleteRows = false;
            this.gridResponseHeaders.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gridResponseHeaders.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.gridResponseHeaders.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.gridResponseHeaders.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridResponseHeaders.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colResponseHeaderName,
            this.colResponseHeaderValue});
            this.gridResponseHeaders.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridResponseHeaders.Location = new System.Drawing.Point(3, 3);
            this.gridResponseHeaders.Name = "gridResponseHeaders";
            this.gridResponseHeaders.ReadOnly = true;
            this.gridResponseHeaders.RowHeadersVisible = false;
            this.gridResponseHeaders.RowTemplate.Height = 23;
            this.gridResponseHeaders.Size = new System.Drawing.Size(740, 403);
            this.gridResponseHeaders.TabIndex = 2;
            // 
            // colResponseHeaderName
            // 
            this.colResponseHeaderName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colResponseHeaderName.DataPropertyName = "Name";
            this.colResponseHeaderName.FillWeight = 20F;
            this.colResponseHeaderName.HeaderText = "名称";
            this.colResponseHeaderName.MinimumWidth = 100;
            this.colResponseHeaderName.Name = "colResponseHeaderName";
            this.colResponseHeaderName.ReadOnly = true;
            this.colResponseHeaderName.Width = 150;
            // 
            // colResponseHeaderValue
            // 
            this.colResponseHeaderValue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colResponseHeaderValue.DataPropertyName = "FirstValue";
            this.colResponseHeaderValue.FillWeight = 80F;
            this.colResponseHeaderValue.HeaderText = "值";
            this.colResponseHeaderValue.MinimumWidth = 200;
            this.colResponseHeaderValue.Name = "colResponseHeaderValue";
            this.colResponseHeaderValue.ReadOnly = true;
            // 
            // pageResponseCookie
            // 
            this.pageResponseCookie.Controls.Add(this.gridResponseCookies);
            this.pageResponseCookie.Controls.Add(this.txtResponseCookies);
            this.pageResponseCookie.Location = new System.Drawing.Point(4, 25);
            this.pageResponseCookie.Name = "pageResponseCookie";
            this.pageResponseCookie.Size = new System.Drawing.Size(746, 409);
            this.pageResponseCookie.TabIndex = 3;
            this.pageResponseCookie.Text = "Cookies";
            this.pageResponseCookie.UseVisualStyleBackColor = true;
            // 
            // gridResponseCookies
            // 
            this.gridResponseCookies.AllowUserToAddRows = false;
            this.gridResponseCookies.AllowUserToDeleteRows = false;
            this.gridResponseCookies.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gridResponseCookies.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.gridResponseCookies.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.gridResponseCookies.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridResponseCookies.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colResponseCookieName,
            this.colResponseCookieValue,
            this.colResponseCookieDomain,
            this.colResponseCookiePath,
            this.colResponseCookieExpires,
            this.colResponseCookieHttpOnly,
            this.colResponseCookieHttps});
            this.gridResponseCookies.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridResponseCookies.Location = new System.Drawing.Point(0, 119);
            this.gridResponseCookies.Name = "gridResponseCookies";
            this.gridResponseCookies.ReadOnly = true;
            this.gridResponseCookies.RowHeadersVisible = false;
            this.gridResponseCookies.RowTemplate.Height = 23;
            this.gridResponseCookies.Size = new System.Drawing.Size(746, 290);
            this.gridResponseCookies.TabIndex = 2;
            // 
            // colResponseCookieName
            // 
            this.colResponseCookieName.DataPropertyName = "Name";
            this.colResponseCookieName.HeaderText = "名称";
            this.colResponseCookieName.Name = "colResponseCookieName";
            this.colResponseCookieName.ReadOnly = true;
            // 
            // colResponseCookieValue
            // 
            this.colResponseCookieValue.DataPropertyName = "FirstValue";
            this.colResponseCookieValue.HeaderText = "值";
            this.colResponseCookieValue.Name = "colResponseCookieValue";
            this.colResponseCookieValue.ReadOnly = true;
            // 
            // colResponseCookieDomain
            // 
            this.colResponseCookieDomain.DataPropertyName = "Domain";
            this.colResponseCookieDomain.HeaderText = "域名";
            this.colResponseCookieDomain.Name = "colResponseCookieDomain";
            this.colResponseCookieDomain.ReadOnly = true;
            // 
            // colResponseCookiePath
            // 
            this.colResponseCookiePath.DataPropertyName = "Path";
            this.colResponseCookiePath.HeaderText = "路径";
            this.colResponseCookiePath.Name = "colResponseCookiePath";
            this.colResponseCookiePath.ReadOnly = true;
            // 
            // colResponseCookieExpires
            // 
            this.colResponseCookieExpires.DataPropertyName = "Expires";
            this.colResponseCookieExpires.HeaderText = "过期时间";
            this.colResponseCookieExpires.Name = "colResponseCookieExpires";
            this.colResponseCookieExpires.ReadOnly = true;
            // 
            // colResponseCookieHttpOnly
            // 
            this.colResponseCookieHttpOnly.DataPropertyName = "HttpOnly";
            this.colResponseCookieHttpOnly.HeaderText = "HttpOnly";
            this.colResponseCookieHttpOnly.Name = "colResponseCookieHttpOnly";
            this.colResponseCookieHttpOnly.ReadOnly = true;
            // 
            // colResponseCookieHttps
            // 
            this.colResponseCookieHttps.DataPropertyName = "Secure";
            this.colResponseCookieHttps.HeaderText = "Secure";
            this.colResponseCookieHttps.Name = "colResponseCookieHttps";
            this.colResponseCookieHttps.ReadOnly = true;
            // 
            // txtResponseCookies
            // 
            this.txtResponseCookies.BackColor = System.Drawing.SystemColors.Window;
            this.txtResponseCookies.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtResponseCookies.Location = new System.Drawing.Point(0, 0);
            this.txtResponseCookies.Multiline = true;
            this.txtResponseCookies.Name = "txtResponseCookies";
            this.txtResponseCookies.ReadOnly = true;
            this.txtResponseCookies.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtResponseCookies.Size = new System.Drawing.Size(746, 119);
            this.txtResponseCookies.TabIndex = 0;
            // 
            // pageResponseRaw
            // 
            this.pageResponseRaw.Controls.Add(this.txtResponseRaw);
            this.pageResponseRaw.Location = new System.Drawing.Point(4, 25);
            this.pageResponseRaw.Name = "pageResponseRaw";
            this.pageResponseRaw.Size = new System.Drawing.Size(746, 409);
            this.pageResponseRaw.TabIndex = 4;
            this.pageResponseRaw.Text = "Raw";
            this.pageResponseRaw.UseVisualStyleBackColor = true;
            // 
            // txtResponseRaw
            // 
            this.txtResponseRaw.BackColor = System.Drawing.SystemColors.Window;
            this.txtResponseRaw.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtResponseRaw.Location = new System.Drawing.Point(0, 0);
            this.txtResponseRaw.Multiline = true;
            this.txtResponseRaw.Name = "txtResponseRaw";
            this.txtResponseRaw.ReadOnly = true;
            this.txtResponseRaw.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtResponseRaw.Size = new System.Drawing.Size(746, 409);
            this.txtResponseRaw.TabIndex = 0;
            // 
            // pageLogs
            // 
            this.pageLogs.Controls.Add(this.rtxtLogs);
            this.pageLogs.Location = new System.Drawing.Point(4, 22);
            this.pageLogs.Name = "pageLogs";
            this.pageLogs.Padding = new System.Windows.Forms.Padding(5);
            this.pageLogs.Size = new System.Drawing.Size(754, 438);
            this.pageLogs.TabIndex = 4;
            this.pageLogs.Text = "Logs";
            this.pageLogs.UseVisualStyleBackColor = true;
            // 
            // rtxtLogs
            // 
            this.rtxtLogs.BackColor = System.Drawing.SystemColors.Window;
            this.rtxtLogs.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtxtLogs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtxtLogs.Location = new System.Drawing.Point(5, 5);
            this.rtxtLogs.Name = "rtxtLogs";
            this.rtxtLogs.ReadOnly = true;
            this.rtxtLogs.Size = new System.Drawing.Size(744, 428);
            this.rtxtLogs.TabIndex = 0;
            this.rtxtLogs.Text = "";
            this.rtxtLogs.WordWrap = false;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.flowLayoutPanel1);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.cbbHttpMethod);
            this.panel2.Controls.Add(this.btnBui);
            this.panel2.Controls.Add(this.txtURL);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(762, 102);
            this.panel2.TabIndex = 9;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.panel5);
            this.flowLayoutPanel1.Controls.Add(this.panel6);
            this.flowLayoutPanel1.Controls.Add(this.panel4);
            this.flowLayoutPanel1.Controls.Add(this.panel3);
            this.flowLayoutPanel1.Controls.Add(this.btnShowInsertMenu);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 54);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(2);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Padding = new System.Windows.Forms.Padding(4);
            this.flowLayoutPanel1.Size = new System.Drawing.Size(762, 48);
            this.flowLayoutPanel1.TabIndex = 13;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.ckbKeepAlive);
            this.panel5.Location = new System.Drawing.Point(6, 6);
            this.panel5.Margin = new System.Windows.Forms.Padding(2);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(87, 30);
            this.panel5.TabIndex = 13;
            // 
            // ckbKeepAlive
            // 
            this.ckbKeepAlive.AutoSize = true;
            this.ckbKeepAlive.CheckAlign = System.Drawing.ContentAlignment.BottomRight;
            this.ckbKeepAlive.Checked = true;
            this.ckbKeepAlive.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckbKeepAlive.Location = new System.Drawing.Point(1, 6);
            this.ckbKeepAlive.Name = "ckbKeepAlive";
            this.ckbKeepAlive.Size = new System.Drawing.Size(84, 16);
            this.ckbKeepAlive.TabIndex = 4;
            this.ckbKeepAlive.Text = "Keep-Alive";
            this.ckbKeepAlive.UseVisualStyleBackColor = true;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.ckbKeepCookie);
            this.panel6.Location = new System.Drawing.Point(97, 6);
            this.panel6.Margin = new System.Windows.Forms.Padding(2);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(62, 30);
            this.panel6.TabIndex = 14;
            // 
            // ckbKeepCookie
            // 
            this.ckbKeepCookie.AutoSize = true;
            this.ckbKeepCookie.CheckAlign = System.Drawing.ContentAlignment.BottomRight;
            this.ckbKeepCookie.Location = new System.Drawing.Point(2, 7);
            this.ckbKeepCookie.Name = "ckbKeepCookie";
            this.ckbKeepCookie.Size = new System.Drawing.Size(60, 16);
            this.ckbKeepCookie.TabIndex = 11;
            this.ckbKeepCookie.Text = "Cookie";
            this.ckbKeepCookie.UseVisualStyleBackColor = true;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.numTimeout);
            this.panel4.Controls.Add(this.label4);
            this.panel4.Controls.Add(this.label5);
            this.panel4.Location = new System.Drawing.Point(163, 6);
            this.panel4.Margin = new System.Windows.Forms.Padding(2);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(104, 30);
            this.panel4.TabIndex = 10;
            // 
            // numTimeout
            // 
            this.numTimeout.Location = new System.Drawing.Point(40, 5);
            this.numTimeout.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.numTimeout.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numTimeout.Name = "numTimeout";
            this.numTimeout.Size = new System.Drawing.Size(39, 21);
            this.numTimeout.TabIndex = 8;
            this.numTimeout.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numTimeout.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(4, 8);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 12);
            this.label4.TabIndex = 9;
            this.label4.Text = "超时:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(83, 8);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(17, 12);
            this.label5.TabIndex = 10;
            this.label5.Text = "秒";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.label3);
            this.panel3.Controls.Add(this.cbbContentType);
            this.panel3.Location = new System.Drawing.Point(271, 6);
            this.panel3.Margin = new System.Windows.Forms.Padding(2);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(313, 30);
            this.panel3.TabIndex = 12;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(2, 8);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "Content-Type";
            // 
            // cbbContentType
            // 
            this.cbbContentType.FormattingEnabled = true;
            this.cbbContentType.Location = new System.Drawing.Point(86, 6);
            this.cbbContentType.Name = "cbbContentType";
            this.cbbContentType.Size = new System.Drawing.Size(222, 20);
            this.cbbContentType.TabIndex = 5;
            this.cbbContentType.SelectedIndexChanged += new System.EventHandler(this.cbbContentType_SelectedIndexChanged);
            // 
            // btnShowInsertMenu
            // 
            this.btnShowInsertMenu.Location = new System.Drawing.Point(590, 10);
            this.btnShowInsertMenu.Margin = new System.Windows.Forms.Padding(4, 6, 4, 4);
            this.btnShowInsertMenu.Name = "btnShowInsertMenu";
            this.btnShowInsertMenu.Size = new System.Drawing.Size(75, 23);
            this.btnShowInsertMenu.TabIndex = 7;
            this.btnShowInsertMenu.Text = "插入标准头";
            this.btnShowInsertMenu.UseVisualStyleBackColor = true;
            this.btnShowInsertMenu.Click += new System.EventHandler(this.btnInsertMenu_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 15F);
            this.label1.Location = new System.Drawing.Point(13, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "URL:";
            // 
            // cbbHttpMethod
            // 
            this.cbbHttpMethod.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbbHttpMethod.Font = new System.Drawing.Font("宋体", 16.5F);
            this.cbbHttpMethod.FormattingEnabled = true;
            this.cbbHttpMethod.ItemHeight = 22;
            this.cbbHttpMethod.Location = new System.Drawing.Point(581, 14);
            this.cbbHttpMethod.Name = "cbbHttpMethod";
            this.cbbHttpMethod.Size = new System.Drawing.Size(94, 30);
            this.cbbHttpMethod.TabIndex = 2;
            this.cbbHttpMethod.SelectedIndexChanged += new System.EventHandler(this.cbbHttpMethod_SelectedIndexChanged);
            // 
            // btnBui
            // 
            this.btnBui.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBui.BackColor = System.Drawing.Color.ForestGreen;
            this.btnBui.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBui.Font = new System.Drawing.Font("微软雅黑", 13F);
            this.btnBui.ForeColor = System.Drawing.Color.White;
            this.btnBui.Location = new System.Drawing.Point(680, 14);
            this.btnBui.Name = "btnBui";
            this.btnBui.Size = new System.Drawing.Size(72, 30);
            this.btnBui.TabIndex = 3;
            this.btnBui.Text = "bui~";
            this.btnBui.UseVisualStyleBackColor = false;
            this.btnBui.Click += new System.EventHandler(this.btnBui_Click);
            // 
            // txtURL
            // 
            this.txtURL.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtURL.Font = new System.Drawing.Font("宋体", 15F);
            this.txtURL.Location = new System.Drawing.Point(66, 14);
            this.txtURL.Name = "txtURL";
            this.txtURL.Size = new System.Drawing.Size(510, 30);
            this.txtURL.TabIndex = 1;
            this.txtURL.Text = "http://baidu.com";
            // 
            // tipListbox
            // 
            this.tipListbox.ToolTipTitle = "11111111111";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.acceptToolStripMenuItem,
            this.acceptEncodingToolStripMenuItem,
            this.acceptLanguageToolStripMenuItem,
            this.userAgentToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(178, 92);
            // 
            // acceptToolStripMenuItem
            // 
            this.acceptToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.texthtmlapplicationxhtmlxmlapplicationxmlq09imagewebpq08ToolStripMenuItem});
            this.acceptToolStripMenuItem.Name = "acceptToolStripMenuItem";
            this.acceptToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.acceptToolStripMenuItem.Text = "Accept";
            // 
            // texthtmlapplicationxhtmlxmlapplicationxmlq09imagewebpq08ToolStripMenuItem
            // 
            this.texthtmlapplicationxhtmlxmlapplicationxmlq09imagewebpq08ToolStripMenuItem.Name = "texthtmlapplicationxhtmlxmlapplicationxmlq09imagewebpq08ToolStripMenuItem";
            this.texthtmlapplicationxhtmlxmlapplicationxmlq09imagewebpq08ToolStripMenuItem.Size = new System.Drawing.Size(519, 22);
            this.texthtmlapplicationxhtmlxmlapplicationxmlq09imagewebpq08ToolStripMenuItem.Text = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            // 
            // acceptEncodingToolStripMenuItem
            // 
            this.acceptEncodingToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.gzipDeflateSdchToolStripMenuItem});
            this.acceptEncodingToolStripMenuItem.Name = "acceptEncodingToolStripMenuItem";
            this.acceptEncodingToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.acceptEncodingToolStripMenuItem.Text = "Accept-Encoding";
            // 
            // gzipDeflateSdchToolStripMenuItem
            // 
            this.gzipDeflateSdchToolStripMenuItem.Name = "gzipDeflateSdchToolStripMenuItem";
            this.gzipDeflateSdchToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.gzipDeflateSdchToolStripMenuItem.Text = "gzip, deflate, sdch";
            // 
            // acceptLanguageToolStripMenuItem
            // 
            this.acceptLanguageToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.zhCNzhq08ToolStripMenuItem});
            this.acceptLanguageToolStripMenuItem.Name = "acceptLanguageToolStripMenuItem";
            this.acceptLanguageToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.acceptLanguageToolStripMenuItem.Text = "Accept-Language";
            // 
            // zhCNzhq08ToolStripMenuItem
            // 
            this.zhCNzhq08ToolStripMenuItem.Name = "zhCNzhq08ToolStripMenuItem";
            this.zhCNzhq08ToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.zhCNzhq08ToolStripMenuItem.Text = "zh-CN,zh;q=0.8";
            // 
            // userAgentToolStripMenuItem
            // 
            this.userAgentToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mozilla50WindowsNT100WOW64AppleWebKit53736KHTMLLikeGeckoChrome5102704103Safari53736ToolStripMenuItem});
            this.userAgentToolStripMenuItem.Name = "userAgentToolStripMenuItem";
            this.userAgentToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.userAgentToolStripMenuItem.Text = "User-Agent";
            // 
            // mozilla50WindowsNT100WOW64AppleWebKit53736KHTMLLikeGeckoChrome5102704103Safari53736ToolStripMenuItem
            // 
            this.mozilla50WindowsNT100WOW64AppleWebKit53736KHTMLLikeGeckoChrome5102704103Safari53736ToolStripMenuItem.Name = "mozilla50WindowsNT100WOW64AppleWebKit53736KHTMLLikeGeckoChrome5102704103Safari537" +
    "36ToolStripMenuItem";
            this.mozilla50WindowsNT100WOW64AppleWebKit53736KHTMLLikeGeckoChrome5102704103Safari53736ToolStripMenuItem.Size = new System.Drawing.Size(768, 22);
            this.mozilla50WindowsNT100WOW64AppleWebKit53736KHTMLLikeGeckoChrome5102704103Safari53736ToolStripMenuItem.Text = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrom" +
    "e/51.0.2704.103 Safari/537.36";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(962, 566);
            this.Controls.Add(this.tabMain);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panelSide);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(528, 406);
            this.Name = "Form1";
            this.Text = "Bui~Bui~Api";
            this.panelSide.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.pageHistory.ResumeLayout(false);
            this.contextMenuStrip3.ResumeLayout(false);
            this.pageFavorite.ResumeLayout(false);
            this.contextMenuStrip4.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxHistory)).EndInit();
            this.tabMain.ResumeLayout(false);
            this.pageParams.ResumeLayout(false);
            this.tabRequestData.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridParams)).EndInit();
            this.contextMenuStrip2.ResumeLayout(false);
            this.pageCustomRequestData.ResumeLayout(false);
            this.pageCustomRequestData.PerformLayout();
            this.pageRequest.ResumeLayout(false);
            this.pageRequest.PerformLayout();
            this.pageResponse.ResumeLayout(false);
            this.tabResponse.ResumeLayout(false);
            this.pageResponseBody.ResumeLayout(false);
            this.pageResponseView.ResumeLayout(false);
            this.pageResponseHeaders.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridResponseHeaders)).EndInit();
            this.pageResponseCookie.ResumeLayout(false);
            this.pageResponseCookie.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridResponseCookies)).EndInit();
            this.pageResponseRaw.ResumeLayout(false);
            this.pageResponseRaw.PerformLayout();
            this.pageLogs.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTimeout)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelSide;
        private System.Windows.Forms.CheckBox ckbDock;
        private System.Windows.Forms.TabControl tabMain;
        private System.Windows.Forms.TabPage pageParams;
        private System.Windows.Forms.TabPage pageRequest;
        private System.Windows.Forms.TabPage pageResponse;
        private System.Windows.Forms.TabPage pageLogs;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnBui;
        private System.Windows.Forms.TextBox txtURL;
        private System.Windows.Forms.ComboBox cbbHttpMethod;
        private System.Windows.Forms.NumericUpDown numMaxHistory;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage pageHistory;
        private System.Windows.Forms.TabPage pageFavorite;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ListBox listHistories;
        private System.Windows.Forms.ToolTip tipListbox;
        private System.Windows.Forms.ListBox listFavorite;
        private System.Windows.Forms.DataGridView gridParams;
        private System.Windows.Forms.CheckBox ckbKeepAlive;
        private System.Windows.Forms.ComboBox cbbContentType;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RichTextBox rtxtLogs;
        private System.Windows.Forms.TabControl tabResponse;
        private System.Windows.Forms.TabPage pageResponseBody;
        private System.Windows.Forms.TabPage pageResponseHeaders;
        private System.Windows.Forms.TabPage pageResponseView;
        private System.Windows.Forms.TabPage pageResponseCookie;
        private System.Windows.Forms.TabPage pageResponseRaw;
        private System.Windows.Forms.RichTextBox rtxtResponseBody;
        private System.Windows.Forms.WebBrowser webResponseView;
        private System.Windows.Forms.DataGridView gridResponseHeaders;
        private System.Windows.Forms.TextBox txtResponseCookies;
        private System.Windows.Forms.DataGridView gridResponseCookies;
        private System.Windows.Forms.DataGridViewTextBoxColumn colResponseHeaderName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colResponseHeaderValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn colResponseCookieName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colResponseCookieValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn colResponseCookieDomain;
        private System.Windows.Forms.DataGridViewTextBoxColumn colResponseCookiePath;
        private System.Windows.Forms.DataGridViewTextBoxColumn colResponseCookieExpires;
        private System.Windows.Forms.DataGridViewTextBoxColumn colResponseCookieSize;
        private System.Windows.Forms.DataGridViewTextBoxColumn colResponseCookieHttpOnly;
        private System.Windows.Forms.DataGridViewTextBoxColumn colResponseCookieHttps;
        private System.Windows.Forms.TextBox txtResponseRaw;
        private System.Windows.Forms.Button btnShowInsertMenu;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem acceptToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem texthtmlapplicationxhtmlxmlapplicationxmlq09imagewebpq08ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem acceptEncodingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gzipDeflateSdchToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem userAgentToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mozilla50WindowsNT100WOW64AppleWebKit53736KHTMLLikeGeckoChrome5102704103Safari53736ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem acceptLanguageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem zhCNzhq08ToolStripMenuItem;
        private System.Windows.Forms.TextBox txtRequestRaw;
        private System.Windows.Forms.NumericUpDown numTimeout;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox ckbKeepCookie;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.ToolStripMenuItem 解析Get参数ToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip3;
        private System.Windows.Forms.ToolStripMenuItem 收藏ToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip4;
        private System.Windows.Forms.ToolStripMenuItem 取消收藏ToolStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn colParamsName;
        private System.Windows.Forms.DataGridViewComboBoxColumn colParamsLocation;
        private System.Windows.Forms.DataGridViewTextBoxColumn colParamsValue;
        private System.Windows.Forms.DataGridViewButtonColumn colRemove;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cbbEncoding;
        private System.Windows.Forms.ToolStripMenuItem 删除ToolStripMenuItem;
        private System.Windows.Forms.TabControl tabRequestData;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage pageCustomRequestData;
        private System.Windows.Forms.TextBox txtCustomRequestData;
    }
}

