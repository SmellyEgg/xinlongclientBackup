using xinLongyuClient.Interface;
using System.Collections.Generic;
using System.Windows.Forms;
using xinLongyuClient.Model.DecoderModel;
using xinLongyuClient.CommonDictionary;
using xinLongyuClient.CommonFunction;

namespace xinLongyuClient.Decoder
{
    /// <summary>
    /// 解析辅助类
    /// </summary>
    public class DecoderAssistant
    {

        private static List<IControl> _currentControlList;

        public static List<IControl> CurrentControlList
        {
            get
            {
                return _currentControlList;
            }

            set
            {
                _currentControlList = value;
            }
        }

        public static void SetControlList(List<IControl> list)
        {
            _currentControlList = list;
        }

        public static void SetControlClickEvent(IControl target, string text)
        {
            EventDecoder ed = new EventDecoder();
            List<decoderOfControl> listEvent = ed.DecodeEvent(text);

            (target as Control).Click += (s, e) => {
                foreach(decoderOfControl docObj in listEvent)
                {
                    int ctrlIndex = _currentControlList.FindIndex(p => (p as Control).Name.Equals(docObj.CtrlId.ToString()));
                    if (ctrlIndex != -1)
                    {
                        CallMethodByPropertyName(_currentControlList[ctrlIndex], docObj);
                    }
                }
            };

        }

        /// <summary>
        /// 事件里面的方法
        /// </summary>
        /// <param name="control"></param>
        /// <param name="value"></param>
        private static void CallMethodByPropertyName(IControl control, decoderOfControl value)
        {
            if (value.Type.Equals(EventType.SqlType))
            {
                //sql赋值的暂时还不做
                return;
            }
            string controlValue = string.Empty;
            if (value.RightDirectValue is decoderOfControl)
            {

            }
            else
            {
                controlValue = object.Equals(value.RightDirectValue, null) ? string.Empty : value.RightDirectValue.ToString();
            }
            switch (value.LeftCtrlProperty)
            {
                case "d0":
                    control.SetValue(controlValue);
                    break;
                case "d1":
                    control.SetWidth(xinLongyuConverter.StringToInt(controlValue));
                    break;
                case "d2":
                    control.SetHeight(xinLongyuConverter.StringToInt(controlValue));
                    break;
                case "d3":
                    control.SetLocation(xinLongyuConverter.StringToInt(controlValue),(control as Control).Location.Y);
                    break;
                case "d4":
                    control.SetLocation((control as Control).Location.Y, xinLongyuConverter.StringToInt(controlValue));
                    break;
                case "a5":
                    control.SetA5(controlValue);
                    break;
                default:
                    break;
            }
        }

        private string GetValue(decoderOfControl obj)
        {

            return string.Empty;
        }
    }
}
