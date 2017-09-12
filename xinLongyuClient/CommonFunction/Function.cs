using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using xinLongyuClient.View.MainForm.ExternalForm;

namespace xinLongyuClient.CommonFunction
{
    public class Function
    {
        /// <summary>
        /// 根据属性名获取属性值
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static string GetValueByPropertyName(object obj, string propertyName)
        {
            if (!object.Equals(obj.GetType().GetFields().Cast<FieldInfo>().Where(p => propertyName.Equals(p.Name)), null))
            {
                return obj.GetType().GetField(propertyName).GetValue(obj).ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 根据属性名设置属性的值
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static void SetValueByPropertyName(object obj, string propertyName, object value)
        {
            if (!object.Equals(obj.GetType().GetFields().Cast<FieldInfo>().Where(p => propertyName.Equals(p.Name)), null))
            {
                obj.GetType().GetField(propertyName).SetValue(obj, value);
            }
        }


        /// <summary>
        /// 对传入的字符串进行转义
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string[] arrayStringToArray(string text)
        {
            text = text.Replace(System.Environment.NewLine, string.Empty).Trim();
            string arrayString = text.Substring(1, text.Length - 2);
            string[] array = arrayString.Split(',');
            for (int i = 0; i < array.Length; i++)
            {
                //这里需要去除掉双引号
                //array[i] = array[i].Substring(2, array[i].Length - 4);
                string pattern = "\\\"" + "(.+?)" + "\\\"";
                Regex r = new Regex(pattern);
                MatchCollection mc = r.Matches(array[i].Trim());
                array[i] = mc[0].Groups[1].Value;
            }
            return array;
        }

        //进度条窗口的控制
        private static frmProgress _frmProgress = new frmProgress();

        public void SetMaxProgress(int value)
        {
            _frmProgress.SetMaxValue(value);
        }

        public void SetProgressValue(int value)
        {
            _frmProgress.SetValue(value);
        }

        public void ShowProgressForm()
        {
            _frmProgress.Show();
        }

        public void HideProgressForm()
        {
            _frmProgress.Hide();
        }
    }
}
