using System.Windows.Forms;
using xinLongyuClient.Interface;

namespace xinLongyuClient.View.CustomControl
{
    /// <summary>
    /// 富文本输入框
    /// </summary>
    public class xinlongyuRichTextBox : RichTextBox, IControl
    {
        /// <summary>
        /// 设置字体对齐方式
        /// </summary>
        /// <param name="text"></param>
        public void SetAlignment(string text)
        {
            switch (text)
            {
                case "0":
                    this.SelectionAlignment = HorizontalAlignment.Left;
                    break;
                case "1":
                    this.SelectionAlignment = HorizontalAlignment.Center;
                    break;
                case "2":
                    this.SelectionAlignment = HorizontalAlignment.Right;
                    break;
                default:
                    break;
            }
        }


    }
}
