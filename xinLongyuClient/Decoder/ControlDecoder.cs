using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using xinLongyuClient.CommonDictionary;
using xinLongyuClient.CommonFunction;
using xinLongyuClient.Interface;
using xinLongyuClient.Model.PageInfo;
using xinLongyuClient.View.CustomControl;

namespace xinLongyuClient.Decoder
{
    public class ControlDecoder
    {
        /// <summary>
        /// 当前窗体
        /// </summary>
        private Control _currentForm;
        /// <summary>
        /// 委托类
        /// </summary>
        private delegateForControl _delegateForControl;
        /// <summary>
        /// 父控件集合
        /// </summary>
        private List<string> _fatherControlList = new List<string>() { xinLongyuControlType.pageType, xinLongyuControlType.navigationBarType, xinLongyuControlType.superViewType
        , xinLongyuControlType.listsType, xinLongyuControlType.bannerType, xinLongyuControlType.cellType, xinLongyuControlType.channerBarType,
        xinLongyuControlType.sectionType, xinLongyuControlType.radioType, xinLongyuControlType.checkboxType, xinLongyuControlType.multilineListType, xinLongyuControlType.PCGrid};

        /// <summary>
        /// D0父控件集合
        /// </summary>
        private List<string> _D0FatherControlList = new List<string>() { xinLongyuControlType.pageType
                ,xinLongyuControlType.superViewType
                ,xinLongyuControlType.navigationBarType};

        public ControlDecoder()
        {
            _delegateForControl = new delegateForControl();
        }
        /// <summary>
        /// 解析控件为界面控件
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public void ProduceControl(ControlDetailForPage obj, List<IControl> listControl, List<ControlDetailForPage> listControlObj, Control frm)
        {
            _currentForm = frm;
            //控件数组一开始进来在这里应该是空的
            if (_fatherControlList.Contains(obj.ctrl_type))
            {
                this.ProduceFatherControl(obj, listControlObj, listControl, _currentForm);
            }
            else
            {
                this.ProductChildControl(obj, listControl, _currentForm);
            }
        }

        //private int _randomIndexForControl = 0;
        /// <summary>
        /// 获取控件接口
        /// </summary>
        /// <param name="controlObj"></param>
        /// <returns></returns>
        private IControl GetIControl(ControlDetailForPage obj)
        {
            IControl control;
            if (xinLongyuControlType.buttonType.Equals(obj.ctrl_type))
            {
                control = new xinlongyuButton();
            }
            else if (xinLongyuControlType.imgType.Equals(obj.ctrl_type))
            {
                control = new xinlongyuPictureBox();
            }
            else if (xinLongyuControlType.inputType.Equals(obj.ctrl_type))
            {
                control = new xinlongyuTextBox();
            }
            else if (xinLongyuControlType.rtfType.Equals(obj.ctrl_type))
            {
                control = new xinlongyuRichTextBox();
            }
            else if (xinLongyuControlType.textType.Equals(obj.ctrl_type))
            {
                control = new xinlongyuLabel();
            }
            else if (xinLongyuControlType.pageType.Equals(obj.ctrl_type))
            {
                control = new xinlongyuPage();
            }
            else if (xinLongyuControlType.switcherType.Equals(obj.ctrl_type))
            {
                control = new xinlongyuSwitcher();
            }
            else if (xinLongyuControlType.seperateLineType.Equals(obj.ctrl_type))
            {
                control = new xinlongyuSeparatorLine();
            }
            else if (xinLongyuControlType.radioType.Equals(obj.ctrl_type))
            {
                control = new xinlongyuRadioButton();
            }
            else if (xinLongyuControlType.checkboxType.Equals(obj.ctrl_type))
            {
                control = new xinlongyuCheckbox();
            }
            else if (_fatherControlList.Contains(obj.ctrl_type))
            {
                control = new fatherControl();
            }
            //没有设定的都先默认使用图片进行显示
            else
            {
                control = new xinlongyuPictureBox();
            }
            (control as Control).Name = obj.ctrl_id.ToString();
            (control as Control).Tag = obj;
            (control as Control).BringToFront();
            return control;
        }

