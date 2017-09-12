using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using xinLongyuClient.CommonDictionary;
using xinLongyuClient.CommonFunction;
using xinLongyuClient.Interface;
using xinLongyuClient.Model.PageInfo;

namespace xinLongyuClient.Decoder
{
    /// <summary>
    /// 页面解析类
    /// 这个类应该是负责将实体解析为真正的界面控件
    /// </summary>
    public class pageDecoder
    {
        /// <summary>
        /// 控件解析层
        /// </summary>
        private ControlDecoder _controlDecode;

        /// <summary>
        /// 委托帮助类
        /// </summary>
        delegateForControl _dfcHelper;

        public pageDecoder()
        {
            _controlDecode = new ControlDecoder();
            _dfcHelper = new delegateForControl();
        }

        /// <summary>
        /// 解析页面函数
        /// </summary>
        /// <param name="pageObj"></param>
        /// <param name="frm"></param>
        public Task<int> DecodePage(pageInfoDetail pageObj, Form frm)
        {
            return Task.Run(() =>
            {
                //页面基本信息
                pageBaseInfo dtObj = pageObj.data;
                if (object.Equals(dtObj.control_list, null))
                {
                    //当页面中没有控件时，直接跳过(这部分时针对pc的情况)
                    return 0;
                }
                try
                {
                    List<ControlDetailForPage> listControlObject = ControlCaster.CastArrayToControl(dtObj.control_list);
                    //判断一下解析出来的控件数组是不是为空
                    if (object.Equals(listControlObject, null) || listControlObject.Count < 1)
                    {
                        return 0;
                    }
                    //设置tag
                    _dfcHelper.SetTag(frm, pageObj);
                    this.DecodeControl(listControlObject, frm);
                    return 1;
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show("加载页面信息出错:" + ex.Message);
                    Logging.Error(ex.Message);
                    return 0;
                }
            });
        }

        /// <summary>
        /// 解析控件
        /// </summary>
        /// <param name="listControlObject"></param>
        private void DecodeControl(List<ControlDetailForPage> listControlObject, Form frm)
        {
            //对于每个使用的变量都尽可能地先检查一遍
            int pageIndex = listControlObject.FindIndex(p => xinLongyuControlType.pageType.Equals(p.ctrl_type));
            if (pageIndex == -1)
            {
                throw new System.Exception("页面缺少主要控件");
            }

            ControlDetailForPage pageControl = listControlObject[pageIndex];
            frm.Tag = pageControl;
            if (string.IsNullOrEmpty(pageControl.d0))
            {
                return;
            }
            //List<int> controlIdList = jsonDecoder.DecodeArray(pageControl.d0);
            //界面控件数组
            List<IControl> listControl = new List<IControl>();
            //这里过滤出页面的控件并对其根据层级进行排序
            //List<ControlDetailForPage> listControlObjs = listControlObject.Where(p => controlIdList.Contains(p.ctrl_id)).OrderBy(p => p.ctrl_level).ToList();
            //生成page控件
            _controlDecode.ProduceControl(pageControl, listControl, listControlObject, frm);

            //foreach (ControlDetailForPage ctObj in listControlObjs)
            //{
            //    _controlDecode.ProduceControl(ctObj, listControl, listControlObject, frm);
            //}
            //加载事件
            DecoderAssistant.CurrentControlList = listControl;
            if (listControl.Count != 2)
            {
                foreach (IControl control in listControl)
                {
                    _controlDecode.SetControlEvent(control, (control as Control).Tag as ControlDetailForPage);
                }
            }
            //显示窗体
            //_dfcHelper.ShowForm(frm);
        }
    }
}
