using System.Data;
using System.Data.SQLite;
using xinLongyuClient.CommonDictionary;
using xinLongyuClient.CommonFunction;

namespace xinLongyuClient.Cacher
{
    /// <summary>
    /// 数据库逻辑层
    /// 本程序使用sqllite作为缓存的数据库
    /// 需要使用数据库的逻辑层直接继承这个父类就可以了
    /// </summary>
    public class dataBaseController
    {
        public SQLiteConnection _connection;
        /// <summary>
        /// 执行sql语句
        /// </summary>
        /// <param name="sql"></param>
        public void ExcuteNoQuery(string sql)
        {
            string databaseName = this.GetDatabaseName();
            using (SQLiteConnection conn = new SQLiteConnection(databaseName))
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 执行查询语句
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataSet ExcuteQuery(string sql)
        {
            string databaseName = this.GetDatabaseName();
            using (SQLiteConnection conn = new SQLiteConnection(databaseName))
            {
                SQLiteCommand command = conn.CreateCommand();
                command.CommandText = sql;
                DataSet ds = new DataSet();
                conn.Open();
                SQLiteDataAdapter da = new SQLiteDataAdapter(command);
                da.Fill(ds);
                da.Dispose();
                command.Dispose();
                conn.Close();
                return ds;
            }
        }

        /// <summary>
        /// 获取唯一返回值
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public string ExcuteReturnOne(string sql)
        {
            DataSet dtset = this.ExcuteQuery(sql);
            if (!object.Equals(dtset, null) && dtset.Tables.Count > 0 && dtset.Tables[0].Rows.Count > 0)
            {
                string value = dtset.Tables[0].Rows[0][0].ToString();
                return value;
            }
            else
            {
                return string.Empty;
            }
        }

        public SQLiteDataReader ExcuteReader(string sql)
        {

            _connection = new SQLiteConnection(this.GetDatabaseName());
            SQLiteCommand command = new SQLiteCommand(sql, _connection);
            _connection.Open();
            SQLiteDataReader reader = command.ExecuteReader();
            
            return reader;
        }

        /// <summary>
        /// 获取数据库路径
        /// </summary>
        /// <returns></returns>
        private string GetDatabaseName()
        {
            string key = xmlController.GetNodeByXpath(configManagerSection.keyPath, CommonFilePath.CommonconfigFilePath);
            string url = xmlController.GetNodeByXpath(configManagerSection.dataBasePath, CommonFilePath.CommonconfigFilePath);
            string dbPath = "Data Source =" + System.Windows.Forms.Application.StartupPath + SmellyEggCrypt.CryPtService.DESDecrypt(url, key);
            return dbPath;
        }
    }
}
