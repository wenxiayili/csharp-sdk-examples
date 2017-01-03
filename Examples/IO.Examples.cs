using System;
using Qiniu.Common;
using Qiniu.Util;
using Qiniu.IO;
using Qiniu.IO.Model;
using Qiniu.Http;

namespace CSharpSDKExamples
{
    /// <summary>
    /// 文件上传示例-上传小文件
    /// </summary>
    public partial class UploadDemo
    {
        /// <summary>
        /// 简单上传-上传小文件
        /// </summary>
        public static void uploadFile()
        {
            // 生成(上传)凭证时需要使用此Mac
            // 这个示例单独提供了一个Settings类，其中包含AccessKey和SecretKey
            // 实际应用中，请自行设置您的AccessKey和SecretKey
            Mac mac = new Mac(Settings.AccessKey, Settings.SecretKey);

            string bucket = "test";
            string saveKey = "1.png";
            string localFile = "D:\\QFL\\1.png";

            // 上传策略，参见 
            // http://developer.qiniu.com/article/developer/security/put-policy.html
            PutPolicy putPolicy = new PutPolicy();

            // 如果需要设置为"覆盖"上传(如果云端已有同名文件则覆盖)，请使用 SCOPE = "BUCKET:KEY"
            // putPolicy.Scope = bucket + ":" + saveKey;
            putPolicy.Scope = bucket;

            // 上传策略有效期(对应于生成的凭证的有效期)          
            putPolicy.SetExpires(3600);

            // 上传到云端多少天后自动删除该文件，如果不设置（即保持默认默认）则不删除
            putPolicy.DeleteAfterDays = 1;

            // 生成上传凭证，参见
            // http://developer.qiniu.com/article/developer/security/upload-token.html            
            string token = UploadManager.CreateUploadToken(mac, putPolicy);

            SimpleUploader su = new SimpleUploader();

            HttpResult result = su.UploadFile(localFile, saveKey, token);

            Console.WriteLine(result);
        }
    }

    /// <summary>
    /// 文件上传示例-上传后触发数据处理
    /// </summary>
    public partial class UploadDemo
    {
        /// <summary>
        /// 上传完毕后触发fop(数据处理)
        /// </summary>
        public static void uploadWithFop()
        {
            // 生成(上传)凭证时需要使用此Mac
            // 这个示例单独提供了一个Settings类，其中包含AccessKey和SecretKey
            // 实际应用中，请自行设置您的AccessKey和SecretKey
            Mac mac = new Mac(Settings.AccessKey, Settings.SecretKey);

            string bucket = "test";
            string saveKey = "1.jpg";
            string localFile = "D:\\QFL\\1.jpg";

            // 如果想要将处理结果保存到SAVEAS_BUCKET空间下，文件名为SAVEAS_KEY
            // 可以使用savas参数 <FOPS>|saveas/<encodedUri>
            // 根据fop操作不同，上传完毕后云端数据处理可能需要消耗一定的处理时间
            string encodedUri = StringHelper.UrlSafeBase64Encode(bucket + ":" + "1-r-x.jpg");
            // 进行imageView2操作后将结果另存
            string fops = "imageView2/0/w/200|saveas/" + encodedUri;

            PutPolicy putPolicy = new PutPolicy();
            putPolicy.Scope = bucket;
            putPolicy.PersistentOps = fops;
            putPolicy.SetExpires(3600);
            putPolicy.DeleteAfterDays = 1;

            string token = UploadManager.CreateUploadToken(mac, putPolicy);

            SimpleUploader su = new SimpleUploader();

            HttpResult result = su.UploadFile(localFile, saveKey, token);

            Console.WriteLine(result);
        }
    }

