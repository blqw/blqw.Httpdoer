using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerformanceTest
{
    public sealed class WebWorks : IListSource
    {
        List<SimpleCrawler> _Works;
        Uri _StartUrl;
        public WebWorks(Uri startUrl, int workCount)
        {
            _Works = Enumerable.Range(0, workCount).Select(it => new SimpleCrawler()).ToList();
            _StartUrl = startUrl;
        }


        public bool ContainsListCollection
        {
            get
            {
                return true;
            }
        }

        public int TotalCount
        {
            get
            {
                return _Works.Sum(it => it.Total);
            }
        }

        public int FailCount
        {
            get
            {
                return _Works.Sum(it => it.Fail);
            }
        }

        public IList GetList()
        {
            return _Works;
        }

        public async Task Start()
        {
            await Task.Yield();
            SimpleCrawler.FirstUrl(_StartUrl);
            Parallel.ForEach(_Works, w => w.Request());
        }

        public async Task Stop()
        {
            await Task.Yield();
            Parallel.ForEach(_Works, w => w.Stop());
        }
    }
}
