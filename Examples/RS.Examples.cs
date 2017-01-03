using System;
using Qiniu.Common;
using Qiniu.RS;
using Qiniu.RS.Model;
using Qiniu.Http;

namespace CSharpSDKExamples
{
    /// <summary>
    /// 空间及空间文件管理
    /// </summary>
    public class BucketDemo
    {

        /// <summary>
        /// 空间文件的stat(获取文件基本信息)操作
        /// </summary>
        public static void stat()
        {
            Mac mac = new Mac(Settings.AccessKey, Settings.SecretKey);

            string bucket = "test";
            string key = "1.avi";

            BucketManager bm = new BucketManager(mac);

            StatResult result = bm.Stat(bucket, key);

            Console.WriteLine(result);
        }

        public static void batchStat()
        {
            Mac mac = new Mac(Settings.AccessKey, Settings.SecretKey);

            string bucket = "test";

            int N = 5;

            // 待查询的N个文件keys
            string[] keys = new string[N];
            for (int i = 0; i < N; ++i)
            {
                keys[i] = string.Format("{0:D3}.txt", i + 1);
            }

            BucketManager bm = new BucketManager(mac);

            BatchResult result = bm.BatchStat(bucket, keys);

            Console.WriteLine(result);
        }

        /// <summary>
        /// 删除空间中指定文件
        /// </summary>
        public static void delete()
        {
            Mac mac = new Mac(Settings.AccessKey, Settings.SecretKey);

            string bucket = "test";
            string key = "test.txt";

            BucketManager bm = new BucketManager(mac);

            HttpResult result = bm.Delete(bucket, key);

            Console.WriteLine(result);
        }

        public static void batchDelete()
        {
            Mac mac = new Mac(Settings.AccessKey, Settings.SecretKey);

            string bucket = "test";

            int N = 5;

            // 待删除的N个文件keys
            string[] keys = new string[N];
            for(int i=0;i<N;++i)
            {
                keys[i] = string.Format("{0:D3}.txt", i + 1);
            }

            BucketManager bm = new BucketManager(mac);

            BatchResult result = bm.BatchDelete(bucket, keys);

            Console.WriteLine(result);
        }

        /// <summary>
        /// 复制文件
        /// </summary>
        public static void copy()
        {
            Mac mac = new Mac(Settings.AccessKey, Settings.SecretKey);

            string srcBucket = "test";
            string srcKey = "1.txt";
            string dstBucket = "test";
            string dstKey = "2.txt";

            BucketManager bm = new BucketManager(mac);
            HttpResult result = bm.Copy(srcBucket, srcKey, dstBucket, dstKey);

            //支持force参数, bool force = true/false
            //HttpResult result = bm.Copy(srcBucket, srcKey, dstBucket, dstKey, force);

            Console.WriteLine(result);
        }

        /// <summary>
        /// 移动文件
        /// </summary>
        public static void move()
        {
            Mac mac = new Mac(Settings.AccessKey, Settings.SecretKey);

            string srcBucket = "test";
            string srcKey = "1.txt";
            string dstBucket = "test";
            string dstKey = "2.txt";

            BucketManager bm = new BucketManager(mac);
            HttpResult result = bm.Move(srcBucket, srcKey, dstBucket, dstKey);

            //支持force参数, bool force = true/false
            //HttpResult result = bm.Move(srcBucket, srcKey, dstBucket, dstKey, force);

            Console.WriteLine(result);
        }

        /// <summary>
        /// 修改文件的MIME_TYPE
        /// </summary>
        public static void chgm()
        {
            Mac mac = new Mac(Settings.AccessKey, Settings.SecretKey);

            string bucket = "test";
            string key = "1.txt";
            string mimeType = "text/html";

            BucketManager bm = new BucketManager(mac);
            HttpResult result = bm.Chgm(bucket, key, mimeType);

            Console.WriteLine(result);
        }

        /// <summary>
        /// 拉取资源到空间
        /// </summary>
        public static void fetch()
        {
            Mac mac = new Mac(Settings.AccessKey, Settings.SecretKey);

            string bucket = "test";
            string saveKey = "1.jpg";
            string remoteUrl = "http://remote-url.com/file-name";

            BucketManager bm = new BucketManager(mac);
            HttpResult result = bm.Fetch(remoteUrl, bucket, saveKey);

            Console.WriteLine(result);
        }

        /// <summary>
        /// 对于设置了镜像存储的空间，从镜像源站抓取指定名称的资源并存储到该空间中
        /// </summary>
        public static void prefetch()
        {
            Mac mac = new Mac(Settings.AccessKey, Settings.SecretKey);

            string bucket = "test";
            string key = "1.jpg";

            BucketManager bm = new BucketManager(mac);
            HttpResult result = bm.Prefetch(bucket, key);

            Console.WriteLine(result);
        }