    /// <summary>
    /// 文件上传示例-大文件分片/断点续上传
    /// </summary>
    public partial class UploadDemo
    {
        /// <summary>
        /// 上传大文件，支持断点续上传
        /// </summary>
        public static void uploadBigFile()
        {
            Mac mac = new Mac(Settings.AccessKey, Settings.SecretKey);

            string bucket = "test";
            string saveKey = "1.mp4";
            string localFile = "D:\\QFL\\1.mp4";

            // 断点记录文件，可以不用设置，让SDK自动生成，如果出现续上传的情况，SDK会尝试从该文件载入断点记录
            // 对于不同的上传任务，请使用不同的recordFile
            string recordFile = "D:\\QFL\\resume.12345"; 

            PutPolicy putPolicy = new PutPolicy();
            putPolicy.Scope = bucket;
            putPolicy.SetExpires(3600);
            putPolicy.DeleteAfterDays = 1;

            string token = UploadManager.CreateUploadToken(mac, putPolicy);

            // 包含两个参数，并且都有默认值
            // 参数1(bool): uploadFromCDN是否从CDN加速上传，默认否
            // 参数2(enum): chunkUnit上传分片大小，可选值128KB,256KB,512KB,1024KB,2048KB,4096KB
            ResumableUploader ru = new ResumableUploader(false, ChunkUnit.U1024K);

            // ResumableUploader.UploadFile有多种形式，您可以根据需要来选择
            //
            // 最简模式，使用默认recordFile和默认uploadProgressHandler
            // UploadFile(localFile,saveKey,token)
            // 
            // 基本模式，使用默认uploadProgressHandler
            // UploadFile(localFile,saveKey,token,recordFile)
            //
            // 一般模式，使用自定义进度处理(监视上传进度)
            // UploadFile(localFile,saveKey,token,recordFile,uploadProgressHandler)
            //
            // 高级模式，包含上传控制(可控制暂停/继续 或者强制终止)
            // UploadFile(localFile,saveKey,token,recordFile,uploadProgressHandler,uploadController)
            // 
            // 使用默认进度处理，使用自定义上传控制
            HttpResult result = ru.UploadFileAdvanced(localFile, saveKey, token, recordFile, 10, null, new UploadController(uploadControl));

            Console.WriteLine(result);
        }

        // 内部变量，仅作演示
        private static bool paused = false;

        /// <summary>
        /// 上传控制
        /// </summary>
        /// <returns></returns>
        private static UPTS uploadControl()
        {
            // 这个函数只是作为一个演示，实际当中请根据需要来设置
            // 这个演示的实际效果是“走走停停”，也就是停一下又继续，如此重复直至上传结束
            paused = !paused;

            if (paused)
            {
                return UPTS.Suspended;
            }
            else
            {
                return UPTS.Activated;
            }
        }
    }

    /// <summary>
    /// 下载文件-下载公开空间的文件
    /// </summary>
    public partial class DownloadDemo
    {
        /// <summary>
        /// 下载可公开访问的文件(不适用于大文件)
        /// </summary>
        public static void downloadFile()
        {
            // 文件URL
            string rawUrl = "http://img.ivsky.com/img/tupian/pre/201610/09/beifang_shanlin_xuejing-001.jpg";
            // 要保存的文件名
            string saveFile = "D:\\QFL\\saved-snow.jpg";

            // 可公开访问的url，直接下载
            HttpResult result = DownloadManager.Download(rawUrl, saveFile);

            Console.WriteLine(result);
        }
    }

    /// <summary>
    /// 下载文件-下载私有空间中的文件
    /// </summary>
    public partial class DownloadDemo
    {
        /// <summary>
        /// 下载私有空间中的文件(不适用于大文件)
        /// </summary>
        public static void downloadPrivateFile()
        {
            Mac mac = new Mac(Settings.AccessKey, Settings.SecretKey);

            DownloadManager dx = new DownloadManager(mac);

            string rawUrl = "http://your-bucket.bkt.clouddn.com/1.jpg";
            string saveFile = "D:\\QFL\\saved-1.jpg";

            // 设置下载链接有效期3600秒
            string expireAt = StringHelper.CalcUnixTimestamp(3600);

            // 加上过期参数，使用?e=<UnixTimestamp>
            // 如果rawUrl中已包含?，则改用&e=<UnixTimestamp>
            string mid = "?e=";
            if (rawUrl.Contains("?"))
            {
                mid = "&e=";
            }
            string token = dx.CreateDownloadToken(rawUrl + mid + expireAt);
            string accUrl = rawUrl + mid + expireAt + "&token=" + token;

            // 接下来可以使用accUrl来下载文件
            HttpResult result = dx.DownloadPriv(accUrl, saveFile);

            Console.WriteLine(result);
        }
    }
}