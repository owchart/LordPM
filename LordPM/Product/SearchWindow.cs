/*****************************************************************************\
*                                                                             *
* CollectionWindow.cs - Collection window functions, types, and definitions. *
*                                                                             *
*               Version 1.00  ����                                          *
*                                                                             *
*               Copyright (c) 2016-2016, Piratecat. All rights reserved.      *
*               Created by Lord 2016/1/8.                                     *
*                                                                             *
******************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using OwLib;
using System.Xml;
using System.IO;
using System.Windows.Forms;

namespace OwLib
{
    //����
    public delegate void ExpressionChanged(List<Expression> expressions);

    /// <summary>
    /// ���ϱ༭����
    /// </summary>
    public class SearchWindow : WindowXmlEx
    {
        #region Lord 2016/1/8
        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="native">������</param>
        public SearchWindow(INativeBase native)
        {
            Load(native, "SearchWindow", "collection");
            //ע�����¼�
            RegisterEvents(m_window);
            this.m_searchTable = GetGrid("dgvTable");
        }
        //�������
        private ExpressionChanged m_expressionChanged;

        //������
        private GridA m_searchTable;

        /// <summary>
        /// �������
        /// </summary>
        public void AddData(List<Expression> list, String str)
        {
            m_searchTable.BeginUpdate();
            GridRow row = new GridRow();
            m_searchTable.AddRow(row);
            String strEx = "";
            for (int i = 0; i < list.Count; i++)
            {
               Expression expression = list[i];
               strEx += expression.getExpression();
            }
            GridCell cell1 = new GridCellExp(str);
            row.AddCell("colT1", cell1);
            GridCell cell = new GridCellExp(strEx);
            row.AddCell("colT2", cell);
            m_searchTable.EndUpdate();
            m_searchTable.Invalidate();
        }

        /// <summary>
        /// �����ť����
        /// </summary>
        /// <param name="sender">������</param>
        /// <param name="mp">����</param>
        /// <param name="button">��ť</param>
        /// <param name="click">�������</param>
        /// <param name="delta">���ֹ���ֵ</param>
        private void ClickButton(object sender, POINT mp, MouseButtonsA button, int click, int delta)
        {
            if (button == MouseButtonsA.Left && click == 1)
            {
                ControlA control = sender as ControlA;
                String name = control.Name;
                if (name == "btnAdd")
                {
                    ShowFilterWindow();
                }
                if (name == "btnSubmit")
                {
                    List<GridRow> rows = m_searchTable.GetRows();
                    int count = rows.Count;
                    if (count > 0)
                    {
                        for (int i = 0; i < count; i++)
                        {
                            GridRow gridrow = rows[i];
                            //�õ����ʽ
                            List<GridCell> cells = gridrow.GetCells();
                            int cellsCount = cells.Count;
                            String[] expressions = cells[1].Text.Split(' ');
                            List<Expression> exp = new List<Expression>();
                            Expression exps = new Expression(expressions[0], expressions[1], expressions[2]);
                            exp.Add(exps);
                            m_expressionChanged(exp);
                        }
                    }
                }
                if (name == "btnDel")
                {
                    DeleteRow();
                }
                if (name == "btnClose")
                {
                    CloseWindow();
                }
            }
        }

        /// <summary>
        /// ע���¼�
        /// </summary>
        /// <param name="control">�ؼ�</param>
        private void RegisterEvents(ControlA control)
        {
            ControlMouseEvent clickButtonEvent = new ControlMouseEvent(ClickButton);
            List<ControlA> controls = control.GetControls();
            int controlsSize = controls.Count;
            for (int i = 0; i < controlsSize; i++)
            {
                ControlA subControl = controls[i];
                ButtonA button = subControl as ButtonA;
                GridA grid = subControl as GridA;
                if (button != null)
                {
                    button.RegisterEvent(clickButtonEvent, EVENTID.CLICK);
                }
                else if (grid != null)
                {
                    GridRowStyle rowStyle = new GridRowStyle();
                    grid.RowStyle = rowStyle;
                    rowStyle.BackColor = COLOR.EMPTY;
                    rowStyle.SelectedBackColor = CDraw.PCOLORS_SELECTEDROWCOLOR;
                    rowStyle.HoveredBackColor = CDraw.PCOLORS_HOVEREDROWCOLOR;
                    rowStyle.SelectedForeColor = CDraw.PCOLORS_FORECOLOR4;
                }
                RegisterEvents(subControl);
            }
        }

        /// <summary>
        /// ע��ı��¼�
        /// </summary>
        /// <param name="expressionChanged"></param>
        public void RegisterExpChangedEvent(ExpressionChanged expressionChanged)
        {
            m_expressionChanged = expressionChanged;
        }
        
        /// <summary>
        /// ��Ӵ���
        /// </summary>
        public void ShowFilterWindow()
        {
            FilterWindow filter = new FilterWindow(Native,this);
            filter.ShowDialog();
        }

        /// <summary>
        /// �رյ�ǰ����
        /// </summary>
        public void CloseWindow()
        {
            this.Close();
        }

        /// <summary>
        /// ɾ��ѡ����
        /// </summary>
        public void DeleteRow()
        {
            if (MessageBox.Show("�Ƿ�ɾ����", "��ʾ��", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                List<GridRow> rows = m_searchTable.SelectedRows;
                int selectRowSize = rows.Count;
                if (selectRowSize <= 0)
                {
                    return;
                }
                m_searchTable.BeginUpdate();
                m_searchTable.RemoveRow(rows[0]);
                m_searchTable.EndUpdate();
                m_searchTable.Invalidate();
            }
        }
        #endregion
    }

}
