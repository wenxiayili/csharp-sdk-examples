using System;
using Qiniu.Common;
using Qiniu.CDN;
using Qiniu.CDN.Model;

namespace CSharpSDKExamples
{
    /// <summary>
    /// 融合CDN功能,另请参阅
    /// http://developer.qiniu.com/article/index.html#fusion-api-handbook
    /// </summary>
    public class FusionDemo
    {
        /// <summary>
        /// 缓存刷新
        /// </summary>
        public static void refresh()
        {
            Mac mac = new Mac(Settings.AccessKey, Settings.SecretKey);
            FusionManager fusionMgr = new FusionManager(mac);

            string[] urls = new string[] { "http://yourdomain.bkt.clouddn.com/somefile.php" };
            string[] dirs = new string[] { "http://yourdomain.bkt.clouddn.com/" };
            RefreshRequest request = new RefreshRequest();
            request.AddUrls(urls);
            request.AddDirs(dirs);
            RefreshResult result = fusionMgr.Refresh(request);

            Console.WriteLine(result);
        }

        /// <summary>
        /// 文件预取
        /// </summary>
        public static void prefetch()
        {
            Mac mac = new Mac(Settings.AccessKey, Settings.SecretKey);
            FusionManager fusionMgr = new FusionManager(mac);

            string[] urls = new string[] { "http://yourdomain.clouddn.com/somefile.php" };
            PrefetchRequest request = new PrefetchRequest(urls);
            PrefetchResult result = fusionMgr.Prefetch(request);

            Console.WriteLine(result);
        }

        /// <summary>
        /// 带宽
        /// </summary>
        public static void bandwidth()
        {
            Mac mac = new Mac(Settings.AccessKey, Settings.SecretKey);
            FusionManager fusionMgr = new FusionManager(mac);

            BandwidthRequest request = new BandwidthRequest();
            request.StartDate = "2016-09-01"; 
            request.EndDate = "2016-09-20";
            request.Granularity = "day";
            request.Domains = "yourdomain.bkt.clouddn.com;yourdomain2;yourdomain3";
            BandwidthResult result = fusionMgr.Bandwidth(request);

            Console.WriteLine(result);
        }

        /// <summary>
        /// 流量
        /// </summary>
        public static void flux()
        {
            Mac mac = new Mac(Settings.AccessKey, Settings.SecretKey);
            FusionManager fusionMgr = new FusionManager(mac);

            FluxRequest request = new FluxRequest();
            request.StartDate = "START_DATE"; 
            request.EndDate = "END_DATE"; 
            request.Granularity = "GRANU";
            request.Domains = "DOMAIN1;DOMAIN2"; 
            FluxResult result = fusionMgr.Flux(request);

            Console.WriteLine(result);
        }

        /// <summary>
        /// 日志查询
        /// </summary>
        public static void loglist()
        {
            Mac mac = new Mac(Settings.AccessKey, Settings.SecretKey);
            FusionManager fusionMgr = new FusionManager(mac);

            LogListRequest request = new LogListRequest();
            request.Day = "2016-09-01"; // date:which-day
            request.Domains = "DOMAIN1;DOMAIN2"; // domains
            LogListResult result = fusionMgr.LogList(request);

            Console.WriteLine(result);
        }

        /// <summary>
        /// 时间戳防盗链
        /// </summary>
        public void hotLink()
        {
            Mac mac = new Mac(Settings.AccessKey, Settings.SecretKey);
            FusionManager fusionMgr = new FusionManager(mac);

            HotLinkRequest request = new HotLinkRequest();
            request.Host = "http://your-host";
            request.Path = "/path/";
            request.File = "file-name";
            request.Query = "?version=1.1";
            request.SetLinkExpire(600);

            //request.RawUrl

            string prefLink = fusionMgr.HotLink(request);

            Console.WriteLine(prefLink);
        }
    }
}
