/*****************************************************************************\
*                                                                             *
* MainFrame.cs -  MainFrame functions, types, and definitions.                *
*                                                                             *
*               Version 1.00  ����                                          *
*                                                                             *
*               Copyright (c) 2016-2016, iTeam. All rights reserved.      *
*               Created by Lord 2016/12/24.                                   *
*                                                                             *
******************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using OwLib;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using System.Data;
using System.IO;
using System.Globalization;

namespace OwLib
{
    /// <summary>
    /// ����ϵͳ
    /// </summary>
    public class MainFrame : UIXmlEx, IDisposable
    {
        
        /// <summary>
        /// ��������ϵͳ
        /// </summary>
        public MainFrame()
        {
        }
        /// <summary>
        /// ���
        /// </summary>
        private int m_timerID;

        /// <summary>
        /// ��ȡ��
        /// </summary>
        private GridA m_gridDgvTable;

        /// <summary>
        /// Jira�б�
        /// </summary>
        private List<Jira> m_jiras = new List<Jira>();

        /// <summary>
        /// ����ID�жϸ��»��߲���
        /// </summary>
        /// <param name="jira">Jira����</param>
        public void AddOrUpdate(Jira jira)
        {
            //������ʽ
            GridCellStyle gridStyle = new GridCellStyle();
            gridStyle.BackColor = COLOR.DISABLEDCONTROL;

            List<GridRow> rows = m_gridDgvTable.m_rows;
            int size = rows.Count;
            //û������
            if (size <= 0)
            {
                Addrows(jira);
                return;
            }
            bool isAddRow = true;
            for (int i = 0; i < size; i++)
            {
                //GridCellStyle gridStyle = new GridCellStyle();
                //gridStyle.BackColor = COLOR.DISABLEDCONTR;
                GridRow row = rows[i];
                String id = row.GetCell("colT1").Text;
                GridCellStyle gridStyle1 = new GridCellStyle();
                gridStyle1.BackColor = COLOR.DISABLEDCONTROL;
                gridStyle1.ForeColor = COLOR.ARGB(255, 97, 0);
                row.GetCell("colT1").Style = gridStyle1;
                
                //����ID�������ǲ��뻹�Ǹ���
                if (id == jira.JiraID)
                {
                    isAddRow = false;
                    GridCellStyle gridStyle2 = new GridCellStyle();
                    gridStyle2.BackColor = COLOR.DISABLEDCONTROL;
                    gridStyle2.ForeColor = COLOR.ARGB(0, 255, 0);
                    row.GetCell("colT2").Style = gridStyle2;
                    row.GetCell("colT2").Text = jira.Title;
                    row.GetCell("colT3").Text = jira.Creater;
                    row.GetCell("colT4").Text = jira.Developer;
                    int count = XmlHandle.Groups.Count;
                    for (int m = 0; m < count; m++)
                    {
                        if (XmlHandle.Groups[m].Id == jira.GroupID)
                        {
                            GridCell cell5 = new GridCellExp(XmlHandle.Groups[m].Name);
                            row.AddCell("colT5", cell5);
                            GridCell cell6 = new GridCellExp(XmlHandle.Groups[m].Manager);
                            row.AddCell("colT6", cell6);
                            List<JCategory> categories = XmlHandle.Groups[m].Categories;
                            int cateCount = categories.Count;
                            for (int n = 0; n < cateCount; n++)
                            {
                                if (categories[n].Id == jira.CategoryID)
                                {
                                    GridCell cell7 = new GridCellExp(categories[n].Name);
                                    row.AddCell("colT7", cell7);
                                    break;
                                }
                            }
                            break;
                        }
                    }
                    row.GetCell("colT8").Text = jira.DeveloperReceive ? "��" : "��";
                    row.GetCell("colT9").Text = jira.DeveloperPass ? "��" : "��";
                    row.GetCell("colT10").Text = jira.TestPass ? "��" : "��";
                    row.GetCell("colT11").Text = jira.ProductPass ? "��" : "��";
                    row.GetCell("colT12").Text = jira.WaitPublish ? "��" : "��";
                    row.GetCell("colT13").Text = jira.Published ? "��" : "��";
                    row.GetCell("colT14").Text = jira.CloseTask ? "��" : "��";
                    row.GetCell("colT15").Text = jira.Hurry;
                    DateTime dt = DateTime.Now;
                    String status = jira.EndDate.ToFileTime() > dt.ToFileTime() ? "(��ʱ)" : "(����)";
                    GridCell cell16 = new GridCellExp(status + jira.StartDate.ToLongDateString().ToString());
                    row.GetCell("colT16").Text = cell16.Text;
                    GridCell cell17 = new GridCellExp(status + jira.StartDate.ToLongDateString().ToString());
                    row.GetCell("colT17").Text = cell17.Text;
                    m_gridDgvTable.Update();
                    return;
                }
            }

            //����
            if (isAddRow)
            {
                Addrows(jira);
                return;
            }
        }

        /// <summary>
        /// ˢ������
        /// </summary>
        public void Renovate()
        {
            //�ӷ������ȡ����
            m_jiras = XmlHandle.GetJiras();
            int count = m_jiras.Count;
            //����Dictionary���ID������
            Dictionary<String, Jira> jiraIDs = new Dictionary<String,Jira>();
            //����������������
            for(int i = 0; i < count; i++)
            {
                Jira jira = m_jiras[i];
                jiraIDs.Add(jira.JiraID, jira);
            }
            //��ȡ���������
            List<GridRow> rows = m_gridDgvTable.GetRows();
            int rowsCount = rows.Count;
            Dictionary<String, GridRow> gridRows = new Dictionary<String,GridRow>();
            //������������
            for(int i = 0; i < rowsCount; i++)
            {
                GridRow row = rows[i];
                //ȡID
                String id = row.GetCell("colT1").Text;
                //����ID�ж�
                if (!jiraIDs.ContainsKey(id))
                {
                    //ID��ƥ��ɾ����
                    m_gridDgvTable.RemoveRow(row);
                    rowsCount--;
                    i--;
                }
                else
                {
                    //ƥ����ӵ�Dictionary
                    gridRows.Add(id, row);
                }
             }
            m_gridDgvTable.Update();
            m_gridDgvTable.BeginUpdate();
            for(int i = 0; i < count; i++)
            {
                //��������������
                Jira jira = m_jiras[i]; 
                bool newData = false;
                String key = jira.JiraID;
                GridRow row;
                if(gridRows.ContainsKey(key))
                {
                    row = gridRows[key];
                }
                else
                {
                    newData = true;
                    row = new GridRow();
                   //row.Grid = m_gridDgvTable;
                   //m_gridDgvTable.m_rows.Add(row);
                   //row.OnAdd();
                    m_gridDgvTable.AddRow(row);
                }

                //����columns
                List<GridColumn> columns = m_gridDgvTable.GetColumns();
                int countColumn = columns.Count;
                for(int j = 0; j < countColumn; j++)
                {
                    GridColumn column = columns[j];
                    GridCell cell;
                    if(newData)
                    {
                        cell = new GridCellExp();
                        row.AddCell(column.Index, cell);
                        cell.Column = column;
                    }
                    else
                    {
                        cell = row.GetCell(column.Index);
                    }
                    DateTime dt = DateTime.Now;
                    String status = jira.EndDate.ToFileTime() > dt.ToFileTime() ? "(��ʱ)" : "(����)";
                    int countGroup = XmlHandle.Groups.Count;
                    switch (j)
                    { 
                        case 0:
                            GridCellStyle gridStyle1 = new GridCellStyle();
                            gridStyle1.BackColor = COLOR.DISABLEDCONTROL;
                            gridStyle1.ForeColor = COLOR.ARGB(255, 255, 255);
                            cell.Text = jira.JiraID;
                            cell.Style = gridStyle1;
                        //colT1
                            break;
                        case 1:
                            GridCellStyle gridStyle2 = new GridCellStyle();
                            gridStyle2.BackColor = COLOR.DISABLEDCONTROL;
                            gridStyle2.ForeColor = COLOR.ARGB(45, 142, 45);
                            cell.Text = jira.Title;
                            cell.Style = gridStyle2;
                            break;
                        case 2:
                            GridCellStyle gridStyle3 = new GridCellStyle();
                            gridStyle3.BackColor = COLOR.DISABLEDCONTROL;
                            gridStyle3.ForeColor = COLOR.ARGB(47, 145, 145);
                            cell.Text = jira.Creater;
                            cell.Style = gridStyle3;
                            break;
                        case 3:
                            GridCellStyle gridStyle4 = new GridCellStyle();
                            gridStyle4.BackColor = COLOR.DISABLEDCONTROL;
                            gridStyle4.ForeColor = COLOR.ARGB(47, 145, 145); ;
                            cell.Style = gridStyle4;
                            cell.Text = jira.Developer;
                            break;
                        case 4:
                            {
                                for (int m = 0; m < countGroup; m++)
                                {
                                    if (XmlHandle.Groups[m].Id == jira.GroupID)
                                    {
                                        GridCellStyle gridStyle5 = new GridCellStyle();
                                        gridStyle5.BackColor = COLOR.DISABLEDCONTROL;
                                        gridStyle5.ForeColor = COLOR.ARGB(47, 145, 145);
                                        cell.Style = gridStyle5;
                                        cell.Text = XmlHandle.Groups[m].Name;
                                        break;
                                    }
                                }
                                break;
                            }
                        case 5:
                            {
                                for (int m = 0; m < countGroup; m++)
                                {
                                    if (XmlHandle.Groups[m].Id == jira.GroupID)
                                    {
                                        GridCellStyle gridStyle6 = new GridCellStyle();
                                        gridStyle6.BackColor = COLOR.DISABLEDCONTROL;
                                        gridStyle6.ForeColor = COLOR.ARGB(255, 153, 153);
                                        cell.Style = gridStyle6;
                                        cell.Text = XmlHandle.Groups[m].Manager;
                                        break;
                                    }
                                }
                                break;
                            }
                        case 6:
                            {
                                for (int m = 0; m < countGroup; m++)
                                {
                                    if (XmlHandle.Groups[m].Id == jira.GroupID)
                                    {
                                        List<JCategory> categories = XmlHandle.Groups[m].Categories;
                                        int cateCount = categories.Count;
                                        for (int n = 0; n < cateCount; n++)
                                        {
                                            if (categories[n].Id == jira.CategoryID)
                                            {
                                                GridCellStyle gridStyle7 = new GridCellStyle();
                                                gridStyle7.BackColor = COLOR.DISABLEDCONTROL;
                                                gridStyle7.ForeColor = COLOR.ARGB(45, 142, 45);
                                                cell.Style = gridStyle7;
                                                cell.Text = categories[n].Name;
                                                break;
                                            }
                                        }
                                        break;
                                    }
                                }
                                break;
                            }
                        case 7:
                            cell.Text = jira.DeveloperReceive ? "��" : "��";
                            GridCellStyle gridStyle8 = new GridCellStyle();
                            gridStyle8.ForeColor = COLOR.ARGB(255, 255, 255);
                            if (cell.Text == "��")
                            {
                                gridStyle8.BackColor = COLOR.ARGB(93, 146, 202);
                            }
                            else
                            {
                                gridStyle8.BackColor = COLOR.ARGB(163, 5, 50);
                            }
                            cell.Style = gridStyle8;
                            break;
                        case 8:
                            cell.Text = jira.DeveloperPass ? "��" : "��";
                            GridCellStyle gridStyle9 = new GridCellStyle();
                            gridStyle9.ForeColor = COLOR.ARGB(255, 255, 255);
                            if (cell.Text == "��")
                            {
                                gridStyle9.BackColor = COLOR.ARGB(93, 146, 202);
                            }
                            else
                            {
                                gridStyle9.BackColor = COLOR.ARGB(163, 5, 50);
                            }
                            cell.Style = gridStyle9;
                            break;
                        case 9:
                            cell.Text = jira.TestPass ? "��" : "��";
                            GridCellStyle gridStyle10 = new GridCellStyle();
                            gridStyle10.ForeColor = COLOR.ARGB(255, 255, 255);
                            if (cell.Text == "��")
                            {
                                gridStyle10.BackColor = COLOR.ARGB(93, 146, 202);
                            }
                            else
                            {
                                gridStyle10.BackColor = COLOR.ARGB(163, 5, 50);
                            }
                            cell.Style = gridStyle10;
                            break;
                        case 10:
                            cell.Text = jira.ProductPass ? "��" : "��";
                            GridCellStyle gridStyle11 = new GridCellStyle();
                            gridStyle11.ForeColor = COLOR.ARGB(255, 255, 255);
                            if (cell.Text == "��")
                            {
                                gridStyle11.BackColor = COLOR.ARGB(93, 146, 202);
                            }
                            else
                            {
                                gridStyle11.BackColor = COLOR.ARGB(163, 5, 50);
                            }
                            cell.Style = gridStyle11;
                            break;
                        case 11:
                            cell.Text = jira.WaitPublish ? "��" : "��";
                            GridCellStyle gridStyle12 = new GridCellStyle();
                            gridStyle12.ForeColor = COLOR.ARGB(255, 255, 255);
                            if (cell.Text == "��")
                            {
                                gridStyle12.BackColor = COLOR.ARGB(93, 146, 202);
                            }
                            else
                            {
                                gridStyle12.BackColor = COLOR.ARGB(163, 5, 50);
                            }
                            cell.Style = gridStyle12;
                            break;
                        case 12:
                            cell.Text = jira.Published ? "��" : "��";
                            GridCellStyle gridStyle13 = new GridCellStyle();
                            gridStyle13.ForeColor = COLOR.ARGB(255, 255, 255);
                            if (cell.Text == "��")
                            {
                                gridStyle13.BackColor = COLOR.ARGB(93, 146, 202);
                            }
                            else
                            {
                                gridStyle13.BackColor = COLOR.ARGB(163, 5, 50);
                            }
                            cell.Style = gridStyle13;
                            break;
                        case 13:
                            cell.Text = jira.CloseTask ? "��" : "��";
                            GridCellStyle gridStyle14 = new GridCellStyle();
                            gridStyle14.ForeColor = COLOR.ARGB(255, 255, 255);
                            if (cell.Text == "��")
                            {
                                gridStyle14.BackColor = COLOR.ARGB(93, 146, 202);
                            }
                            else
                            {
                                gridStyle14.BackColor = COLOR.ARGB(163, 5, 50);
                            }
                            cell.Style = gridStyle14;
                            break;
                        case 14:
                            cell.Text = jira.Hurry;
                            GridCellStyle gridStyle15 = new GridCellStyle();
                            gridStyle15.BackColor = COLOR.ARGB(0, 0, 0);
                            if (cell.Text == "����")
                            {
                                gridStyle15.ForeColor = COLOR.ARGB(163, 5, 50);
                            }
                            else
                            {
                                gridStyle15.ForeColor = COLOR.ARGB(227, 171, 26);
                            }
                            cell.Style = gridStyle15;
                            break;
                        case 15:
                            GridCellStyle gridStyle16 = new GridCellStyle();
                            GridCell cell16 = new GridCellExp(status + jira.StartDate.ToLongDateString().ToString());
                            cell.Text = cell16.Text;
                            gridStyle16.ForeColor = COLOR.ARGB(255, 255, 0);
                            cell.Style = gridStyle16;
                            break;
                        case 16:
                            GridCellStyle gridStyle17 = new GridCellStyle();
                            GridCell cell17 = new GridCellExp(status + jira.StartDate.ToLongDateString().ToString());
                            cell.Text = cell17.Text;
                            gridStyle17.ForeColor = COLOR.ARGB(255, 255, 0);
                            cell.Style = gridStyle17;
                            break;
                    }
                }
            }
                    
            m_gridDgvTable.EndUpdate();
            m_gridDgvTable.Invalidate();
        }

        /// <summary>
        /// ����������
        /// </summary>
        /// <param name="jira">Jira����</param>
        public void Addrows(Jira jira)
        {
            m_gridDgvTable.BeginUpdate();
            //������ʽ
            GridCellStyle gridStyle = new GridCellStyle();
            gridStyle.BackColor = COLOR.DISABLEDCONTROL;
            GridRow row = new GridRow();
            m_gridDgvTable.AddRow(row);
            //���ñ߿��ߵ���ɫ
            m_gridDgvTable.GridLineColor = COLOR.EMPTY;
            m_gridDgvTable.HeaderHeight = 35;
            m_gridDgvTable.BackColor = COLOR.ARGB(31,29,40);
            row.Tag = jira;
            //�����и�
            row.Height = 35;
            GridCell cell1 = new GridCellExp(jira.JiraID);
            row.AddCell("colT1", cell1);
            GridCellStyle gridStyle1 = new GridCellStyle();
            gridStyle1.BackColor = COLOR.DISABLEDCONTROL;
            gridStyle1.ForeColor = COLOR.ARGB(255, 255, 255);
            cell1.Style = gridStyle1;
            GridCell cell2 = new GridCellExp(jira.Title);
            row.AddCell("colT2", cell2);
            GridCellStyle gridStyle2 = new GridCellStyle();
            gridStyle2.BackColor = COLOR.DISABLEDCONTROL;
            gridStyle2.ForeColor = COLOR.ARGB(45, 142, 45);
            cell2.Style = gridStyle2;
            GridCell cell3 = new GridCellExp(jira.Creater);
            row.AddCell("colT3", cell3);
            GridCell cell4 = new GridCellExp(jira.Developer);
            row.AddCell("colT4", cell4);
            GridCellStyle gridStyle3 = new GridCellStyle();
            gridStyle3.BackColor = COLOR.DISABLEDCONTROL;
            gridStyle3.ForeColor = COLOR.ARGB(47, 145, 145);
            cell3.Style = gridStyle3;
            cell4.Style = gridStyle3;
            int count = XmlHandle.Groups.Count; 
            for (int j = 0; j < count; j++)
            {
		        JGroup group =  XmlHandle.Groups[j];
                if (group.Id == jira.GroupID)
                {
                    GridCell cell5 = new GridCellExp(group.Name);
                    row.AddCell("colT5", cell5);
                    GridCellStyle gridStyle5 = new GridCellStyle();
                    gridStyle5.BackColor = COLOR.DISABLEDCONTROL;
                    gridStyle5.ForeColor = COLOR.ARGB(47, 145, 145);
                    cell5.Style = gridStyle5;
                    GridCell cell6 = new GridCellExp(group.Manager);
                    row.AddCell("colT6", cell6);
                    GridCellStyle gridStyle6 = new GridCellStyle();
                    gridStyle6.BackColor = COLOR.DISABLEDCONTROL;
                    gridStyle6.ForeColor = COLOR.ARGB(255, 153, 153);
                    cell6.Style = gridStyle6;
                    List<JCategory> categories = group.Categories;
		            int categoriesCount = categories.Count;
                    for (int n = 0; n < categoriesCount; n++)
                    {
                        if (categories[n].Id == jira.CategoryID)
                        {
                            GridCell cell7 = new GridCellExp(categories[n].Name);
                            row.AddCell("colT7", cell7);
                            GridCellStyle gridStyle7 = new GridCellStyle();
                            gridStyle7.BackColor = COLOR.DISABLEDCONTROL;
                            gridStyle7.ForeColor = COLOR.ARGB(45, 142, 45);
                            cell7.Style = gridStyle7;
                            break;
                        }
                    }
                    break;
                }
            }
            String str = jira.DeveloperReceive ? "��" : "��";
            GridCell cell8 = new GridCellExp(str);
            GridCellStyle gridStyle8 = new GridCellStyle();
            gridStyle8.ForeColor = COLOR.ARGB(255, 255, 255);
            if (str == "��")
            {
                gridStyle8.BackColor = COLOR.ARGB(93, 146, 202);
            }
            else 
            {
                gridStyle8.BackColor = COLOR.ARGB(163, 5, 50);
            }
            cell8.Style = gridStyle8;
            row.AddCell("colT8", cell8);
            String str1 = jira.DeveloperPass ? "��" : "��";
            GridCell cell9 = new GridCellExp(str1);
            GridCellStyle gridStyle9 = new GridCellStyle();
            gridStyle9.ForeColor = COLOR.ARGB(255, 255, 255);
            if (str1 == "��")
            {
                gridStyle9.BackColor = COLOR.ARGB(93, 146, 202);
            }
            else
            {
                gridStyle9.BackColor = COLOR.ARGB(163, 5, 50);
            }
            cell9.Style = gridStyle9;
            row.AddCell("colT9", cell9);
            String str2 = jira.TestPass ? "��" : "��";
            GridCell cell10 = new GridCellExp(str2);
            GridCellStyle gridStyle10 = new GridCellStyle();
            gridStyle10.ForeColor = COLOR.ARGB(255, 255, 255);
            if (str2 == "��")
            {
                gridStyle10.BackColor = COLOR.ARGB(93, 146, 202);
            }
            else
            {
                gridStyle10.BackColor = COLOR.ARGB(163, 5, 50);
            }
            cell10.Style = gridStyle10;
            row.AddCell("colT10", cell10);
            String str3 = jira.ProductPass ? "��" : "��";
            GridCell cell11 = new GridCellExp(str3);
            GridCellStyle gridStyle11 = new GridCellStyle();
            gridStyle11.ForeColor = COLOR.ARGB(255, 255, 255);
            if (str3 == "��")
            {
                gridStyle11.BackColor = COLOR.ARGB(93, 146, 202);
            }
            else
            {
                gridStyle11.BackColor = COLOR.ARGB(163, 5, 50);
            }
            cell11.Style = gridStyle11;
            row.AddCell("colT11", cell11);
            String str4 = jira.WaitPublish ? "��" : "��";
            GridCell cell12 = new GridCellExp(str4);
            GridCellStyle gridStyle12 = new GridCellStyle();
            gridStyle12.ForeColor = COLOR.ARGB(255, 255, 255);
            if (str4 == "��")
            {
                gridStyle12.BackColor = COLOR.ARGB(93, 146, 202);
            }
            else
            {
                gridStyle12.BackColor = COLOR.ARGB(163, 5, 50);
            }
            cell12.Style = gridStyle12;
            row.AddCell("colT12", cell12);
            String str5 = jira.Published ? "��" : "��";
            GridCell cell13 = new GridCellExp(str5);
            GridCellStyle gridStyle13 = new GridCellStyle();
            gridStyle13.ForeColor = COLOR.ARGB(255, 255, 255);
            if (str5 == "��")
            {
                gridStyle13.BackColor = COLOR.ARGB(93, 146, 202);
            }
            else
            {
                gridStyle13.BackColor = COLOR.ARGB(163, 5, 50);
            }
            cell13.Style = gridStyle13;
            row.AddCell("colT13", cell13);

            String str6 = jira.CloseTask ? "��" : "��";
            GridCell cell14 = new GridCellExp(str6);
            GridCellStyle gridStyle14 = new GridCellStyle();
            gridStyle14.ForeColor = COLOR.ARGB(255, 255, 255);
            if (str6 == "��")
            {
                gridStyle14.BackColor = COLOR.ARGB(93, 146, 202);
            }
            else
            {
                gridStyle14.BackColor = COLOR.ARGB(163, 5, 50);
            }
            cell14.Style = gridStyle14;
            row.AddCell("colT14", cell14);

            GridCell cell15 = new GridCellExp(jira.Hurry);
            GridCellStyle gridStyle15 = new GridCellStyle();
            gridStyle15.BackColor = COLOR.ARGB(0, 0, 0);
            if (jira.Hurry == "����")
            {
                gridStyle15.ForeColor = COLOR.ARGB(255, 0, 0);
            }
            else
            {
                gridStyle15.ForeColor = COLOR.ARGB(255, 255, 0);
            }
            cell15.Style = gridStyle15;
            row.AddCell("colT15", cell15);
            GridCellStyle gridStyle16 = new GridCellStyle();
            gridStyle16.BackColor = COLOR.DISABLEDCONTROL;
            gridStyle16.ForeColor = COLOR.ARGB(255, 255, 0);
            DateTime dt1 = DateTime.Now;
            String status = jira.EndDate.ToFileTime() > dt1.ToFileTime() ? "(��ʱ)" : "(����)";
            GridCell cells16 = new GridCellExp(status + jira.StartDate.ToLongDateString().ToString());
            row.AddCell("colT16", cells16);
            cells16.Tag = jira.StartDate;
            GridCell cells17 = new GridCellExp(status + jira.StartDate.ToLongDateString().ToString());
            row.AddCell("colT17", cells17);
            cells16.Style = gridStyle16;
            cells17.Style = gridStyle16;
            cells17.Tag = jira.EndDate;
            m_gridDgvTable.EndUpdate();
        }

        /// <summary>
        /// ����¼�
        /// </summary>
        /// <param name="sender">������</param>
        /// <param name="mp">����</param>
        /// <param name="button">��ť</param>
        /// <param name="clicks">�������</param>
        /// <param name="delta">����ֵ/param>
        private void ClickEvent(object sender, POINT mp, MouseButtonsA button, int clicks, int delta)
        {
            if (button == MouseButtonsA.Left && clicks == 1)
            {
                ControlA control = sender as ControlA;
                String name = control.Name;
                if (name == "btnFilter")
                {
                    ShowSearchWindow();
                }
                if (name == "btnModify")
                {
                    ShowEditWindow();
                }
                if (name == "btnAdd")
                {
                    ShowAddWindow();
                }
                if (name == "btnDelete")
                {
                    DeleteRow();
                }
                if (name == "btnRefresh")
                {
                    m_gridDgvTable.Invalidate();
                }
            }
        }

        /// <summary>
        /// ɾ��ѡ����
        /// </summary>
        public void DeleteRow()
        {
            if (MessageBox.Show("�Ƿ�ɾ����", "��ʾ��", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                List<GridRow> rows = m_gridDgvTable.SelectedRows;
                int selectRowSize = rows.Count;
                if (selectRowSize <= 0)
                {
                    return;
                }
                m_gridDgvTable.BeginUpdate();
                m_gridDgvTable.RemoveRow(rows[0]);
                m_gridDgvTable.EndUpdate();
                m_gridDgvTable.Invalidate();
            }
        }

        /// <summary>
        /// ������Դ����
        /// </summary>
        public override void Dispose()
        {
        }

        /// <summary>
        /// �˳�����
        /// </summary>
        public override void Exit()
        {
        }

        /// <summary>
        /// �Ƿ��д�����ʾ
        /// </summary>
        /// <returns>�Ƿ���ʾ</returns>
        public bool IsWindowShowing()
        {
            List<ControlA> controls = Native.GetControls();
            int controlsSize = controls.Count;
            for (int i = 0; i < controlsSize; i++)
            {
                WindowFrameA frame = controls[i] as WindowFrameA;
                if (frame != null)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// ����XML
        /// </summary>
        /// <param name="xmlPath">XML·��</param>
        public override void Load(String xmlPath)
        {
            LoadFile(xmlPath, null);
            DataCenter.MainUI = this;
            ControlA control = Native.GetControls()[0];
            control.BackColor = COLOR.CONTROL;
            RegisterEvents(control);
            m_jiras.Clear();
            //�ӷ������ȡ����
            m_jiras = XmlHandle.GetJiras();
            this.m_gridDgvTable = GetGrid("dgvTable");

            //ע���¼�������������һ���Զ��巽�������
            m_gridDgvTable.RegisterEvent(new ControlTimerEvent(TimerEvent), EVENTID.TIMER);
            m_timerID = ControlA.GetNewTimerID();
            m_gridDgvTable.StartTimer(m_timerID, 1000);
            int size = m_jiras.Count;
            for (int i = 0; i < size; i++)
            {
                Jira jira = m_jiras[i];
                AddOrUpdate(jira);//���һ��
            }
            //ˢ�½���
            m_gridDgvTable.Invalidate();
        }

        /// <summary>
        /// ����¼�
        /// </summary>
        /// <param name="sender">������</param>
        /// <param name="timerID">���ID</param>
        private void TimerEvent(object sender, int timerID)
        {
            if (m_timerID == timerID)
            {
                Renovate();
                m_gridDgvTable.Invalidate();
            }
        }

        /// <summary>
        /// ��д����������Զ�����
        /// </summary>
        /// <param name="sender">���ID</param>
        public void OnTimer(int timerID)
        {
            
        }

        /// <summary>
        /// �ػ�˵�����
        /// </summary>
        /// <param name="sender">���ö���</param>
        /// <param name="paint">��ͼ����</param>
        /// <param name="clipRect">�ü�����</param>
        private void PaintLayoutDiv(object sender, CPaint paint, OwLib.RECT clipRect)
        {
            ControlA control = sender as ControlA;
            int width = control.Width, height = control.Height;
            OwLib.RECT drawRect = new OwLib.RECT(0, 0, width, height);
            paint.FillGradientRect(CDraw.PCOLORS_BACKCOLOR, CDraw.PCOLORS_BACKCOLOR2, drawRect, 0, 90);
        }

        /// <summary>
        /// ע���¼�
        /// </summary>
        /// <param name="control">�ؼ�</param>
        private void RegisterEvents(ControlA control)
        {
            ControlMouseEvent clickButtonEvent = new ControlMouseEvent(ClickEvent);
            List<ControlA> controls = control.GetControls();
            int controlsSize = controls.Count;
            for (int i = 0; i < controlsSize; i++)
            {
                ControlA subControl = controls[i];
                ButtonA button = subControl as ButtonA;
                if (button != null)
                {
                    button.RegisterEvent(clickButtonEvent, EVENTID.CLICK);
                }
                RegisterEvents(controls[i]);
            }
        }

        /// <summary>
        /// ��ʾ���Ӵ���
        /// </summary>
        public void ShowAddWindow()
        {
            EditWindow editWindow = new EditWindow(Native);
            editWindow.MainFrame = this;
            editWindow.Create(null);
            editWindow.ShowDialog();
        }

        /// <summary>
        /// ��ʾ�޸Ĵ���
        /// </summary>
        public void ShowEditWindow()
        {
            List<GridRow> rows = m_gridDgvTable.SelectedRows;
            Jira jira = rows[0].Tag as Jira;
            EditWindow editWindow = new EditWindow(Native);
            editWindow.MainFrame = this;
            editWindow.Create(jira);
            editWindow.ShowDialog();//ֻ����Ե�ǰ���ڲ���
        }

        /// <summary>
        /// ��ʾ��ʾ����
        /// </summary>
        /// <param name="text">�ı�</param>
        /// <param name="caption">����</param>
        /// <param name="uType">��ʽ</param>
        /// <returns>���</returns>
        public int ShowMessageBox(String text, String caption, int uType)
        {
            MessageBox.Show(text, caption);
            return 1;
        }

        /// <summary>
        /// ��ʾ�߼���ѯ����
        /// </summary>
        public void ShowSearchWindow()
        {
            SearchWindow searchWindow = new SearchWindow(Native);
            //�ص�
            searchWindow.RegisterExpChangedEvent(new ExpressionChanged(ExpressionChange));
            searchWindow.ShowDialog();
        }

        /// <summary>
        /// �ص�����,ɸѡ���
        /// </summary>
        /// <param name="expressions"></param>
        public void ExpressionChange(List<Expression> expressions)
        { 
            int count = expressions.Count;
            List<GridRow> rows = m_gridDgvTable.GetRows();
            int rowsCount = rows.Count;
            for (int j = 0; j < rowsCount; j++)
            { 
                bool flag = true;
                for (int i = 0; i < count; i++)
                {
                    Expression expression = expressions[i];
                    int expressionID = expression.Id;
                    String expressionContent = expression.Content;
                    Jira jira = rows[j].Tag as Jira;
                    if (expression.Name == "ID")
                    {
                        if (expression.Content != jira.JiraID)
                        {
                            flag = false;
                            break;
                        }
                    }
                    else if (expression.Name == "Title")
                    {
                        if (!expression.Content.Contains(jira.Title))
                        {
                            flag = false;
                            break;
                        }
                    }
                    else if (expression.Name == "Developer")
                    {
                        if (!expression.Content.Contains(jira.Developer))
                        {
                            flag = false;
                            break;
                        }
                    }
                    else if (expression.Name == "DevelopePass")
                    {
                        if (expression.Content != rows[j].GetCell("colT8").Text)
                        {
                            flag = false;
                            break;
                        }
                    }
                    else if (expression.Name == "TestPass")
                    {
                        if (expression.Content != rows[j].GetCell("colT10").Text)
                        {
                            flag = false;
                            break;
                        }
                    }
                    else if (expression.Name == "WaitPublish")
                    {
                        if (expression.Content != rows[j].GetCell("colT12").Text)
                        {
                            flag = false;
                            break;
                        }
                    }
                    else if (expression.Name == "Publish")
                    {
                        if (expression.Content != rows[j].GetCell("colT13").Text)
                        {
                            flag = false;
                            break;
                        }
                    }
                    else if (expression.Name == "CloseTask")
                    {
                        if (expression.Content != rows[j].GetCell("colT14").Text)
                        {
                            flag = false;
                            break;
                        }
                    }
                    else if (expression.Name == "ProductPass")
                    {
                        if (expression.Content != rows[j].GetCell("colT11").Text)
                        {
                            flag = false;
                            break;
                        }
                    }
                    else if (expression.Name == "StartDate")
                    {
                        long expressionDate = Convert.ToDateTime(expression.Content).ToFileTime();
                        DateTime colt6 = (DateTime)(rows[j].GetCell("colT16").Tag);
                        long cellDate = colt6.ToFileTime();
                        if (expression.Str == "=")
                        {
                            if (expressionDate == cellDate)
                            {
                                flag = false;
                                break;
                            }
                        }
                        if (expression.Str == ">=")
                        {
                            if (expressionDate >= cellDate)
                            {
                                flag = false;
                                break;
                            }
                        }
                        if (expression.Str == "<=")
                        {
                            if (expressionDate <= cellDate)
                            {
                                flag = false;
                                break;
                            }
                        }
                        if (expression.Str == ">")
                        {
                            if (expressionDate > cellDate)
                            {
                                flag = true;
                                break;
                            }
                        }
                        if (expression.Str == "<")
                        {
                            if (expressionDate < cellDate)
                            {
                                flag = true;
                                break;
                            }
                        }
                        if (expression.Str == "<>")
                        {
                            if (expressionDate != cellDate)
                            {
                                flag = true;
                                break;
                            }
                        }

                    }
                    else if (expression.Name == "EndDate")
                    {
                        long expressionDate = Convert.ToDateTime(expression.Content).ToFileTime();
                        DateTime colt7 = (DateTime)(rows[j].GetCell("colT17").Tag);
                        long cellDate = colt7.ToFileTime();
                        if(expression.Str == "=")
                        {
                            if (expressionDate == cellDate)
                            {
                                flag = false;
                                break;
                            }
                        }
                        if (expression.Str == ">=")
                        {
                            if (expressionDate >= cellDate)
                            {
                                flag = true;
                                break;
                            }
                        }
                        if (expression.Str == "<=")
                        {
                            if (expressionDate <= cellDate)
                            {
                                flag = false;
                                break;
                            }
                        }
                        if (expression.Str == ">")
                        {
                            if (expressionDate > cellDate)
                            {
                                flag = true;
                                break;
                            }
                        }
                        if (expression.Str == "<")
                        {
                            if (expressionDate < cellDate)
                            {
                                flag = false;
                                break;
                            }
                        }
                        if (expression.Str == "<>")
                        {
                            if (expressionDate != cellDate)
                            {
                                flag = false;
                                break;
                            }
                        }
                        
                    }   
                }
                if (flag)
                {
                    rows[j].Visible = true;//��������
                }
                else
                {
                    rows[j].Visible = false;
                }
            }
            m_gridDgvTable.Update();
            m_gridDgvTable.Invalidate();
        }

        ~MainFrame()
        {
            m_gridDgvTable.StopTimer(m_timerID);
        }
    }

    /// <summary>
    /// �������չ
    /// </summary>
    public class GridCellExp : GridStringCell
    {
        public GridCellExp()
        { 
        }

        public GridCellExp(String text)
            : base(text)
        {
            
        }

        /// <summary>
        /// �ػ���
        /// </summary>
        /// <param name="paint"></param>
        /// <param name="rect"></param>
        /// <param name="clipRect"></param>
        /// <param name="isAlternate"></param>
        public override void OnPaint(CPaint paint, RECT rect, RECT clipRect, bool isAlternate)
        {
            rect.left += 2;
            rect.right += 2;
            RECT cRect = new RECT(rect.left, rect.top - 1, rect.right, rect.bottom + 1);
            GridCellStyle style = Style;
            if (style == null)
            {
                style = new GridCellStyle();
            }
            paint.FillRect(style.BackColor, cRect);
            String text = Text;
            FONT font = m_grid.Font;
            SIZE tSize = paint.TextSize(text, font);
            RECT tRect = new RECT();
            //����Ĭ�Ͼ���
            tRect.left = cRect.left + 1;
            tRect.right = cRect.left + tSize.cx;
            tRect.top = ((cRect.bottom - cRect.top) / 2 + cRect.top) - tSize.cy / 2;
            tRect.bottom = ((cRect.bottom - cRect.top) / 2 + cRect.top) + tSize.cy / 2;
            HorizontalAlignA align = style.Align;

            if(align == HorizontalAlignA.Center)
            {
                tRect.left = ((cRect.right - cRect.left) / 2 + cRect.left) - tSize.cx / 2;
                tRect.right = ((cRect.right - cRect.left) / 2 + cRect.left) + tSize.cx / 2;
            }
            if(align == HorizontalAlignA.Right)
            {
                tRect.left = (cRect.right - tSize.cx);
                tRect.right = cRect.right - 1;
            }
            paint.DrawText(text, style.ForeColor, font, tRect);
            int lw = cRect.right - cRect.left;
            int lh = cRect.bottom - cRect.top;
            paint.DrawLine(COLOR.ARGB(0, 0, 255), 1, 0, new POINT(cRect.left, cRect.top), new POINT(cRect.right, cRect.top));
        }
    }
}