        /// <summary>
        /// 批量操作
        /// </summary>
        public static void batch()
        {
            Mac mac = new Mac(Settings.AccessKey, Settings.SecretKey);

            // 批量操作类似于
            // op=<op1>&op=<op2>&op=<op3>...
            string batchOps = "op=OP1&op=OP2";
            BucketManager bm = new BucketManager(mac);
            var result = bm.Batch(batchOps);
            // 或者
            //string[] batch_ops={"<op1>","<op2>","<op3>",...};
            //bm.Batch(batch_ops);

            Console.WriteLine(result);
        }

        /// <summary>
        /// 列举所有的bucket
        /// </summary>
        public static void buckets()
        {
            Mac mac = new Mac(Settings.AccessKey, Settings.SecretKey);
            BucketManager bm = new BucketManager(mac);
            BucketsResult result = bm.Buckets();

            Console.WriteLine(result);
        }

        /// <summary>
        /// 获取指定bucket对应的域名(可能不止一个),类似于abcxx.bkt.clouddn.com这样
        /// </summary>
        public static void domains()
        {
            Mac mac = new Mac(Settings.AccessKey, Settings.SecretKey);

            string bucket = "test";

            BucketManager bm = new BucketManager(mac);
            DomainsResult result = bm.Domains(bucket);

            Console.WriteLine(result);
        }

        /// <summary>
        /// 获取空间文件列表
        /// 
        /// BucketManager.listFiles(bucket, prefix, marker, limit, delimiter)
        /// 
        /// bucket:    目标空间名称
        /// 
        /// prefix:    返回指定文件名前缀的文件列表(prefix可设为null)
        /// 
        /// marker:    考虑到设置limit后返回的文件列表可能不全(需要重复执行listFiles操作)
        ///            执行listFiles操作时使用marker标记来追加新的结果
        ///            特别注意首次执行listFiles操作时marker为null   
        ///            
        /// limit:     每次返回结果所包含的文件总数限制(limit最大值1000，建议值100) 
        /// 
        /// delimiter: 分隔符，比如-或者/等等，可以模拟作为目录结构(参考下述示例)
        ///            假设指定空间中有2个文件 fakepath/1.txt fakepath/2.txt
        ///            现设置分隔符delimiter = / 得到返回结果items =[]，commonPrefixes = [fakepath/]
        ///            然后调整prefix = fakepath/ delimiter = null 得到所需结果items = [1.txt,2.txt]
        ///            于是可以在本地先创建一个目录fakepath,然后在该目录下写入items中的文件  
        ///            
        /// </summary>
        public static void listFiles()
        {
            Mac mac = new Mac(Settings.AccessKey, Settings.SecretKey);

            string bucket = "test";
            string marker = ""; // 首次请求时marker必须为空
            string prefix = null; // 按文件名前缀保留搜索结果
            string delimiter = null; // 目录分割字符(比如"/")
            int limit = 100; // 单次列举数量限制(最大值为1000)

            BucketManager bm = new BucketManager(mac);
            //List<FileDesc> items = new List<FileDesc>();
            //List<string> commonPrefixes = new List<string>();

            do
            {
                ListResult result = bm.List(bucket, prefix, marker, limit, delimiter);

                Console.WriteLine(result);

                marker = result.Result.Marker;
                
                //if (result.Result.Items != null)
                //{
                //    items.AddRange(result.Result.Items);
                //}

                //if (result.Result.CommonPrefixes != null)
                //{
                //    commonPrefixes.AddRange(result.Result.CommonPrefixes);
                //}

            } while (!string.IsNullOrEmpty(marker));

            //foreach (string cp in commonPrefixes)
            //{
            //    Console.WriteLine(cp);
            //}

            //foreach(var item in items)
            //{
            //    Console.WriteLine(item.Key);
            //}
        }

        /// <summary>
        /// 更新文件的lifecycle
        /// </summary>
        public static void updateLifecycle()
        {
            Mac mac = new Mac(Settings.AccessKey, Settings.SecretKey);

            string bucket = "test";
            string key = "1.txt";

            // 新的deleteAfterDays，设置为0表示取消lifecycle
            int deleteAfterDays = 1;

            BucketManager bm = new BucketManager(mac);
            HttpResult result = bm.UpdateLifecycle(bucket, key, deleteAfterDays);

            Console.WriteLine(result);
        }
    }
}