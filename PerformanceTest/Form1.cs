using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace PerformanceTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            dataGridView1.AutoGenerateColumns = false;
        }


        WebWorks _Works;
        Stopwatch _Sw;
        private void button1_Click(object sender, EventArgs e)
        {
            if (_Works == null)
            {
                _Sw = Stopwatch.StartNew();
                _Works = new WebWorks(new Uri(textBox1.Text), (int)numericUpDown1.Value);
                dataGridView1.DataSource = _Works;
                Task.Run(_Works.Start);
                button1.Text = "停止";
            }
            else
            {
                _Sw.Stop();
                Task.Run(_Works.Stop);
                _Works = null;
                button1.Text = "开始";
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (DateTime.Now.Second % 5 == 0)
            {
                dataGridView1.Refresh();
            }
            if (_Works == null)
            {
                return;
            }
            var total = _Works.TotalCount;
            if (total == 0)
            {
                return;
            }
            this.Text = $"总请求:{total}; 平均每秒:{(int)(total / _Sw.Elapsed.TotalSeconds)}; 失败率:{(_Works.FailCount * 100 / (double)total).ToString("f3")}%; 运行时长:{_Sw.Elapsed.ToString(@"hh\:mm\:ss")}";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView1.Refresh();
        }
    }
}