        /// <summary>
        /// 生成子控件
        /// </summary>
        private IControl ProductChildControl(ControlDetailForPage controlObj, List<IControl> listControl, Control fatherControl)
        {
            IControl control = this.GetIControl(controlObj);
            //应该等到所有控件加载完毕之后再一起加载事件
            this.SetControlProperties(control, controlObj);
            //this.SetControlEvent(control, controlObj);
            //
            listControl.Add(control);

            //只有显性控件才进行这两步设置
            if (xinLongyuConverter.StringToBool(controlObj.d18))
            {
                //这里由于是异步，所以使用委托添加到窗体中
                _delegateForControl.AddControl(control as Control, fatherControl);
                //控制层级之间的关系
                //_delegateForControl.SetCtrlLevel(control as Control);
            }
            //
            return control;
        }

        /// <summary>
        /// 生成父控件
        /// </summary>
        private void ProduceFatherControl(ControlDetailForPage controlObj, List<ControlDetailForPage> listControlObj, List<IControl> listControl, Control fatherControl)
        {
            if (controlObj.ctrl_type.Equals(xinLongyuControlType.PCGrid))
            {
                GridViewDecoder gvd = new GridViewDecoder();
                gvd.DecodeGridView(controlObj, listControlObj, listControl, fatherControl);
                return;
            }
            IControl newfatherControl = this.ProductChildControl(controlObj, listControl, fatherControl);
            string controlList = _D0FatherControlList.Contains(controlObj.ctrl_type) ? controlObj.d0 : controlObj.d17;
            List <int> controlIdList = jsonDecoder.DecodeArray(controlList);
            if (controlIdList.Count < 1)
            {
                return;
            }
            //这里对层级进行排序
            List<ControlDetailForPage> childrenList = listControlObj.Where(p => controlIdList.Contains(p.ctrl_id)).OrderBy(p => p.ctrl_level).ToList();
            foreach(ControlDetailForPage ctObj in childrenList)
            {
                //这里使用递归循环生成控件
                if (_fatherControlList.Contains(ctObj.ctrl_type))
                {
                    ProduceFatherControl(ctObj, listControlObj, listControl, newfatherControl as Control);
                }
                else
                {
                    this.ProductChildControl(ctObj, listControl, newfatherControl as Control);
                }
            }
        }

        /// <summary>
        /// 设置控件的属性
        /// </summary>
        /// <param name="control"></param>
        /// <param name="controlObj"></param>
        private void SetControlProperties(IControl control, ControlDetailForPage controlObj)
        {
            //属性这个由于是通用的，所以相当于已经完成了
            //设置控件的主数据
            control.SetValue(controlObj.d0);
            //宽度
            control.SetWidth(controlObj.d1);
            //高度
            control.SetHeight(controlObj.d2);
            //坐标
            control.SetLocation(controlObj.d3, controlObj.d4);
            //行高属性
            control.SetD5Property(controlObj.d5);
            //字体大小
            control.SetFontSize(controlObj.d6);
            //字体颜色
            control.SetFontColor(controlObj.d7);
            //背景颜色
            control.SetBackGroundColor(controlObj.d8);
            //字体对齐方式
            control.SetAlignment(controlObj.d9);
            //设置选中时颜色
            control.SetActiveBackgroundColorAndFontColor(controlObj.d10, controlObj.d11);
            //是否自适应高度
            control.SetAutoSize(controlObj.d12);
            //设置可见性
            control.SetVisibility(controlObj.d13);
            //
        }

        /// <summary>
        /// 设置控件的事件
        /// </summary>
        public void SetControlEvent(IControl control, ControlDetailForPage controlObj)
        {
            //这里也可以设置成通用的，不过需要对语法进行解析
            control.SetClickEvent(controlObj.p0);
            //双击事件
            control.SetDoubleClickEvent(controlObj.p1);
            //设置长按
            control.SetLongClickEvent(controlObj.p2);
            //主键改变事件
            control.SetKeyChangeEvent(controlObj.p3);
            //数据获取事件
            control.SetGetDataEvent(controlObj.p4);
            //获取焦点事件
            control.SetGetFocusEvent(controlObj.p5);
            //失去焦点事件
            control.SetLoseFocusEvent(controlObj.p6);
            //页面加载事件
            control.SetPageInitEvent(controlObj.p7);
            //页面消失事件
            control.SetPageDisappearEvent(controlObj.p8);
            //触发后触发
            control.SetFinishedEvent(controlObj.p9);

        }
    }
}
