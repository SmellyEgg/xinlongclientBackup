using System.Windows.Forms;
using xinLongyuClient.Interface;

namespace xinLongyuClient.View.CustomControl
{
    public class fatherControl : Panel, IControl
    {
        public fatherControl()
        {
            this.BorderStyle = BorderStyle.FixedSingle;
        }
    }
}
