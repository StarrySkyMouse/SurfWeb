using Aliyun.OSS;
using Aliyun.OSS.Common;

namespace Utils.OSS
{
    public class AliyunOSSConfig
    {
        public string AccessKeyId { get; set; }
        public string AccessKeySecret { get; set; }
        public string BucketName { get; set; }
        public string Endpoint { get; set; }
        public string Region { get; set; }
    }

    //文档
    //https://help.aliyun.com/zh/oss/developer-reference/preface-4/?spm=a2c4g.11186623.help-menu-31815.d_5_2_9.f5376250pnCHYz
    public class AliyunOSSUtil
    {
        private AliyunOSSConfig _config;
        public AliyunOSSUtil(AliyunOSSConfig config)
        {
            _config = config;
        }
        /// <summary>
        /// 上传字节流
        /// </summary>
        /// <param name="objectName">OSS中的对象名（路径+文件名）</param>
        /// <param name="bytes">要上传的字节数组</param>
        public void Upload(string objectName, byte[] bytes)
        {
            var conf = new ClientConfiguration();
            conf.SignatureVersion = SignatureVersion.V4;
            var client = new OssClient(_config.Endpoint, _config.AccessKeyId, _config.AccessKeySecret, conf);
            client.SetRegion(_config.Region);
            using var stream = new MemoryStream(bytes);
            try
            {
                client.PutObject(_config.BucketName, objectName, stream);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
