using System.Windows.Forms;
using xinLongyuClient.CommonFunction;
using xinLongyuClient.Interface;
using xinLongyuClient.Model.DecoderModel;

namespace xinLongyuClient.View.CustomControl
{
    /// <summary>
    /// 按钮类
    /// </summary>
    public class xinlongyuButton : Button, IControl
    {

        //public void SetClickEvent(object obj)
        //{
        //    decoderOfControl dcObj = obj as decoderOfControl;
        //    this.Click += (s, e) =>
        //   {
        //       Connection.ConnectionManager cm = new Connection.ConnectionManager();
        //       cm.ExcuteSqlWithReturn("SELECT * FROM `hs_test_app` WHERE app_type = '服务' AND app_verify = 0 GROUP BY subtype to 6");
        //       this.Text = "我被单击了";

        //   };
        //}

        /// <summary>
        /// 设置是否自适应大小
        /// </summary>
        /// <param name="text"></param>
        public void SetAutoSize(string text)
        {
            this.AutoSize = xinLongyuConverter.StringToBool(text);
        }
    }
}
