using System;
using System.Collections.Generic;
using System.Windows.Forms;
using xinLongyuClient.Connection;
using xinLongyuClient.Decoder;
using xinLongyuClient.Model.PageInfo;
using xinLongyuClient.View.MainForm.ExternalForm;

namespace xinLongyuClient.View.MainForm
{
    public partial class frmMain : Form
    {
        /// <summary>
        /// 第一个页面ID
        /// </summary>
        private int _firstPageId = 1001;
        /// <summary>
        /// 连接逻辑层
        /// </summary>
        private ConnectionManager _conController;
        /// <summary>
        /// 各个页面的信息集合
        /// </summary>
        private List<pageInfoDetail> _listPageInfo;

        //private bool isCompleted = false;

        private pageDecoder _pageDecoder;

        private frmWaiting progressBarForm;
        public frmMain()
        {
            InitializeComponent();
            _conController = new ConnectionManager();
            _listPageInfo = new List<pageInfoDetail>();
            _pageDecoder = new pageDecoder();
            progressBarForm = new frmWaiting();
        }

        private void WaitingForm()
        {
            frmWaiting frm = new frmWaiting();
            frm.ShowDialog();
        }

        /// <summary>
        /// 初始化
        /// 加载第一个页面
        /// </summary>
        private async void Init(Form frm)
        {
            try
            {
                _firstPageId = this.GetPageId();
                if (_firstPageId == -1)
                {
                    this.Close();
                    return;
                }
                Application.DoEvents();
                progressBarForm.Show();
                pageInfoDetail firstPage = await _conController.GetPageInfo(_firstPageId);
                if (!object.Equals(firstPage, null))
                {
                    if (await _pageDecoder.DecodePage(firstPage, frm) == 1)
                    {
                        progressBarForm.HideForm();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("解析页面失败：" + ex.Message);
            }
        }

        /// <summary>
        /// 测试用，根据页面ID加载页面
        /// </summary>
        /// <returns></returns>
        private int GetPageId()
        {
            Form tipForm = new Form();
            tipForm.Text = "测试专用";
            tipForm.StartPosition = FormStartPosition.CenterScreen;
            tipForm.Size = new System.Drawing.Size(400, 200);
            Label lblTips = new Label();
            lblTips.Text = "页面ID：";
            lblTips.Location = new System.Drawing.Point(50, 45);
            lblTips.AutoSize = false;
            lblTips.Size = new System.Drawing.Size(60, 20);

            Button btnOk = new Button();
            btnOk.Text = "确定";
            btnOk.Location = new System.Drawing.Point(90, 90);
            btnOk.Click += (s, e) => { tipForm.DialogResult = DialogResult.OK; };

            Button btnCancel = new Button();
            btnCancel.Text = "取消";
            btnCancel.Location = new System.Drawing.Point(220, 90);
            btnCancel.Click += (s, e) => { tipForm.DialogResult = DialogResult.Cancel; };

            TextBox txtPageId = new TextBox();
            txtPageId.Size = new System.Drawing.Size(200, 30);
            txtPageId.Location = new System.Drawing.Point(120, 40);
            txtPageId.KeyDown += (s, e) => {
                if (e.KeyCode == Keys.Enter)
                {
                    btnOk.PerformClick();
                }
            };

            tipForm.Controls.Add(lblTips);
            tipForm.Controls.Add(txtPageId);
            tipForm.Controls.Add(btnOk);
            tipForm.Controls.Add(btnCancel);

            if (tipForm.ShowDialog() == DialogResult.OK)
            {
                return int.Parse(txtPageId.Text.Trim());
            }
            else
            {
                return -1;
            }
        }

        private Form CreateNewForm()
        {
            Form mainForm = new Form();
            mainForm.Size = new System.Drawing.Size(1366, 768);
            mainForm.StartPosition = FormStartPosition.CenterScreen;
            mainForm.TopMost = true;
            mainForm.Visible = false;
            //mainForm.Show();
            mainForm.Hide();
            return mainForm;
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            
        }

        private void frmMain_Shown(object sender, EventArgs e)
        {
            this.Visible = false;
            //Init(this);
            pageController.CurrentMainForm = this;
            pageController.CreatePage(_firstPageId);
        }
    }
}
