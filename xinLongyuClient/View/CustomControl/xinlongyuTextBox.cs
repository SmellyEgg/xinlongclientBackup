using System.Windows.Forms;
using xinLongyuClient.Interface;

namespace xinLongyuClient.View.CustomControl
{
    /// <summary>
    /// 简单文本输入框
    /// </summary>
    public class xinlongyuTextBox : TextBox, IControl
    {
        public xinlongyuTextBox()
        {
            this.AutoSize = false;
        }

        /// <summary>
        /// 设置对齐方式
        /// </summary>
        /// <param name="text"></param>
        public void SetAlignment(string text)
        {
            switch (text)
            {
                case "0":
                    this.TextAlign = HorizontalAlignment.Left;
                    break;
                case "1":
                    this.TextAlign = HorizontalAlignment.Center;
                    break;
                case "2":
                    this.TextAlign = HorizontalAlignment.Right;
                    break;
                default:
                    break;
            }
            
        }
    }
}
