using System.Collections.Generic;
using System.Data.SQLite;
using System.Text;
using xinLongyuClient.CommonFunction;
using xinLongyuClient.Decoder;
using xinLongyuClient.Model.PageInfo;
namespace xinLongyuClient.Cacher
{
    /// <summary>
    /// 页面缓存类
    /// </summary>
    public class pageCacher : dataBaseController
    {
        //hs_new_pages
        private string _pageTableName = "hs_new_pages";

        //private string _pageControlTableName = "hs_new_page_ctrls";
        /// <summary>
        /// 缓存页面信息
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="listControlObject"></param>
        /// <returns></returns>
        public int CachePageInfo(pageInfoDetail obj)
        {
            List<ControlDetailForPage> listControlObject = ControlCaster.CastArrayToControl(obj.data.control_list);
            //
            this.CachePageBaseInfo(obj);
            //
            this.CachePageControlInfo(listControlObject);
            return 1;
        }

        /// <summary>
        /// 获取页面信息
        /// </summary>
        /// <param name="pageid"></param>
        /// <returns></returns>
        public pageInfoDetail GetPageInfo(int pageid)
        {
            pageInfoDetail pd = new pageInfoDetail();
            pd.data = this.GetPageBaseInfo(pageid);
            pd.data.control_list = ControlCaster.CastControlToObjectArray(this.GetControlBaseInfo(pageid));
            return pd;
        }

        /// <summary>
        /// 获取缓存界面的时间戳
        /// </summary>
        /// <param name="pageid"></param>
        /// <returns></returns>
        public int GetTimeStampOfPage(int pageid)
        {
            string sql = @"select last_time from hs_new_pages where page_id = '{0}'";
            sql = string.Format(sql, pageid);
            string value = this.ExcuteReturnOne(sql);
            return string.IsNullOrEmpty(value) ? 0 : int.Parse(value);
        }

        /// <summary>
        /// 获取页面基本信息
        /// </summary>
        /// <param name="pageid"></param>
        /// <returns></returns>
        private pageBaseInfo GetPageBaseInfo(int pageid)
        {
            string sql = @"select page_id, page_name, page_width, page_height, last_time from hs_new_pages where page_id = '{0}'";
            sql = string.Format(sql, pageid);
            SQLiteDataReader reader = this.ExcuteReader(sql);

            while (reader.Read())
            {
                pageBaseInfo obj = new pageBaseInfo();
                obj.page_id = int.Parse(reader[0].ToString());
                obj.page_name = reader[1].ToString();
                obj.page_width = int.Parse(reader[2].ToString());
                obj.page_height = int.Parse(reader[3].ToString());
                obj.last_time = int.Parse(reader[4].ToString());
                return obj;
            }
            reader.Close();
            this._connection.Close();
            return null;
        }

        /// <summary>
        /// 获取控件基本信息
        /// </summary>
        /// <param name="pageid"></param>
        /// <returns></returns>
        private List<ControlDetailForPage> GetControlBaseInfo(int pageid)
        {
             string sql = @"select * from hs_new_page_ctrls t where t.page_id = '{0}'";
            sql = string.Format(sql, pageid);
            SQLiteDataReader reader = this.ExcuteReader(sql);
            List<ControlDetailForPage> listControl = new List<ControlDetailForPage>();
            while (reader.Read())
            {
                //int index = 0;
                ControlDetailForPage obj = new ControlDetailForPage();
                foreach (var prop in obj.GetType().GetFields())
                {
                    string value = reader[prop.Name].ToString();
                    //这里判断一下类型
                    if ("Boolean".Equals(prop.FieldType.Name))
                    {
                        prop.SetValue(obj, xinLongyuConverter.StringToBool(value));
                    }
                    else if ("Int32".Equals(prop.FieldType.Name))
                    {
                        prop.SetValue(obj, xinLongyuConverter.StringToInt(value));
                    }
                    else
                    {
                        prop.SetValue(obj, value);
                    }
                        //prop.SetValue(obj, reader[prop.Name]);
                    //index++;
                }
                listControl.Add(obj);
            }
            reader.Close();
            this._connection.Close();
            return listControl;
        }

        public int DeletePageInfo(pageInfoDetail obj)
        {
            string sql = this.GetDeleteSql(obj.data);
            this.ExcuteNoQuery(sql);
            return 1;
        }

