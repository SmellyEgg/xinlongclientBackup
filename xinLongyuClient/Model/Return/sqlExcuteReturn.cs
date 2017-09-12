using System.Collections.Generic;

namespace xinLongyuClient.Model.Return
{
    /// <summary>
    /// sql执行返回类
    /// </summary>
    public class sqlExcuteReturn
    {
        public string error_code = string.Empty;

        public Dictionary<string, string>[] data;

        public string allow_null = string.Empty;

        public string to = string.Empty;
    }
}
