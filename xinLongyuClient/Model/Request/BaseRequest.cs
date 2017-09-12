namespace xinLongyuClient.Model.Request
{
    /// <summary>
    /// 请求基类
    /// </summary>
    public class BaseRequest
    {
        public CommonLoginRequest auth;
        public string api_type = string.Empty;
        public object data;

        public BaseRequest(string userName, string password, string apitype)
        {
            auth = new CommonLoginRequest(userName, password);
            this.api_type = apitype;
        }
    }

    
}