        /// <summary>
        /// 缓存页面基本信息
        /// </summary>
        /// <param name="obj"></param>
        private void CachePageBaseInfo(pageInfoDetail obj)
        {
            string deleteSql = string.Format(@"delete from hs_new_pages where page_id = '{0}'", obj.data.page_id);
            this.ExcuteNoQuery(deleteSql);
            string sql = @"insert into hs_new_pages (page_id, page_name, page_width, page_height,last_time) values ('{0}', '{1}', '{2}', '{3}', '{4}')";
            sql = string.Format(sql, obj.data.page_id, obj.data.page_name, obj.data.page_width, obj.data.page_height, obj.data.last_time);
            this.ExcuteNoQuery(sql);
        }

        /// <summary>
        /// 缓存控件基本信息
        /// </summary>
        /// <param name="obj"></param>
        private void CachePageControlInfo(List<ControlDetailForPage> listControlObject)
        {
            if (object.Equals(listControlObject, null) || listControlObject.Count < 1)
            {
                return;
            }
            string deleteSql = string.Format(@"delete from hs_new_page_ctrls where page_id = '{0}'", listControlObject[0].page_id);
            this.ExcuteNoQuery(deleteSql);
            string insertSql = @"insert into hs_new_page_ctrls({0}) {1}";
            StringBuilder sbColumns = new StringBuilder(1000);
            StringBuilder sbValues = new StringBuilder(1000);
            int index = 0;
            int indexOfValue = 0;
            bool valueStartTag = false;
            foreach (ControlDetailForPage controlObj in listControlObject)
            {
                foreach (var prop in controlObj.GetType().GetFields())
                {
                    if (index == 0)
                    {
                        if (sbColumns.Length < 1)
                        {
                            sbColumns.Append(prop.Name);
                        }
                        else
                        {
                            sbColumns.Append("," + prop.Name);
                        }
                    }
                    if (indexOfValue == 0)
                    {
                        if (sbValues.Length < 1)
                        {
                            sbValues.Append("select " + "'" + prop.GetValue(controlObj) + "'");
                        }
                        else
                        {
                            sbValues.Append("," + "'" + prop.GetValue(controlObj) + "'");
                        }
                    }
                    else
                    {
                        if (!valueStartTag)
                        {
                            sbValues.Append("\n" + " union select " + "'" + prop.GetValue(controlObj) + "'");
                            valueStartTag = true;
                        }
                        else
                        {
                            sbValues.Append("," + "'" + prop.GetValue(controlObj) + "'");
                        }
                    }
                }
                valueStartTag = false;
                indexOfValue++;
                index++;
            }

            insertSql = string.Format(insertSql, sbColumns.ToString(), sbValues.ToString());

            this.ExcuteNoQuery(insertSql);
        }

        /// <summary>
        /// 获取插入表的语句
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        //private string GetInsertSql(pageBaseInfo obj)
        //{
        //    string insertSql = "insert into {0} values ({1})";
        //    string values = string.Empty;
        //    StringBuilder sb = new StringBuilder(1000);
        //    foreach (var prop in obj.GetType().GetFields())
        //    {
        //        if (sb.Length < 1)
        //        {
        //            sb.Append(prop.GetValue(obj));
        //        }
        //        else
        //        {
        //            sb.Append("," + prop.GetValue(obj));
        //        }
        //    }
        //    insertSql = string.Format(insertSql, _pageTableName, sb.ToString());
        //    return insertSql;
        //}

        /// <summary>
        /// 获取更新的语句
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private string GetUpdateSql(pageBaseInfo obj)
        {
            string updateSql = "update {0} set {1} where page_id = '{2}'";
            string values = string.Empty;
            StringBuilder sb = new StringBuilder(500);
            foreach (var prop in obj.GetType().GetFields())
            {
                if (sb.Length < 1)
                {
                    sb.Append(prop.Name + "=" + prop.GetValue(obj));
                }
                else
                {
                    sb.Append("," + prop.Name + "=" + prop.GetValue(obj));
                }
            }
            updateSql = string.Format(updateSql, _pageTableName, sb.ToString(), obj.page_id);
            return updateSql;
        }

        private string GetDeleteSql(pageBaseInfo obj)
        {
            string sql = "delete from {0} where page_id = '{1}'";
            sql = string.Format(sql, _pageTableName, obj.page_id);
            return sql;
        }

        /// <summary>
        /// 测试数据库的链接
        /// </summary>
        public void TestConnect()
        {
            string sql = "insert into testxinlongyu values ('1', 'wubiqiu')";
            this.ExcuteNoQuery(sql);
        }
    }
}
