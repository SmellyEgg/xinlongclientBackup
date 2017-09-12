using System.Windows.Forms;

namespace xinLongyuClient.View.MainForm.ExternalForm
{
    public partial class frmProgress : Form
    {
        public frmProgress()
        {
            InitializeComponent();
        }

        public void SetValue(int value)
        {
            this.SetValueOfProgressBar(this.prg, value);
        }

        public void SetMaxValue(int maxValue)
        {
            this.SetMaxValueOfProgressBar(this.prg, maxValue);
        }

        delegate void delegateSetValueOfProgressBar(ProgressBar prgg, int value);
        public void SetValueOfProgressBar(ProgressBar prgg, int value)
        {
            if (prgg.InvokeRequired)//如果调用控件的线程和创建创建控件的线程不是同一个则为True
            {
                while (!prgg.IsHandleCreated)
                {
                    //解决窗体关闭时出现“访问已释放句柄“的异常
                    if (prgg.Disposing || prgg.IsDisposed)
                        return;
                }
                delegateSetValueOfProgressBar d = new delegateSetValueOfProgressBar(SetValueOfProgressBar);
                prgg.Invoke(d, new object[] { prgg, value });
            }
            else
            {
                prgg.Value = value;
            }
        }

        public void SetMaxValueOfProgressBar(ProgressBar prgg, int value)
        {
            if (prgg.InvokeRequired)//如果调用控件的线程和创建创建控件的线程不是同一个则为True
            {
                while (!prgg.IsHandleCreated)
                {
                    //解决窗体关闭时出现“访问已释放句柄“的异常
                    if (prgg.Disposing || prgg.IsDisposed)
                        return;
                }
                delegateSetValueOfProgressBar d = new delegateSetValueOfProgressBar(SetMaxValueOfProgressBar);
                prgg.Invoke(d, new object[] { prgg, value });
            }
            else
            {
                prgg.Maximum = value;
            }
        }

    }
}
