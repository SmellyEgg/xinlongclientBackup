namespace xinLongyuClient.Model.PageInfo
{
    /// <summary>
    /// 页面信息请求类
    /// </summary>
    public class pageInfoRequest
    {
        public int page_id = 0;

        public int time = 0;

        public string page_platform = string.Empty;

        public pageInfoRequest(int id, int time, string platform)
        {
            this.page_id = id;
            this.time = time;
            this.page_platform = platform;
        }
    }
}
