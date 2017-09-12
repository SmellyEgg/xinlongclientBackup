using System.Windows.Forms;

namespace xinLongyuClient.CommonFunction
{
    public class delegateForControl
    {
        delegate void SetFormControlCallback(Control ct, Control pnl);

        delegate void SetFormTagCallback( Control pnl, object ct);

        delegate void SetControlLevel(Control ct);

        delegate void delegateShowForm(Form frm);

        delegate void delegateAddColumnForDataGridView(DataGridView dvg, DataGridViewColumn column);

        delegate void delegateAddRowForDataGridView(DataGridView dvg, DataGridViewRow column);
        public void AddControl(Control ct, Control pnl)
        {
            if (pnl.InvokeRequired)//如果调用控件的线程和创建创建控件的线程不是同一个则为True
            {
                //while (!pnl.IsHandleCreated)
                //{
                //    //解决窗体关闭时出现“访问已释放句柄“的异常
                //    if (pnl.Disposing || pnl.IsDisposed)
                //        return;
                //}
                SetFormControlCallback d = new SetFormControlCallback(AddControl);
                pnl.Invoke(d, new object[] { ct, pnl });
            }
            else
            {
                pnl.Controls.Add(ct);
            }
        }

        public void SetCtrlLevel(Control ct)
        {
            if (ct.InvokeRequired)//如果调用控件的线程和创建创建控件的线程不是同一个则为True
            {
                while (!ct.IsHandleCreated)
                {
                    //解决窗体关闭时出现“访问已释放句柄“的异常
                    if (ct.Disposing || ct.IsDisposed)
                        return;
                }
                SetControlLevel d = new SetControlLevel(SetCtrlLevel);
                ct.Invoke(d, new object[] { ct });
            }
            else
            {
                ct.BringToFront();
            }
        }

        public void ShowForm(Form frm)
        {
            if (frm.InvokeRequired)//如果调用控件的线程和创建创建控件的线程不是同一个则为True
            {
                while (!frm.IsHandleCreated)
                {
                    //解决窗体关闭时出现“访问已释放句柄“的异常
                    if (frm.Disposing || frm.IsDisposed)
                        return;
                }
                delegateShowForm d = new delegateShowForm(ShowForm);
                frm.Invoke(d, new object[] { frm });
            }
            else
            {
                //Application.Run(frm);
                frm.Show();
            }
        }

        public void SetTag(Control frm, object tag)
        {
            if (frm.InvokeRequired)//如果调用控件的线程和创建创建控件的线程不是同一个则为True
            {
                while (!frm.IsHandleCreated)
                {
                    //解决窗体关闭时出现“访问已释放句柄“的异常
                    if (frm.Disposing || frm.IsDisposed)
                        return;
                }
                SetFormTagCallback d = new SetFormTagCallback(SetTag);
                frm.Invoke(d, new object[] { frm, tag });
            }
            else
            {
                frm.Tag = tag;
            }
        }

        public void AddColumn(DataGridView frm, DataGridViewColumn column)
        {
            if (frm.InvokeRequired)//如果调用控件的线程和创建创建控件的线程不是同一个则为True
            {
                while (!frm.IsHandleCreated)
                {
                    //解决窗体关闭时出现“访问已释放句柄“的异常
                    if (frm.Disposing || frm.IsDisposed)
                        return;
                }
                delegateAddColumnForDataGridView d = new delegateAddColumnForDataGridView(AddColumn);
                frm.Invoke(d, new object[] { frm, column });
            }
            else
            {
                frm.Columns.Add(column);
            }
        }

        public void AddRow(DataGridView frm, DataGridViewRow row)
        {
            if (frm.InvokeRequired)//如果调用控件的线程和创建创建控件的线程不是同一个则为True
            {
                while (!frm.IsHandleCreated)
                {
                    //解决窗体关闭时出现“访问已释放句柄“的异常
                    if (frm.Disposing || frm.IsDisposed)
                        return;
                }
                delegateAddRowForDataGridView d = new delegateAddRowForDataGridView(AddRow);
                frm.Invoke(d, new object[] { frm, row });
            }
            else
            {
                frm.Rows.Add(row);
            }
        }

    }
}
