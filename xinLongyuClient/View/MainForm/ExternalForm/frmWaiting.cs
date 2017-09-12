
using System.Windows.Forms;

namespace xinLongyuClient.View.MainForm.ExternalForm
{
    public partial class frmWaiting : Form
    {
        public frmWaiting()
        {
            InitializeComponent();
        }

        public void HideForm()
        {
            this.Invoke((MethodInvoker)delegate
            {
                this.Hide();
            });
        }

        public void ShowForm()
        {
            this.Invoke((MethodInvoker)delegate
            {
                this.Show();
            });
        }

        //public void UpdateProgressBar(int index, int value)
        //{
        //    this.Invoke((MethodInvoker)delegate {
        //        if (index == 1)
        //        {
        //            prg.Value = value;
        //        }
        //        else
        //        {
        //            prg.Value = value;
        //        }
        //    });
        //}
    }
}
