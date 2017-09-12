using Newtonsoft.Json;
using System.Collections.Generic;
using xinLongyuClient.Model.PageInfo;
using xinLongyuClient.Model.Return;

namespace xinLongyuClient.CommonFunction
{
    public class jsonDecoder
    {
        public static BaseReturn DecodeBaseReturnJson(string json)
        {
            BaseReturn m = JsonConvert.DeserializeObject<BaseReturn>(json);
            return m;
        }

        public static pageInfoDetail DecodepageDetailInfo(string json)
        {
            pageInfoDetail m = JsonConvert.DeserializeObject<pageInfoDetail>(json);
            return m;
        }

        public static sqlExcuteReturn DecodeSqlReturn(string json)
        {
            sqlExcuteReturn m = JsonConvert.DeserializeObject<sqlExcuteReturn>(json);
            return m;
        }

        public static Dictionary<string, string>[] DecodeDictionaryArray(string json)
        {
            Dictionary<string, string>[] m = JsonConvert.DeserializeObject<Dictionary<string, string>[]>(json);
            return m;
        }

        public static List<int> DecodeArray(string json)
        {
            List<int> m = JsonConvert.DeserializeObject<List<int>>(json);
            if (object.Equals(m, null))
            {
                m = new List<int>();
            }
            return m;
        }
    }
}
