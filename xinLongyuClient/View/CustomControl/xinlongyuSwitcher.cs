using System.Drawing;
using System.Windows.Forms;
using xinLongyuClient.Interface;

namespace xinLongyuClient.View.CustomControl
{
    public class xinlongyuSwitcher : Control, IControl
    {
        Image backgroundImage, pressedImage;
        bool pressed = false;

        public xinlongyuSwitcher()
        {
            //this.Size = new Size(164, 80);
            backgroundImage = Properties.Resources.switch_close;
            pressedImage = Properties.Resources.switch_open;
        }

       
        // When the mouse button is pressed, set the "pressed" flag to true  
        // and invalidate the form to cause a repaint.  The .NET Compact Framework  
        // sets the mouse capture automatically. 
        protected override void OnMouseDown(MouseEventArgs e)
        {
            //this.pressed = true;
            this.pressed = !this.pressed;
            this.Invalidate();
            base.OnMouseDown(e);
        }

        // When the mouse is released, reset the "pressed" flag 
        // and invalidate to redraw the button in the unpressed state. 
        protected override void OnMouseUp(MouseEventArgs e)
        {
            //this.pressed = false;
            //this.Invalidate();
            //base.OnMouseUp(e);
        }

        // Override the OnPaint method to draw the background image and the text. 
        protected override void OnPaint(PaintEventArgs e)
        {
            Image targetImage;
            if (this.pressed && this.pressedImage != null)
                targetImage = this.pressedImage;
            ///e.Graphics.DrawImage(this.pressedImage, 0, 0);
            else
                targetImage = this.backgroundImage;
            // e.Graphics.DrawImage(this.backgroundImage, 0, 0);
            System.Drawing.Image resizedImage = new Bitmap(targetImage, new Size(this.Width, this.Height));
            //Rectangle destRect = new Rectangle(this.Location.X, this.Location.Y, this.Width, this.Height);

            // Draw image to screen.
            e.Graphics.DrawImage(resizedImage, 0, 0);

            e.Graphics.DrawRectangle(new Pen(Color.Black), 0, 0,
                this.ClientSize.Width - 1, this.ClientSize.Height - 1);

            base.OnPaint(e);
        }
    }
}
