
using System;
using System.Drawing;
using System.Windows.Forms;
using xinLongyuClient.Interface;

namespace xinLongyuClient.View.CustomControl
{
    /// <summary>
    /// 分隔线
    /// </summary>
    public class xinlongyuSeparatorLine : Control, IControl
    {
        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.DrawLine(new Pen(Color.Blue),
                new Point((this.ClientSize.Width - Size.Width) / 2, (this.ClientSize.Height - Size.Height) / 2),
                new Point((this.ClientSize.Width - Size.Width) / 2 + this.Width, (this.ClientSize.Height - Size.Height) / 2));
            base.OnPaint(e);
        }

        protected override void OnResize(EventArgs e)
        {
            this.Invalidate();
            base.OnResize(e);
        }
    }
}
