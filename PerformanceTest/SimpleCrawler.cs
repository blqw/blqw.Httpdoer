using blqw.Web;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.ComponentModel;

namespace PerformanceTest
{
    /// <summary>
    /// 简单爬虫,获取html 得到所有url 加入队列
    /// </summary>
    public class SimpleCrawler
    {
        static SimpleCrawler()
        {
            RequestPool.Interval = 5000;
        }
        static ConcurrentQueue<Uri> Urls = new ConcurrentQueue<Uri>();

        static readonly HashSet<string> Visited = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        public int Succeed { get; private set; }
        public int Fail { get; private set; }
        public TimeSpan MaxTime { get; private set; }
        public TimeSpan MinTime { get; private set; }
        public TimeSpan AvgTime { get; private set; }
        public int Total { get { return Succeed + Fail; } }
        public bool Runing { get; private set; }

        public string Text1
        {
            get
            {
                return $"{Succeed}/{Fail}/{Total}";
            }
        }
        public string Text2
        {
            get
            {
                return $"{(int)MaxTime.TotalMilliseconds}/{(int)MinTime.TotalMilliseconds}/{(int)AvgTime.TotalMilliseconds}";
            }
        }

        public string Text3
        {
            get
            {
                if (Stopped)
                {
                    if (Runing)
                    {
                        return "正在停止";
                    }
                    else
                    {
                        return "停止";
                    }
                }
                else if (Runing)
                {
                    return "运行中";
                }
                else
                {
                    return "运行完毕";
                }
            }
        }


        public async Task Request()
        {
            try
            {
                Stopped = false;
                Runing = true;
                while (true)
                {
                    string text;
                    while ((text = await GetText()) == null)
                    {
                    }
                    foreach (Match m in HttpExpr.Matches(text))
                    {
                        Uri uri;
                        if (Uri.TryCreate(m.Value, UriKind.Absolute, out uri))
                        {
                            var ext = Path.GetExtension(uri.AbsolutePath)?.ToLowerInvariant();
                            switch (ext)
                            {
                                case ".jpg":
                                case ".gif":
                                case ".png":
                                case ".jpeg":
                                    break;
                                default:
                                    Urls.Enqueue(uri);
                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex);
            }
            finally
            {
                Runing = false;
            }
        }

        private async Task<string> GetText()
        {
            Stopwatch sw = Stopwatch.StartNew();
            try
            {
                var www = new HttpRequest();
                while (true)
                {
                    www.KeepAlive = true;
                    www.BaseUrl = await GetUrl();
                    var html = await www.GetString();
                    sw.Stop();
                    if (sw.Elapsed > MaxTime)
                    {
                        MaxTime = sw.Elapsed;
                    }
                    else if (sw.Elapsed < MinTime || MinTime == default(TimeSpan))
                    {
                        MinTime = sw.Elapsed;
                    }
                    AvgTime = new TimeSpan((sw.Elapsed.Ticks - AvgTime.Ticks) / (Total + 1) + AvgTime.Ticks);
                    Succeed++;
                    if (www.Headers["Content-Type"]?.StartsWith("text/") == true)
                    {
                        return html;
                    }
                }
            }
            catch (TaskCanceledException)
            {
                throw;
            }
            catch (Exception)
            {
                Fail++;
                return null;
            }
        }

        private async Task<Uri> GetUrl()
        {
            Uri uri;
            while (true)
            {
                if (Stopped)
                {
                    throw new TaskCanceledException("任务终止");
                }
                if (Urls.TryDequeue(out uri))
                {
                    var url = uri.GetComponents(UriComponents.HttpRequestUrl, UriFormat.UriEscaped);
                    lock (Visited)
                    {
                        if (Visited.Contains(url))
                        {
                            continue;
                        }
                        Visited.Add(url);
                        return uri;
                    }
                }
                await Task.Delay(10);
            }
        }

        bool Stopped;
        public void Stop()
        {
            Stopped = true;
        }

        public static void FirstUrl(Uri first)
        {
            if (Urls != null)
            {
                Uri uri;
                while (Urls.TryDequeue(out uri))
                {

                }
            }
            Visited.Clear();
            Urls = new ConcurrentQueue<Uri>();
            Urls.Enqueue(first);
        }

        static readonly Regex HttpExpr = new Regex("http[s]?://[a-z0-9.-_/?=&%]+", RegexOptions.Compiled);


    }
}
