using System.Drawing;
using System.Windows.Forms;
using xinLongyuClient.Connection;
using xinLongyuClient.Interface;

namespace xinLongyuClient.View.CustomControl
{
    /// <summary>
    /// 图片控件类
    /// </summary>
    public class xinlongyuPictureBox : PictureBox, IControl
    {
        /// <summary>
        /// 设置控件的值
        /// 这里由于是图片控件，所以需要重写一下
        /// </summary>
        /// <param name="text"></param>
        public void SetValue(string text)
        {
            this.SizeMode = PictureBoxSizeMode.StretchImage;
            ConnectionManager cm = new ConnectionManager();
            Image img = cm.GetImage(text);
            if (object.Equals(img, null))
            {
                img = Properties.Resources.defaultImg;
            }
            this.Image = img;
        }
    }
}
