using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using xinLongyuClient.CommonFunction;
using xinLongyuClient.Decoder;
using xinLongyuClient.Model.DecoderModel;
using xinLongyuClient.Model.Return;

namespace xinLongyuClient.Interface
{
    /// <summary>
    /// 控件的统一接口
    /// 所有控件都需要实现这个接口
    /// </summary>
    public interface IControl
    {
    }

    /// <summary>
    /// 这里是需要给子类的扩展方法
    /// </summary>
    public static class IControlAddtion
    {
        //属性设置
        public static void SetWidth<T>(this T inControl, int width) where T : IControl
        {
            if (width != -1)
            {
                (inControl as Control).Width = width;
            }
        }

        public static void SetHeight<T>(this T inControl, int height) where T : IControl
        {
            (inControl as Control).Height = height;
        }

        /// <summary>
        /// 坐标
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inControl"></param>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        public static void SetLocation<T>(this T inControl, int X, int Y) where T : IControl
        {
            (inControl as Control).Location = new Point(X, Y);
        }

        /// <summary>
        /// 设置D5属性，一般是行高属性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inControl"></param>
        /// <param name="text"></param>
        public static void SetD5Property<T>(this T inControl, string text) where T : IControl
        {
            ExcuteClassMethodByname(inControl, "SetD5Property", text);
        }

        /// <summary>
        /// 设置字体大小
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inControl"></param>
        /// <param name="text"></param>
        public static void SetFontSize<T>(this T inControl, string text) where T : IControl
        {
            if (!string.IsNullOrEmpty(text) && xinLongyuConverter.StringToInt(text) != -1)
            {
                (inControl as Control).Font = new Font((inControl as Control).Font.FontFamily, xinLongyuConverter.StringToInt(text));
            }
        }

        /// <summary>
        /// 设置字体颜色
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inControl"></param>
        /// <param name="text"></param>
        public static void SetFontColor<T>(this T inControl, string text) where T : IControl
        {
            (inControl as Control).ForeColor = ColorTranslator.FromHtml(text);
        }

        /// <summary>
        /// 设置背景颜色
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inControl"></param>
        /// <param name="text"></param>
        public static void SetBackGroundColor<T>(this T inControl, string text) where T : IControl
        {
            (inControl as Control).BackColor = ColorTranslator.FromHtml(text);
        }

        /// <summary>
        /// 字体对齐方式
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inControl"></param>
        /// <param name="text"></param>
        public static void SetAlignment<T>(this T inControl, string text) where T : IControl
        {
            ExcuteClassMethodByname(inControl, "SetAlignment", text);
        }

        /// <summary>
        /// 选中时背景颜色
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inControl"></param>
        /// <param name="text"></param>
        public static void SetActiveBackgroundColorAndFontColor<T>(this T inControl, string backcolorText, string fontColortext) where T : IControl
        {
            Color originalColor = (inControl as Control).BackColor;
            Color originalFont = (inControl as Control).ForeColor;
            (inControl as Control).GotFocus += (s, e) => { (inControl as Control).BackColor = ColorTranslator.FromHtml(backcolorText);
                (inControl as Control).ForeColor = ColorTranslator.FromHtml(fontColortext);
            };
            (inControl as Control).LostFocus += (s, e) => { (inControl as Control).BackColor = originalColor;
                (inControl as Control).ForeColor = originalFont;
            };
        }

        /// <summary>
        /// 是否自适应大小
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inControl"></param>
        /// <param name="text"></param>
        public static void SetAutoSize<T>(this T inControl, string text) where T : IControl
        {
            ExcuteClassMethodByname(inControl, "SetAutoSize", text);
        }

        /// <summary>
        /// 可见性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inControl"></param>
        /// <param name="text"></param>
        public static void SetVisibility<T>(this T inControl, string text) where T : IControl
        {
            (inControl as Control).Visible = xinLongyuConverter.StringToBool(text);
        }


        /// <summary>
        /// 设置控件的值
        /// 这个是父类的方法，如果是图片控件什么的，需要自己重新重写一下这个方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inControl"></param>
        /// <param name="text"></param>
        public static void SetValue<T>(this T inControl, string text) where T : IControl
        {
            //痛苦，这里需要用到反射执行控件的所有方法
            if (ExcuteClassMethodByname(inControl, "SetValue", text) == ReturnConst.CancleReturn)
            {
                (inControl as Control).Text = text;
            }
        }

        /// <summary>
        /// 根据方法名调用类里面的方法
        /// </summary>
        /// <param name="inControl"></param>
        /// <param name="methodName"></param>
        /// <param name="text"></param>
        private static int ExcuteClassMethodByname(object inControl, string methodName, object text)
        {
            MethodInfo[] methodArray = inControl.GetType().GetMethods();
            if (!object.Equals(methodArray, null))
            {
                List<MethodInfo> listmethod = new List<MethodInfo>();
                listmethod.AddRange(methodArray);
                int methodIndex = listmethod.FindIndex(p => methodName.Equals(p.Name));
                if (methodIndex != -1)
                {
                    inControl.GetType().GetMethod(methodName).Invoke(inControl, new[] { text });
                    return ReturnConst.OkReturn;
                }
            }
            return ReturnConst.CancleReturn;
        }

