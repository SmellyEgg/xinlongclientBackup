using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using xinLongyuClient.CommonFunction;
using xinLongyuClient.Interface;

namespace xinLongyuClient.View.CustomControl
{
    public class xinlongyuRadioButton : Panel, IControl
    {
        private List<string> _currentArray;
        public xinlongyuRadioButton()
        {
            this.AutoScroll = true;
            this.BorderStyle = BorderStyle.FixedSingle;
            _currentArray = new List<string>();
        }

        public void SetValue(string text)
        {
            _currentArray.Clear();
            _currentArray.AddRange(Function.arrayStringToArray(text));
            this.SetValue(_currentArray);
        }

        private void SetValue(List<string> array)
        {
            _currentArray = array;
            Point previousPoint = new Point(10, 10);
            foreach (string str in array)
            {
                RadioButton rdt = new RadioButton();
                rdt.CheckedChanged += Rdt_CheckedChanged;
                rdt.Text = str;

                if (previousPoint.X > this.Location.X + this.Width)
                {
                    previousPoint.X = 10;
                    previousPoint.Y += 20;
                }
                rdt.Location = previousPoint;
                previousPoint.X += rdt.Width;
                this.Controls.Add(rdt);
            }
        }

        private void Rdt_CheckedChanged(object sender, System.EventArgs e)
        {
            if ((sender as RadioButton).Checked)
            {
                foreach (Control ct in this.Controls)
                {
                    if (ct != sender)
                    {
                        (ct as RadioButton).Checked = false;
                    }
                }
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            this.Invalidate();
            this.SetValue(_currentArray);
            base.OnSizeChanged(e);
        }
    }
}
