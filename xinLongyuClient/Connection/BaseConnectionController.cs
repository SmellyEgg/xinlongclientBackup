using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using xinLongyuClient.CommonDictionary;
using xinLongyuClient.CommonFunction;

namespace xinLongyuClient.Connection
{
    public class BaseConnectionController
    {
        /// <summary>
        /// 服务地址，从加密串中解密出来
        /// </summary>
        private string _urlService = string.Empty;

        /// <summary>
        /// json业务层
        /// </summary>
        private JsonController _jsManager;

        public BaseConnectionController()
        {
            _jsManager = new JsonController();
        }

        /// <summary>
        /// 获取服务地址
        /// </summary>
        /// <returns></returns>
        private void GetServiceUrl()
        {
            //这里多了一步加密解密的步骤
            string key = xmlController.GetNodeByXpath(configManagerSection.keyPath, CommonFilePath.CommonconfigFilePath);
            string url = xmlController.GetNodeByXpath(configManagerSection.urlPath, CommonFilePath.CommonconfigFilePath);
            _urlService = SmellyEggCrypt.CryPtService.DESDecrypt(url, key);
        }

        /// <summary>
        /// 传数据
        /// </summary>
        /// <param name="jsonContent"></param>
        /// <returns></returns>
        private string Post(string jsonContent)
        {
            if (string.IsNullOrEmpty(_urlService))
            {
                this.GetServiceUrl();
            }
            WebRequest request = (WebRequest)HttpWebRequest.Create(_urlService);
            request.Method = "POST";
            byte[] postBytes = null;
            request.ContentType = @"application/x-www-form-urlencoded";
            postBytes = Encoding.UTF8.GetBytes(jsonContent);
            request.ContentLength = postBytes.Length;
            request.Timeout = 3000;
            using (Stream outstream = request.GetRequestStream())
            {
                outstream.Write(postBytes, 0, postBytes.Length);
            }
            string result = string.Empty;
            using (WebResponse response = request.GetResponse())
            {
                if (response != null)
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                        {
                            result = reader.ReadToEnd();
                        }
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 调用服务并获取返回结果
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetPostResult(object obj)
        {
            string jsonText = _jsManager.SerializeToJson(obj);
            return this.Post(jsonText);
        }

        /// <summary>
        /// 获取网络图片
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public Image GetWebImage(string url)
        {
            try
            {
                var webClient = new WebClient();
                byte[] imageBytes = webClient.DownloadData(url);
                using (var ms = new MemoryStream(imageBytes))
                {
                    Image image = Image.FromStream(ms);
                    webClient.Dispose();
                    webClient = null;
                    imageBytes = null;
                    return image;
                }
            }
            catch (Exception ex)
            {
                Logging.Error("保存本地图片出错：" + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 测试是否能联通服务
        /// </summary>
        /// <returns></returns>
        public bool TestConnect()
        {
            try
            {
                if (string.IsNullOrEmpty(_urlService))
                {
                    this.GetServiceUrl();
                }
                WebRequest request = (WebRequest)HttpWebRequest.Create(_urlService);
                request.Method = "POST";
                byte[] postBytes = null;
                request.ContentType = @"application/x-www-form-urlencoded";
                postBytes = Encoding.UTF8.GetBytes("");
                request.ContentLength = postBytes.Length;
                using (Stream outstream = request.GetRequestStream())
                {
                }
                return true;
            }
            catch (System.Exception ex)
            {
                string errorCode = "无法连接到远程服务器";
                if (errorCode.Equals(ex.Message))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        public void Dispose()
        {
            if (!object.Equals(_jsManager, null))
            {
                _jsManager = null;
            }
        }

    }
}


