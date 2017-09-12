using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using xinLongyuClient.CommonDictionary;
using xinLongyuClient.CommonFunction;
using xinLongyuClient.Connection;
using xinLongyuClient.Interface;
using xinLongyuClient.Model.PageInfo;
using xinLongyuClient.View.CustomControl;

namespace xinLongyuClient.Decoder
{
    /// <summary>
    /// 表格控件解析器
    /// </summary>
    public class GridViewDecoder
    {
        /// <summary>
        /// 委托管理类
        /// </summary>
        private delegateForControl _delegateForControl;
        /// <summary>
        /// 连接管理类
        /// </summary>
        private ConnectionManager _connectionManager;

        private ControlDetailForPage _currentGridControlObj;

        private Dictionary<string, string>[] _dicDataSource;
        public GridViewDecoder()
        {
            _delegateForControl = new delegateForControl();
            _connectionManager = new ConnectionManager();
        }

        public void DecodeGridView(ControlDetailForPage controlObj, List<ControlDetailForPage> listControlObj, List<IControl> listControl, Control fatherControl)
        {
            xinlongyuDataGridView myNewGrid = new xinlongyuDataGridView();
            _currentGridControlObj = controlObj;
            myNewGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            myNewGrid.Location = new System.Drawing.Point(controlObj.d3, controlObj.d4);
            myNewGrid.Name = controlObj.ctrl_id.ToString();
            myNewGrid.Size = new System.Drawing.Size(controlObj.d1, controlObj.d2);
            myNewGrid.TabIndex = 0;
            myNewGrid.RowHeadersVisible = false;
            myNewGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            myNewGrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;
            ((System.ComponentModel.ISupportInitialize)(myNewGrid)).EndInit();
            myNewGrid.Visible = true;
            //添加到控件列表中
            listControl.Add(myNewGrid);
            //添加到窗体中
            _delegateForControl.AddControl(myNewGrid, fatherControl);
            //加载列
            AddChildControl(controlObj, listControlObj, myNewGrid);
            //加载内容
            LoadContent(myNewGrid, controlObj);
            //订阅事件
            HandledEvent(myNewGrid);
        }

        private void HandledEvent(xinlongyuDataGridView myDtGrid)
        {
            myDtGrid.CellContentClick += MyDtGrid_CellContentClick;
        }

