using System;
using Qiniu.Common;
using Qiniu.RSF;
using Qiniu.RSF.Model;
using Qiniu.Util;

namespace CSharpSDKExamples
{
    /// <summary>
    /// 数据处理
    /// </summary>
    public class FileProcessing
    {
        /// <summary>
        /// 持久化并保存处理结果
        /// </summary>
        public static void pfopAndSave()
        {
            string bucket = "test";
            string key = "1.avi";
            // 私有队列名称
            string pipeline = "MEDIAPROC_PIPELINE";
            string notifyUrl = "NOTIFY_URL";
            bool force = false;

            string saveAsUri = StringHelper.UrlSafeBase64Encode("<SAVEAS_BUCKET>:<SAVEAS_KEY>");
            string fops = "<FOPS>" + "|saveas/" + saveAsUri;

            Mac mac = new Mac(Settings.AccessKey, Settings.SecretKey);

            FileOpManager fx = new FileOpManager(mac);
            PfopResult result = fx.Pfop(bucket, key, fops, pipeline, notifyUrl, force);

            // 稍后可以根据PersistentId查询处理进度/结果
            //string persistentId = result.PersistentId;
            //Qiniu.Http.HttpResult pr = fx.Prefop(persistentId);
            //System.Console.WriteLine(pr.Code);

            Console.WriteLine(result);

        }

        /// <summary>
        /// dfop形式1:URL
        /// </summary>
        public static void dfopUrl()
        {
            Mac mac = new Mac(Settings.AccessKey, Settings.SecretKey);

            FileOpManager fx = new FileOpManager(mac);

            string fop = "imageView2/0/w/400";
            string url = "http://img.ivsky.com/img/tupian/pre/201610/09/beifang_shanlin_xuejing-001.jpg";

            var result = fx.Dfop(fop, url);

            Console.WriteLine(result);
        }

        /// <summary>
        /// dfop形式2:Data
        /// </summary>
        public static void dfopData()
        {
            Mac mac = new Mac(Settings.AccessKey, Settings.SecretKey);
            FileOpManager fx = new FileOpManager(mac);

            string fop = "imageInfo";
            string localFile= "D:\\QFL\\1.jpg"; 

            var result = fx.Dfop(fop, localFile);

            Console.WriteLine(result);
        }

        /// <summary>
        /// 文本处理
        /// </summary>
        public static void dfopText()
        {
            Mac mac = new Mac(Settings.AccessKey, Settings.SecretKey);
            FileOpManager fx = new FileOpManager(mac);

            string fop = "TEXT_FOP"; // 文本处理fop，例如垃圾文本识别

            // 方式1：从本地文件读取文本内容
            string localFile = "D:\\QFL\\1.jpg";
            var result = fx.DfopText(fop, localFile);

            // 方式2：直接传入文本内容
            //string text = "这是待处理的文本内容";
            //var result = fx.DfopText(fop, text);

            Console.WriteLine(result);
        }

    }
}
