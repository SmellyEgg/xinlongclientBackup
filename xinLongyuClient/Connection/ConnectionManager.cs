using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using xinLongyuClient.Cacher;
using xinLongyuClient.CommonDictionary;
using xinLongyuClient.CommonFunction;
using xinLongyuClient.Model.PageInfo;
using xinLongyuClient.Model.Request;
using xinLongyuClient.Model.Return;

namespace xinLongyuClient.Connection
{
    /// <summary>
    /// 网络请求逻辑层
    /// </summary>
    class ConnectionManager
    {
        /// <summary>
        /// 基础连接层
        /// </summary>
        private BaseConnectionController _bsConController;

        private pageCacher _pageCacher;

        public ConnectionManager()
        {
            _bsConController = new BaseConnectionController();
            _pageCacher = new pageCacher();
        }

        /// <summary>
        /// sql请求
        /// </summary>
        /// <param name="sql"></param>
        public void GetDataOfSqlRequest(string sql)
        {

        }
        /// <summary>
        /// 请求的账户和密码
        /// </summary>
        private string userName = "admin";
        private string password = "123456";
        /// <summary>
        /// 平台
        /// </summary>
        private string _platForm = "pc";

        /// <summary>
        /// 页面请求
        /// </summary>
        /// <param name="pageId"></param>
        public Task<pageInfoDetail> GetPageInfo(int pageId)
        {
            return Task.Run(() =>
            {
                int timestamp = _pageCacher.GetTimeStampOfPage(pageId);
                string apitype = jsonApiType.page;
                BaseRequest bj = this.GetCommonBaseRequest(apitype);
                pageInfoRequest pagerequeset = new pageInfoRequest(pageId, timestamp, _platForm);
                bj.data = pagerequeset;
                try
                {
                    pageInfoDetail prd;
                    try
                    {
                        string result = _bsConController.GetPostResult(bj);
                        BaseReturn brj = jsonDecoder.DecodeBaseReturnJson(result);
                        prd = jsonDecoder.DecodepageDetailInfo(brj.data.ToString());
                    }
                    catch
                    {
                        //这种情况是连不上服务的处理
                        prd = new pageInfoDetail();
                        prd.data = null;
                    }
                    if (object.Equals(prd.data, null))
                    {
                        return _pageCacher.GetPageInfo(pageId);
                    }
                    else
                    {
                        _pageCacher.CachePageInfo(prd);
                        return prd;
                    }

                }
                catch (Exception ex)
                {
                    Logging.Error(ex.Message);
                    throw ex;
                }
            });
        }

        /// <summary>
        /// sql请求
        /// </summary>
        /// <param name="pageId"></param>
        public sqlExcuteReturn ExcuteSqlWithReturn(string sql)
        {
            string apitype = jsonApiType.execute;
            BaseRequest bj = GetCommonBaseRequest(apitype);
            string type = "db";
            sqlExcuteRequest sr = new sqlExcuteRequest(sql, type);
            bj.data = sr;
            try
            {
                string result = _bsConController.GetPostResult(bj);
                BaseReturn brj = jsonDecoder.DecodeBaseReturnJson(result);
                sqlExcuteReturn ser = jsonDecoder.DecodeSqlReturn(brj.data.ToString());
                //每个字典代表结果集中一行的信息，每个字典中的每个主键，都代表一个列名
                //Dictionary<string, string>[] dicList = jsonDecoder.DecodeDictionaryArray(ser.data.ToString());
                Dictionary<string, string>[] dicList = null;
                return ser;
            }
            catch (Exception ex)
            {
                Logging.Error(ex.Message);
                throw ex;
            }
        }

        /// <summary>
        /// 基本请求
        /// </summary>
        /// <param name="apiType"></param>
        /// <returns></returns>
        private BaseRequest GetCommonBaseRequest(string apiType)
        {
            BaseRequest bj = new BaseRequest(userName, password, apiType);
            return bj;
        }

        /// <summary>
        /// 图片请求
        /// </summary>
        /// <param name="imageUrl"></param>
        /// <returns></returns>
        public Image GetImage(string url)
        {
            //检查传入的值，对于函数来说，所有传入的值都要进行检查
            if (string.IsNullOrEmpty(url))
            {
                return Properties.Resources.defaultImg;
            }
            //这里由于暂时未加域名，所以这里拼上测试域名，后面应该不用做这个操作
            if (!url.StartsWith("http"))
            {
                url = @"http://192.168.1.157" + url;
            }
            //先尝试从本地中获取
            Image image = this.GetLocalImage(url);
            if (!object.Equals(image, null))
            {
                return image;
            }
            //通过服务获取图片
            image = _bsConController.GetWebImage(url);

            if (object.Equals(image, null))
            {
                image = Properties.Resources.defaultImg;
            }
            else
            {
                //这里由于前面的文件流已经被垃圾回收器回收，所以这里需要拷贝然后新建一份
                using (Bitmap bmp = new Bitmap(image))
                {
                    bmp.Save(this.GetFullPathNameForImage(url), ImageFormat.Png);
                }
            }
            return image;
        }

        /// <summary>
        /// 获取本地图片
        /// </summary>
        /// <param name="pageid"></param>
        /// <param name="ctrlid"></param>
        /// <returns>获取到的图片</returns>
        public Image GetLocalImage(string url)
        {
            string filePath = this.GetFullPathNameForImage(url);
            if (File.Exists(filePath))
            {
                return Image.FromFile(filePath);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取图片路径
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private string GetFullPathNameForImage(string url)
        {
            //使用正则去除地址中的特殊符号
            string pattern = @"[^a-z&&^0-9]";
            url = System.Text.RegularExpressions.Regex.Replace(url, pattern, string.Empty);
            string filepath = CommonFilePath.localImageFolder + "\\" + url + ".bmp";
            if (!Directory.Exists(CommonFilePath.localImageFolder))
            {
                Directory.CreateDirectory(CommonFilePath.localImageFolder);
            }
            return filepath;
        }

    }
}
