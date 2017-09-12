using System.Windows.Forms;
using xinLongyuClient.Decoder;
using xinLongyuClient.Interface;

namespace xinLongyuClient.View.CustomControl
{
    public class xinlongyuPage : Panel, IControl
    {
        /// <summary>
        /// 用于跳转页面
        /// </summary>
        /// <param name="inText"></param>
        public void SetA5(string inText)
        {
            int pageId = int.Parse(inText);
            pageController.CreatePage(pageId);
        }
    }
}
