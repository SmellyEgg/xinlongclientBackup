using System.Collections.Generic;
using xinLongyuClient.CommonFunction;
using xinLongyuClient.Model.PageInfo;

namespace xinLongyuClient.Decoder
{
    public class ControlCaster
    {
        #region 对象数组转为实体
        public static List<ControlDetailForPage> CastArrayToControl(object[][] objArray)
        {
            List<ControlDetailForPage> controlst = new List<ControlDetailForPage>();
            for (int i = 0; i < objArray.Length; i++)
            {
                ControlDetailForPage obj = new ControlDetailForPage();
                int propertyIndex = 0;
                //这里使用反射进行转换，但是需要注意的是顺序改变的话可能会出错
                foreach (var prop in obj.GetType().GetFields())
                {
                    if ("Int32".Equals(prop.FieldType.Name))
                    {
                        int value = SetInt(objArray[i][propertyIndex]);
                        prop.SetValue(obj, value);
                    }
                    else
                    {
                        string value = setString(objArray[i][propertyIndex]);
                        prop.SetValue(obj, value);
                    }
                    propertyIndex++;
                }
                controlst.Add(obj);
            }
            return controlst;
        }
        #endregion

        #region 保存页面请求实体转换
        //public static List<ControlDetailForRequest> CastControlToArrayForRequest(List<ControlDetailForPage> list)
        //{
        //    if (list.Count < 1)
        //    {
        //        return null;
        //    }
        //    List<ControlDetailForRequest> controlList = new List<ControlDetailForRequest>();
        //    foreach (ControlDetailForPage obj in list)
        //    {
        //        ControlDetailForRequest childObj = new ControlDetailForRequest();
        //        foreach (var prop in obj.GetType().GetFields())
        //        {
        //            //由于这两个类之间的差异只有一个pageid，所以这里直接跳过一个pageid就可以了
        //            if (!"page_id".Equals(prop.Name))
        //            {
        //                FieldInfo propp = childObj.GetType().GetField(prop.Name, BindingFlags.Public | BindingFlags.Instance);
        //                propp.SetValue(childObj, prop.GetValue(obj));
        //            }
        //        }
        //        controlList.Add(childObj);
        //    }
        //    return controlList;
        //}
        #endregion

        #region 实体转换为对象数组
        public static object[][] CastControlToObjectArray(List<ControlDetailForPage> list)
        {
            object[][] objArray = new object[list.Count][];
            int index = 0;
            foreach (ControlDetailForPage obj in list)
            {
                object[] objChildrenArray = new object[obj.GetType().GetFields().Length];
                int propertyIndex = 0;
                foreach (var prop in obj.GetType().GetFields())
                {
                    objChildrenArray[propertyIndex] = prop.GetValue(obj);
                    propertyIndex++;
                }
                objArray[index] = objChildrenArray;
                index++;
            }
            return objArray;
        }
        #endregion

        private static int SetInt(object text)
        {
            if (object.Equals(text, null) || string.IsNullOrEmpty(text.ToString()))
            {
                return -1;
            }
            else
            {
                return xinLongyuConverter.StringToInt(text.ToString());
            }
        }

        private static string setString(object text)
        {
            if (object.Equals(text, null) || string.IsNullOrEmpty(text.ToString()))
            {
                return string.Empty;
            }
            else
            {
                return text.ToString();
            }
        }
    }
}

