/*****************************************************************************\
*                                                                             *
* CollectionWindow.cs - Collection window functions, types, and definitions. *
*                                                                             *
*               Version 1.00  ★★★                                          *
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
using System.Windows.Forms;

namespace OwLib
{
    /// <summary>
    /// 集合编辑界面
    /// </summary>
    public class FilterWindow : WindowXmlEx
    {
        #region Lord 2016/1/8
        /// <summary>
        /// 创建界面
        /// </summary>
        /// <param name="native">方法库</param>
        public FilterWindow(INativeBase native,SearchWindow search)
        {
            Load(native, "FilterWindow", "collection");
            //注册点击事件
            RegisterEvents(m_window);
            m_id = GetLabel("Label1");
            m_title = GetLabel("Label2");
            m_developer = GetLabel("Label3");
            m_published = GetLabel("Label4");
            m_closeTask = GetLabel("Label5");
            m_developePass = GetLabel("Label6");
            m_productPass = GetLabel("Label7");
            m_testPass = GetLabel("Label8");
            m_waitPublish = GetLabel("Label9");
            m_endTime = GetLabel("Label10");
            m_startTime = GetLabel("Label11");
            m_planName = GetLabel("Label12");
            m_dtpStart = GetDatePicker("dtpStartDate");
            m_dtpEnd = GetDatePicker("dtpEndDate");
            m_cbDevelopePass = GetComboBox("cbDeveloperPass");
            m_cbTestPass = GetComboBox("cbTestPass");
            m_cbProductPass = GetComboBox("cbProductPass");
            m_cbWaitPublish = GetComboBox("cbWaitPublish");
            m_cbCloseTask = GetComboBox("cbCloseTask");
            m_cbPublished = GetComboBox("cbPublished");
            m_cbStartDate = GetComboBox("cbStartDate");
            m_cbEndDate = GetComboBox("cbEndDate");
            m_txtId = GetTextBox("txtJiraID");
            m_txtTitle = GetTextBox("txtTitle");
            m_txtDeveloper = GetTextBox("txtDeveloper");
            m_txtPlanName = GetTextBox("txtName");
            m_txtCustomExpression = GetTextBox("rtbCustomExpression");
            m_cbIgnoreStartDate = GetCheckBox("cbIgnoreStartDate");
            //代理注册点击事件

            ControlMouseEvent clickButtonEvent = new ControlMouseEvent(CheckedChangedEvent);
            m_cbIgnoreStartDate.RegisterEvent(clickButtonEvent, EVENTID.CLICK);
            m_cbIgnoreEndDate = GetCheckBox("cbIgnoreEndDate");
            m_cbIgnoreEndDate.RegisterEvent(clickButtonEvent, EVENTID.CLICK);
            m_search = search;
        }
        private LabelA m_id;
        private LabelA m_title;
        private LabelA m_developer;
        private LabelA m_developePass;
        private LabelA m_testPass;
        private LabelA m_productPass;
        private LabelA m_waitPublish;
        private LabelA m_published;
        private LabelA m_closeTask;
        private LabelA m_startTime;
        private LabelA m_endTime;
        private LabelA m_planName;
        private TextBoxA m_txtId;
        private TextBoxA m_txtTitle;
        private TextBoxA m_txtDeveloper;
        private TextBoxA m_txtPlanName;
        private TextBoxA m_txtCustomExpression;
        private ComboBoxA m_cbDevelopePass;
        private ComboBoxA m_cbPublished;
        private ComboBoxA m_cbCloseTask;
        private ComboBoxA m_cbProductPass;
        private ComboBoxA m_cbTestPass;
        private ComboBoxA m_cbWaitPublish;
        private ComboBoxA m_cbStartDate;
        private ComboBoxA m_cbEndDate;
        private DateTimePickerA m_dtpStart;
        private DateTimePickerA m_dtpEnd;
        private SearchWindow m_search;
        private CheckBoxA m_cbIgnoreEndDate;
        private CheckBoxA m_cbIgnoreStartDate;

        /// <summary>
        /// 点击按钮方法
        /// </summary>
        /// <param name="sender">调用者</param>
        /// <param name="mp">坐标</param>
        /// <param name="button">按钮</param>
        /// <param name="click">点击次数</param>
        /// <param name="delta">滚轮滚动值</param>
        private void ClickButton(object sender, POINT mp, MouseButtonsA button, int click, int delta)
        {
            if (button == MouseButtonsA.Left && click == 1)
            {
                ControlA control = sender as ControlA;
                String name = control.Name;
                if (name == "btnSubmit")
                {
                    Expression();
                }
                if (name == "btnCancel")
                {
                    CloseWindow();
                }
            }
        }

        /// <summary>
        /// 选中改变事件
        /// </summary>
        /// <param name="sender">调用者</param>
        /// 
        private void CheckedChangedEvent(object sender, POINT mp, MouseButtonsA button, int clicks, int delta)
        {
            if (button == MouseButtonsA.Left && clicks == 1)
            {
                ControlA control = sender as ControlA;
                String name = control.Name;
                if (name == "btnSubmit")
                {
                    Expression();
                }
                if (name == "cbIgnoreStartDate")
                {
                    if (m_cbIgnoreStartDate.Checked)
                    {
                        m_cbStartDate.Enabled = false;
                        m_dtpStart.Enabled = false;
                    }
                    else
                    {
                        m_cbStartDate.Enabled = true;
                        m_dtpStart.Enabled = true;
                    }
                    m_cbStartDate.Invalidate();
                    m_dtpStart.Invalidate();
                }
                if (name == "cbIgnoreEndDate")
                {
                    if (m_cbIgnoreEndDate.Checked)
                    {

                        m_cbEndDate.Enabled = false;//只读状态
                        m_dtpEnd.Enabled = false;
                    }
                    else
                    {
                        m_cbEndDate.Enabled = true;//只读状态
                        m_dtpEnd.Enabled = true;
                    }
                    m_cbEndDate.Invalidate();
                    m_dtpEnd.Invalidate();
                }
            }
        }

        /// <summary>
        /// 关闭当前窗体
        /// </summary>
        public void CloseWindow()
        {
            this.Close();
        }

        /// <summary>
        /// 注册事件
        /// </summary>
        /// <param name="control">控件</param>
        private void RegisterEvents(ControlA control)
        {
            ControlMouseEvent clickButtonEvent = new ControlMouseEvent(ClickButton);//代理
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
        /// 查询表达式
        /// </summary>
        public void Expression()
        {
            List<Expression> list = new List<Expression>();
            if (m_id.Text != null && m_txtId.Text.Length != 0)
            {
                Expression exp = new Expression("ID", "like", m_txtId.Text);
                list.Add(exp);
            }
            if(m_title.Text != null && m_txtTitle.Text.Length != 0)
            {
                Expression exp = new Expression("Title", "like", m_txtTitle.Text);
                list.Add(exp);
            }
            if(m_developer != null && m_txtDeveloper.Text.Length != 0)
            {
                Expression exp = new Expression("Developer", "like", m_txtDeveloper.Text);
                list.Add(exp);
            }
            if (m_developer != null && m_cbDevelopePass.Text.Length != 0)
            {
                Expression exp = new Expression("DevelopePass", "like", m_cbDevelopePass.Text);
                list.Add(exp);
            }
            if (m_testPass != null && m_cbTestPass.Text.Length != 0)
            {
                Expression exp = new Expression("TestPass", "like", m_cbTestPass.Text);
                list.Add(exp);
            }
            if (m_waitPublish != null && m_cbWaitPublish.Text.Length != 0)
            {
                Expression exp = new Expression("WaitPublish", "like", m_cbWaitPublish.Text);
                list.Add(exp);
            }
            if (m_published != null && m_cbPublished.Text.Length != 0)
            {
                Expression exp = new Expression("Publish", "like", m_cbPublished.Text);
                list.Add(exp);
            }
            if (m_closeTask != null && m_cbCloseTask.Text.Length != 0)
            {
                Expression exp = new Expression("CloseTask", "like", m_cbCloseTask.Text);
                list.Add(exp);
            }
            if (m_startTime != null && m_dtpStart.Text.Length != 0)
            {
                Expression exp = new Expression("StartDate", m_cbStartDate.Text, m_dtpStart.Text);
                list.Add(exp);
            }
            if (m_endTime != null && m_dtpEnd.Text.Length != 0)
            {
                Expression exp = new Expression("EndDate", m_cbEndDate.Text, m_dtpEnd.Text);
                list.Add(exp);
            }
            if (m_txtPlanName.Text != null && m_txtPlanName.Text.Length != 0)
            {
                m_search.AddData(list, m_txtPlanName.Text);
                this.Close();
            }
            else 
            {
                MessageBox.Show("名称不能为空！");
            }
        }
        #endregion
    }
}
