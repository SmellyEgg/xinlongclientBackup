namespace xinLongyuClient.Model.Request
{
    public class CommonLoginRequest
    {
        public string user_name = string.Empty;

        public string password = string.Empty;

        public CommonLoginRequest(string name, string pssword)
        {
            this.user_name = name;
            this.password = pssword;
        }
    }
}
