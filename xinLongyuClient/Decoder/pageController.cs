using System;
using System.Collections.Generic;
using System.Windows.Forms;
using xinLongyuClient.Connection;
using xinLongyuClient.Model.PageInfo;
using xinLongyuClient.View.MainForm.ExternalForm;

namespace xinLongyuClient.Decoder
{
    /// <summary>
    /// 窗体页面控制类
    /// 该类为静态类，在使用的时候要很小心
    /// </summary>
    public class pageController
    {
        /// <summary>
        /// 连接逻辑层
        /// </summary>
        private static ConnectionManager _conController = new ConnectionManager();
        /// <summary>
        /// 各个页面的信息集合
        /// </summary>
        private static List<Form> _listPageInfo = new List<Form>();

        /// <summary>
        /// 页面解析类
        /// </summary>
        private static pageDecoder _pageDecoder = new pageDecoder();

        /// <summary>
        /// 进度条窗口
        /// </summary>
        private static frmWaiting progressBarForm = new frmWaiting();

        /// <summary>
        /// 主窗体界面
        /// </summary>
        private static Form _currentMainForm = null;

        public static Form CurrentMainForm
        {
            get
            {
                return _currentMainForm;
            }

            set
            {
                _currentMainForm = value;
            }
        }

        /// <summary>
        /// 创建页面
        /// </summary>
        /// <param name="pageId"></param>
        /// <returns></returns>
        public static int CreatePage(int pageId)
        {
            Form frm = CreateNewForm();
            _listPageInfo.Add(frm);
            //下面为引用部分，地址不会改变
            Application.DoEvents();
            progressBarForm.Show();
            Init(frm, pageId);
            //progressBarForm.HideForm();
            //frm.Show();
            return 1;
        }

        private static Form CreateNewForm()
        {
            Form mainForm = new Form();
            mainForm.Size = new System.Drawing.Size(1366, 768);
            mainForm.StartPosition = FormStartPosition.CenterScreen;
            mainForm.TopMost = true;
            mainForm.Visible = false;
            //mainForm.Show();
            mainForm.Hide();
            mainForm.FormClosed += MainForm_FormClosed;
            return mainForm;
        }

        private static void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            //throw new NotImplementedException();
            //用于退出程序
            _listPageInfo.Remove(sender as Form);
            if (_listPageInfo.Count < 1)
            {
                //尝试释放一些资源，这个应该有助于减少内存占用
                _conController = null;
                _listPageInfo = null;
                _pageDecoder = null;
                progressBarForm.Close();
                progressBarForm.Dispose();
                progressBarForm = null;
                //Application.Exit();
                //这里还是改成走正常窗体的退出流程好一点，这样好像才能正常地释放资源
                _currentMainForm.Close();
                _currentMainForm = null;
            }
        }

        /// <summary>
        /// 初始化
        /// 加载第一个页面
        /// </summary>
        private static async void Init(Form frm, int pageId)
        {
            try
            {
                if (pageId == -1)
                {
                    //return -1;
                    return;
                }
                
                
                pageInfoDetail firstPage = await _conController.GetPageInfo(pageId);
                if (!object.Equals(firstPage, null))
                {
                    if (await _pageDecoder.DecodePage(firstPage, frm) == 1)
                    {
                        //隐藏正在等待的窗口
                        progressBarForm.HideForm();
                        frm.Text = "页面" + "--" + pageId.ToString();
                        frm.Show();
                        return ;
                    }
                }
                return ;
            }
            catch (Exception ex)
            {
                MessageBox.Show("解析页面失败：" + ex.Message);
                return ;
            }
        }

    }
        
}