        //触发设置

        /// <summary>
        /// 单击事件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inControl"></param>
        /// <param name="eventText"></param>
        public static void SetClickEvent<T>(this T inControl, string text) where T : IControl
        {
            if (ExcuteClassMethodByname(inControl, "SetClickEvent", text) == ReturnConst.CancleReturn)
            {
                if (!string.IsNullOrEmpty(text))
                {
                    DecoderAssistant.SetControlClickEvent(inControl, text);
                }
            }
        }

        /// <summary>
        /// 双击事件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inControl"></param>
        /// <param name="eventText"></param>
        public static void SetDoubleClickEvent<T>(this T inControl, string eventText) where T : IControl
        {
            if (ExcuteClassMethodByname(inControl, "SetDoubleClickEvent", eventText) == ReturnConst.CancleReturn)
            {
                (inControl as Control).DoubleClick += (s, e) => { };
            }
        }

        /// <summary>
        /// 长按事件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inControl"></param>
        /// <param name="eventText"></param>
        public static void SetLongClickEvent<T>(this T inControl, string eventText) where T : IControl
        {
            if (ExcuteClassMethodByname(inControl, "SetLongClickEvent", eventText) == ReturnConst.CancleReturn)
            {
                //

            }
        }

        /// <summary>
        /// 主值改变事件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inControl"></param>
        /// <param name="eventText"></param>
        public static void SetKeyChangeEvent<T>(this T inControl, string eventText) where T : IControl
        {
            if (ExcuteClassMethodByname(inControl, "SetKeyChangeEvent", eventText) == ReturnConst.CancleReturn)
            {
                //

            }
        }

        /// <summary>
        /// 数据获取事件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inControl"></param>
        /// <param name="eventText"></param>
        public static void SetGetDataEvent<T>(this T inControl, string eventText) where T : IControl
        {
            if (ExcuteClassMethodByname(inControl, "SetGetDataEvent", eventText) == ReturnConst.CancleReturn)
            {
                //

            }
        }

        /// <summary>
        /// 获取焦点事件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inControl"></param>
        /// <param name="eventText"></param>
        public static void SetGetFocusEvent<T>(this T inControl, string eventText) where T : IControl
        {
            if (ExcuteClassMethodByname(inControl, "SetGetFocusEvent", eventText) == ReturnConst.CancleReturn)
            {
                //

            }
        }

        /// <summary>
        /// 失去焦点事件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inControl"></param>
        /// <param name="eventText"></param>
        public static void SetLoseFocusEvent<T>(this T inControl, string eventText) where T : IControl
        {
            if (ExcuteClassMethodByname(inControl, "SetLoseFocusEvent", eventText) == ReturnConst.CancleReturn)
            {
                //

            }
        }

        /// <summary>
        /// 页面加载事件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inControl"></param>
        /// <param name="eventText"></param>
        public static void SetPageInitEvent<T>(this T inControl, string eventText) where T : IControl
        {
            if (ExcuteClassMethodByname(inControl, "SetPageInitEvent", eventText) == ReturnConst.CancleReturn)
            {
                //

            }
        }

        /// <summary>
        /// 页面消失事件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inControl"></param>
        /// <param name="eventText"></param>
        public static void SetPageDisappearEvent<T>(this T inControl, string eventText) where T : IControl
        {
            if (ExcuteClassMethodByname(inControl, "SetPageDisappearEvent", eventText) == ReturnConst.CancleReturn)
            {
                //

            }
        }

        /// <summary>
        /// 触发后触发的事件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inControl"></param>
        /// <param name="eventText"></param>
        public static void SetFinishedEvent<T>(this T inControl, string eventText) where T : IControl
        {
            if (ExcuteClassMethodByname(inControl, "SetFinishedEvent", eventText) == ReturnConst.CancleReturn)
            {

            }
        }

        //方法

        /// <summary>
        /// 刷新
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inControl"></param>
        public static void Refresh<T>(this T inControl, string inText) where T : IControl
        {
            (inControl as Control).Refresh();
        }

        /// <summary>
        /// 启动方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inControl"></param>
        public static void Start<T>(this T inControl, string inText) where T : IControl
        {
            
        }

        /// <summary>
        /// 设置A5事件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inControl"></param>
        /// <param name="inText"></param>
        public static void SetA5<T>(this T inControl, string inText) where T : IControl
        {
            if (ExcuteClassMethodByname(inControl, "SetA5", inText) == ReturnConst.CancleReturn)
            {
                //
            }
        }

    }
}
