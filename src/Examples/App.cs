namespace CSharpSDKExamples
{
    /// <summary>
    /// 示例代码中的bucket,saveKey,localFile等请自行设置
    /// </summary>
    class ConsoleDemo
    {
        static void Main(string[] args)
        {
            // 载入密钥设置(AK&SK)		
            Settings.LoadFromFile("D:\\QFL\\qkey.txt");
            //Settings.Load();

            // 上传一个文件
            //UploadDemo.uploadFile();

            // 上传字节数据
            //UploadDemo.uploadData();

            // 上传后触发fop(数据处理)
            //UploadDemo.uploadWithFop();

            // 断点续上传
            UploadDemo.uploadBigFile();

            // 下载文件
            //DownloadDemo.downloadFile();

            // 下载私有文件
            //DownloadDemo.downloadPrivateFile();

            // 空间文件stat
            //BucketDemo.stat();

            // 批量stat
            //BucketDemo.batchStat();

            // 删除空间文件
            //BucketDemo.delete();

            // 批量删除
            //BucketDemo.batchDelete();

            // 复制空间文件
            //BucketDemo.copy();

            // 移动空间文件
            //BucketDemo.move();

            // 修改空间文件的mimeType
            //BucketDemo.chgm();

            // 批量操作
            //BucketDemo.batch();

            // 取回文件并保存到空间
            //BucketDemo.fetch();

            // 镜像资源更新
            //BucketDemo.prefetch();

            // 列举所有bucket
            //BucketDemo.buckets();

            //BucketDemo.bucket();

            // 获取指定bucket的域名
            //BucketDemo.domains();

            // 获取空间文件列表
            //BucketDemo.listFiles();

            // 更新文件的生命周期
            //BucketDemo.updateLifecycle();

            // 文件处理+保存处理结果
            //ProcessingDemo.pfopAndSave();

            // 数据处理dfop:url
            //ProcessingDemo.dfopUrl();

            // 文本处理
            //ProcessingDemo.dfopText();

            // 数据处理dfop:data
            //ProcessingDemo.dfopData();

            // 融合CDN 缓存刷新
            //FusionDemo.refresh();

            // 融合CDN 文件预取
            //FusionDemo.prefetch();

            // 融合CDN 带宽
            //FusionDemo.bandwidth();

            // 融合CDN 流量
            //FusionDemo.flux();

            // 融合CDN 日志查询
            //FusionDemo.loglist();

            System.Console.ReadLine();
        }
    }
}