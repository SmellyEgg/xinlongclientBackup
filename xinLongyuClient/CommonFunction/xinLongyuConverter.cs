using System;

namespace xinLongyuClient.CommonFunction
{
    public class xinLongyuConverter
    {
        public static int StringToInt(string str)
        {
            try
            {
                int result = Convert.ToInt32(str);
                return result;
            }
            catch
            {
                return -1;
            }
        }

        public static Boolean StringToBool(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return true;
            }
            else if ("1".Equals(str) || "True".Equals(str) || "true".Equals(str))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
