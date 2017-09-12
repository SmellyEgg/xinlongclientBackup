using System.Collections.Generic;
using System.Text.RegularExpressions;
using xinLongyuClient.CommonDictionary;
using xinLongyuClient.CommonFunction;
using xinLongyuClient.Connection;
using xinLongyuClient.Model.DecoderModel;
using xinLongyuClient.Model.Return;

namespace xinLongyuClient.Decoder
{
    /// <summary>
    /// 事件解析类
    /// </summary>
    public class EventDecoder
    {
        /// <summary>
        /// 解析事件数组
        /// </summary>
        /// <param name="inText"></param>
        /// <returns></returns>
        public List<decoderOfControl> DecodeEvent(string inText)
        {
            string[] array = Function.arrayStringToArray(inText);
            List<decoderOfControl> list = new List<decoderOfControl>();
            foreach(string eventText in array)
            {
                list.Add(DecodeNewCharacter(eventText));
            }
            return list;
        }

        public decoderOfControl DecodeNewCharacter(string inText)
        {
            inText = inText.Trim();
            if (inText.StartsWith("SQL:"))
            {
                //sql语法的进行单独处理
                return DecodeSqlEvent(inText);
            }
            //
            decoderOfControl control = new decoderOfControl();
            string judgePattern = @"\d*.a\d\(\d*\)";
            //
            if (!Regex.IsMatch(inText, judgePattern))
            {
                string leftpattern = @"\d\.\w\d\=";
                string rightPattern = @"=.*";
                string leftPart = Regex.Match(inText, leftpattern).Groups[0].ToString();
                string rightPart = Regex.Match(inText, rightPattern).Groups[0].ToString().Remove(0, 1);
                //string rightPart = inText.Substring(leftPart.Length -1, inText.Length - leftPart.Length);
                //排除掉"="号
                leftPart = leftPart.Substring(0, leftPart.Length - 1);
                this.DecodeLeftPart(leftPart, control);
                this.DecodeRightPart(rightPart, control);
            }
            else
            {
                string valuePattern = @"\d*.a\d*\((\d*)\)";
                control.RightDirectValue = Regex.Match(inText, valuePattern).Groups[1].ToString();

                string controlIdPattern = @".a\d*\((\d*)\)";
                control.CtrlId = xinLongyuConverter.StringToInt(inText.Replace(Regex.Match(inText, controlIdPattern).Groups[0].ToString(), string.Empty));
                control.LeftCtrlProperty = Regex.Match(inText, @"\d*.(\w\d*)\(\d*\)").Groups[1].ToString();
            }
            return control;
        }

        /// <summary>
        /// sql语法处理
        /// </summary>
        /// <param name="inText"></param>
        /// <returns></returns>
        private decoderOfControl DecodeSqlEvent(string inText)
        {
            decoderOfControl control = new decoderOfControl();
            control.Type = EventType.SqlType;
            ConnectionManager cm = new ConnectionManager();
            inText = inText.Substring(4, inText.Length - 4);
            sqlExcuteReturn result = cm.ExcuteSqlWithReturn(inText);
            control.CtrlId = xinLongyuConverter.StringToInt(result.to);
            control.LeftCtrlProperty = "d0";
            control.RightDirectValue = result.data;
            return control;
        }

        /// <summary>
        /// 解析左半边部分
        /// </summary>
        /// <param name="leftPart"></param>
        private void DecodeLeftPart(string leftPart, decoderOfControl control)
        {
            string pattern = @"\.[d]\d";
            string propertyPart = Regex.Match(leftPart, pattern).Groups[0].ToString();

            string ctrlId = leftPart.Replace(propertyPart, string.Empty);
            //转为整型
            control.CtrlId = xinLongyuConverter.StringToInt(ctrlId);
            //排除掉"."符号
            control.LeftCtrlProperty = propertyPart.Substring(1, propertyPart.Length - 1);
        }

        /// <summary>
        /// 解析右半边部分
        /// </summary>
        /// <param name="rightPart"></param>
        /// <param name="control"></param>
        private void DecodeRightPart(string rightPart, decoderOfControl control)
        {
            //先看看简单的情况
            if (!rightPart.StartsWith("{"))
            {
                control.RightDirectValue = rightPart;
            }
            else
            {
                //右边也包含控件ID的情况
                string[] rightpart = rightPart.Substring(1, rightPart.Length - 2).Split('.');
                decoderOfControl obj = new decoderOfControl();
                obj.CtrlId = xinLongyuConverter.StringToInt(rightpart[0]);
                obj.RightDirectValue = rightpart[1];
                control.RightDirectValue = obj;
            }
            //if (rightPart.StartsWith("{"))
            //{
            //    string rightpart = rightPart.Substring(1, rightPart.Length - 2);
            //    DecodeRightPart(rightpart, control);
            //}
            //else
            //{

            //}
        }

        
    }
}