        private void MyDtGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                e.RowIndex >= 0)
            {
                List<Dictionary<string, string>> lst = new List<Dictionary<string, string>>();
                if (!object.Equals(_dicDataSource, null) && _dicDataSource.Length > 0)
                {
                    lst.AddRange(_dicDataSource);
                    lst.RemoveAt(e.RowIndex);
                    _dicDataSource = lst.ToArray();
                    senderGrid.Rows.RemoveAt(e.RowIndex);
                    senderGrid.Refresh();
                }
            }
        }

        /// <summary>
        /// 添加子控件
        /// </summary>
        /// <param name="controlObj"></param>
        /// <param name="listControlObj"></param>
        /// <param name="myNewGrid"></param>
        private void AddChildControl(ControlDetailForPage controlObj, List<ControlDetailForPage> listControlObj, xinlongyuDataGridView myNewGrid)
        {
            string controlList = controlObj.d17;
            List<int> controlIdList = jsonDecoder.DecodeArray(controlList);
            if (controlIdList.Count < 1)
            {
                return;
            }

            List<ControlDetailForPage> childrenList = listControlObj.Where(p => controlIdList.Contains(p.ctrl_id)).ToList();
            List<ControlDetailForPage> columnList = childrenList.Where(p => xinLongyuControlType.GridColumnName.Equals(p.ctrl_type)).OrderBy(p => p.d3).ToList();

            List<ControlDetailForPage> columnValue = childrenList.Where(p => !xinLongyuControlType.GridColumnName.Equals(p.ctrl_type)).OrderBy(p => p.d3).ToList();
            for (int i = 0; i <= columnList.Count - 1; i++)
            {
                SetColumn(columnList[i], columnValue[i], myNewGrid);
            }
        }


        /// <summary>
        /// 添加列
        /// </summary>
        /// <param name="ctColumnObj"></param>
        /// <param name="ctObj"></param>
        /// <param name="myDtView"></param>
        private void SetColumn(ControlDetailForPage ctColumnObj, ControlDetailForPage ctObj, xinlongyuDataGridView myDtView)
        {
            if (ctObj.ctrl_type.Equals(xinLongyuControlType.imgType))
            {
                DataGridViewImageColumn imageCol = new DataGridViewImageColumn();
                imageCol.HeaderText = ctColumnObj.d0;
                imageCol.Width = ctColumnObj.d1;
                imageCol.Tag = ctObj.ctrl_id;
                _delegateForControl.AddColumn(myDtView, imageCol);
            }
            else if (xinLongyuControlType.buttonType.Equals(ctObj.ctrl_type))
            {
                DataGridViewButtonColumn btnCol = new DataGridViewButtonColumn();
                btnCol.HeaderText = ctColumnObj.d0;
                btnCol.Width = ctColumnObj.d1;
                btnCol.Tag = ctObj.d0;
                _delegateForControl.AddColumn(myDtView, btnCol);
            }
            else if (xinLongyuControlType.textType.Equals(ctObj.ctrl_type))
            {
                DataGridViewTextBoxColumn normalCols = new DataGridViewTextBoxColumn();
                normalCols.HeaderText = ctColumnObj.d0;
                normalCols.Width = ctColumnObj.d1;
                normalCols.Tag = ctObj.ctrl_id;
                _delegateForControl.AddColumn(myDtView, normalCols);
            }
        }

        /// <summary>
        /// 设置边框，这个感觉其实可有可无
        /// </summary>
        /// <param name="column"></param>
        //private void Setpadding(DataGridViewColumn column)
        //{
        //    int w = column.Width;
        //    int margin = (w > 60) ? (w - 60) >> 1 : 0;
        //    int vriMargin = (w > 40) ? (w - 40) >> 1 : 0;
        //    Padding p = column.DefaultCellStyle.Padding;
        //    p.Left = margin;
        //    p.Right = margin;
        //    p.Top = margin;
        //    p.Bottom = margin;
        //    column.DefaultCellStyle.Padding = p;
        //}

        /// <summary>
        /// 加载内容
        /// </summary>
        /// <param name="myDtView"></param>
        /// <param name="gridObj"></param>
        private void LoadContent(xinlongyuDataGridView myDtView, ControlDetailForPage gridObj)
        {
            _dicDataSource = _connectionManager.ExcuteSqlWithReturn(gridObj.d0).data;
            List<Dictionary<string, string>> lst = new List<Dictionary<string, string>>();
            lst.AddRange(_dicDataSource);
            SetContent(myDtView, gridObj, lst);
        }

        private void SetContent(xinlongyuDataGridView myDtView, ControlDetailForPage gridObj, List<Dictionary<string, string>> dicResult)
        {
            Dictionary<int, string> dicCtrlToColumn = GetDicCtrlToColumn(gridObj.d22);

            foreach (Dictionary<string, string> dicRow in dicResult)
            {
                DataGridViewRow row = (DataGridViewRow)myDtView.Rows[0].Clone();
                for (int i = 0; i < myDtView.ColumnCount; i++)
                {
                    int ctrlId = xinLongyuConverter.StringToInt(myDtView.Columns[i].Tag.ToString());
                    if (dicCtrlToColumn.ContainsKey(ctrlId))
                    {
                        string columnName = dicCtrlToColumn[ctrlId];
                        if (dicRow.ContainsKey(columnName))
                        {
                            if (myDtView.Columns[i] is DataGridViewImageColumn)
                            {
                                row.Cells[i].Value = _connectionManager.GetImage(dicRow[columnName]);
                            }
                            else
                            {
                                row.Cells[i].Value = dicRow[columnName];
                            }
                        }
                    }
                    //赋值按钮列
                    else if (myDtView.Columns[i] is DataGridViewButtonColumn)
                    {
                        row.Cells[i].Value = myDtView.Columns[i].Tag.ToString();
                    }
                }
                _delegateForControl.AddRow(myDtView, row);
            }
            //
            //myDtView.Tag = dicResult;//将数据源暂时与控件绑定到一起
        }

        /// <summary>
        /// 获取列与值对应
        /// </summary>
        /// <param name="inText"></param>
        /// <returns></returns>
        private Dictionary<int, string> GetDicCtrlToColumn(string inText)
        {
            Dictionary<int, string> dicresult = new Dictionary<int, string>();
            string[] array = inText.Split('&');
            foreach (string str in array)
            {
                string[] value = str.Split('=');
                dicresult.Add(xinLongyuConverter.StringToInt(value[0]), value[1]);
            }
            return dicresult;
        }
    }
}
