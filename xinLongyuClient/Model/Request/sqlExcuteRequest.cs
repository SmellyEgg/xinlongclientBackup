namespace xinLongyuClient.Model.Request
{
    /// <summary>
    /// sql执行请求类
    /// </summary>
    public class sqlExcuteRequest
    {
        public string sql = string.Empty;

        public string type = string.Empty;

        public sqlExcuteRequest(string newsql, string newtype)
        {
            this.sql = newsql;
            this.type = newtype;
        }
    }
}
