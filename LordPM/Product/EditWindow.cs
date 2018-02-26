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
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;

namespace OwLib
{
    /// <summary>
    /// 集合编辑界面
    /// </summary>
    public class EditWindow : WindowXmlEx
    {
        #region Lord 2016/1/8
        /// <summary>
        /// 创建界面(添加)
        /// </summary>
        /// <param name="native">方法库</param>
        public EditWindow(INativeBase native)
        {
            Load(native, "EditWindow", "collection");
            //注册点击事件
            RegisterEvents(m_window);
            //获取值
            m_btnCancel = GetButton("btnCancel");
            m_btnGo = GetButton("btnGo");
            m_btnSelectDeveloper = GetButton("btnSelectDeveloper");
            m_btnSubmit = GetButton("btnSubmit");
            m_chbDeveloperPass = GetCheckBox("cbDeveloperPass");
            m_chbCloseTask = GetCheckBox("cbCloseTask");
            m_chbDeveloperReceive = GetCheckBox("cbDeveloperReceive");
            m_chbTestPass = GetCheckBox("cbTestPass");
            m_chbProductPass = GetCheckBox("cbProductPass");
            m_chbPublished = GetCheckBox("cbPublished");
            m_chbPublished = GetCheckBox("cbPublished");
            m_chbWaitPublish = GetCheckBox("cbWaitPublish");
            m_cmbCategory = GetComboBox("cbCategory");
            m_cmbGroup = GetComboBox("cbGroup");
            //组的联动
            m_cmbGroup.RegisterEvent(new ControlEvent(SelectedIndexChangedEvent), EVENTID.SELECTEDINDEXCHANGED);
            m_cmbHurry = GetComboBox("cbHurry");
            m_dtpEnd = GetDatePicker("dtpEnd");
            m_dtpStart = GetDatePicker("dtpStart");
            m_rtbDescription = GetTextBox("rtbDescription");
            m_txtBranches = GetTextBox("rtbBranches");
            m_txtCreater = GetTextBox("txtCreater");
            m_txtDeveloper = GetTextBox("txtDeveloper");
            m_txtHttpPath = GetTextBox("txtHttpPath");
            m_txtJiraID = GetTextBox("txtJiraID");
            m_txtRelativeGroup = GetTextBox("txtRelativeGroup");
            m_txtTitle = GetTextBox("txtTitle");
        }

        //取消按钮
        private ButtonA m_btnCancel;
        //前往指定地址
        private ButtonA m_btnGo;
        //选取按钮
        private ButtonA m_btnSelectDeveloper;
        //提交按钮
        private ButtonA m_btnSubmit;
        //多选框关闭任务
        private CheckBoxA m_chbCloseTask;
        //多选框开发领取
        private CheckBoxA m_chbDeveloperReceive;
        //多选框开发完成
        private CheckBoxA m_chbDeveloperPass;
        //多选框产品通过
        private CheckBoxA m_chbProductPass;
        //多选框已经上线
        private CheckBoxA m_chbPublished;
        //多选框测试通过
        private CheckBoxA m_chbTestPass;
        //多选框准备上线
        private CheckBoxA m_chbWaitPublish;
        //分类下拉框
        private ComboBoxA m_cmbCategory;
        //分组下拉框
        private ComboBoxA m_cmbGroup;
        //紧急程度下拉框
        private ComboBoxA m_cmbHurry;
        //结束时间
        private DateTimePickerA m_dtpEnd;
        //开始时间
        private DateTimePickerA m_dtpStart;
        //分支框
        private TextBoxA m_txtBranches;
        //发布人框
        private TextBoxA m_txtCreater;
        //描述框
        private TextBoxA m_rtbDescription;
        //开发者框
        private TextBoxA m_txtDeveloper;
        //地址栏
        private TextBoxA m_txtHttpPath;
        //JiraId框
        private TextBoxA m_txtJiraID;
        //相关组框
        private TextBoxA m_txtRelativeGroup;
        //标题框
        private TextBoxA m_txtTitle;

        private bool m_isAdd;

        /// <summary>
        /// 是否添加
        /// </summary>
        public bool IsAdd
        {
            get { return m_isAdd; }
            set { m_isAdd = value; }
        }
        private bool m_isOverride;

        /// <summary>
        /// 获取或设置是否覆盖
        /// </summary>
        public bool IsOverride
        {
            get { return m_isOverride; }
            set { m_isOverride = value; }
        }

        private Jira m_jira;

        /// <summary>
        /// 获取或设置当前编辑的Jira
        /// </summary>
        public Jira Jira
        {
            get { return m_jira; }
            set { m_jira = value; }
        }

        private MainFrame m_mainFrame;

        /// <summary>
        /// 获取设置MainFrame
        /// </summary>
        public MainFrame MainFrame
        {
            get { return m_mainFrame; }
            set { m_mainFrame = value; }
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        private void AddData()
        {
            if (m_txtJiraID.Text == null || m_txtJiraID.Text.Length == 0)
            {
                MessageBox.Show("ID不能为空,请填写ID!");
                return;
            }
            if (m_txtCreater.Text == null || m_txtCreater.Text.Length == 0)
            {
                MessageBox.Show("发布人不能为空，请填写发布人!");
                return;
            }
            if (m_txtTitle.Text == null || m_txtTitle.Text.Length == 0)
            {
                MessageBox.Show("标题栏不能为空，请填写标题!");
                return;
            }
            //设置配置
            m_jira = new Jira();
            m_jira.Branches = this.m_txtBranches.Text;
            m_jira.CloseTask = this.m_chbCloseTask.Checked;
            m_jira.Creater = this.m_txtCreater.Text;
            m_jira.Description = this.m_rtbDescription.Text;
            m_jira.Developer = this.m_txtDeveloper.Text;
            m_jira.DeveloperPass = this.m_chbDeveloperPass.Checked;
            m_jira.DeveloperReceive = this.m_chbDeveloperReceive.Checked;
            m_jira.EndDate = Convert.ToDateTime(this.m_dtpEnd.Text);
            m_jira.HttpPath = this.m_txtHttpPath.Text;
            m_jira.Hurry = this.m_cmbHurry.Text;
            m_jira.JiraID = this.m_txtJiraID.Text;
            m_jira.ModifyDate = DateTime.Now;
            m_jira.ProductPass = this.m_chbProductPass.Checked;
            m_jira.Published = this.m_chbPublished.Checked;
            m_jira.RelativeGroup = this.m_txtRelativeGroup.Text;
            m_jira.StartDate = Convert.ToDateTime(this.m_dtpStart.Text);//c#字符串转datatime
            m_jira.Title = this.m_txtTitle.Text;
            m_jira.TestPass = this.m_chbTestPass.Checked;
            m_jira.WaitPublish = this.m_chbWaitPublish.Checked;
            //分组ID
            try
            {
                m_jira.GroupID = (m_cmbGroup.GetItems()[this.m_cmbGroup.SelectedIndex].Tag as JGroup).Id;
                string cachePath = Path.Combine(XmlHandle.UserDir, "SetCache.txt");
                File.WriteAllText(cachePath, m_jira.GroupID, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                ErrorException.OnError(ex);
            }
            //分类ID
            try
            {
                m_jira.CategoryID = (m_cmbCategory.GetItems()[this.m_cmbCategory.SelectedIndex].Tag as JCategory).Id;
            }
            catch (Exception ex)
            {
                ErrorException.OnError(ex);
            }
            //检查添加
            if (m_isAdd)
            {
                Jira checkJira = XmlHandle.GetJira(m_jira.JiraID);
                String isOk = HttpGetService.Get(DataCenter.ServerAddr + "/giraservice?func=checkid&gid=" + m_jira.JiraID);
                if (isOk != null && isOk == "0")
                {
                    m_isOverride = false;
                }
                else
                {
                    if (DialogResult.OK != MessageBox.Show("ID已存在,自动更换为最新的ID,请重新提交!", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question))
                    {
                        String strJiraID = HttpGetService.Get(DataCenter.ServerAddr + "giraservice?func=availablenum");
                        int maxJiraID = 0;
                        int.TryParse(strJiraID, out maxJiraID);
                        this.m_txtJiraID.Text = maxJiraID.ToString();
                        return;
                    }
                }
            }
            m_mainFrame.AddOrUpdate(m_jira);
            Close();
        }

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
                if (name == "btnCancel")
                {
                    this.Close();
                }
                else if (name == "btnGo")
                {
                    try
                    {
                        Process.Start(this.m_txtHttpPath.Text);//前往指定地址
                    }
                    catch (Exception ex)
                    {
                        ErrorException.OnError(ex);
                    }
                }
                else if (name == "btnSelectDeveloper")
                {
                }
                else if (name == "btnSubmit")
                {
                    AddData();
                }
            }
        }

        /// <summary>
        /// 创建Jira
        /// </summary>
        /// <param name="date">日期</param>
        /// <param name="jira">Jira</param>
        /// <param name="jiras">Jira列表</param>
        public void Create(Jira jira)
        {
            //初始化分组
            int count = XmlHandle.Groups.Count;
            for (int i = 0; i < count; i++)
            {
                MenuItemA item = new MenuItemA();
                item.Text = XmlHandle.Groups[i].Name;
                item.Tag = XmlHandle.Groups[i];
                this.m_cmbGroup.AddItem(item);
            }
            if (jira != null)
            {
                //初始化界面信息
                this.m_chbCloseTask.Checked = jira.CloseTask;
                this.m_chbDeveloperPass.Checked = jira.DeveloperPass;
                this.m_chbDeveloperReceive.Checked = jira.DeveloperReceive;
                this.m_cmbHurry.Text = jira.Hurry;
                this.m_chbProductPass.Checked = jira.ProductPass;
                this.m_chbPublished.Checked = jira.Published;
                this.m_chbTestPass.Checked = jira.TestPass;
                this.m_chbWaitPublish.Checked = jira.WaitPublish;
                this.m_jira = jira;
                this.m_txtBranches.Text = jira.Branches;
                this.m_rtbDescription.Text = jira.Description;
                this.m_txtCreater.Text = jira.Creater;
                this.m_txtDeveloper.Text = jira.Developer;
                this.m_txtHttpPath.Text = jira.HttpPath;
                this.m_txtJiraID.Text = jira.JiraID;
                this.m_txtRelativeGroup.Text = jira.RelativeGroup;
                this.m_txtTitle.Text = jira.Title;
                //日期格式化
                try
                {
                    this.m_dtpStart.Text = jira.StartDate.ToString("D");
                }
                catch (Exception ex)
                {
                    ErrorException.OnError(ex);
                }
                try
                {
                    this.m_dtpEnd.Text = jira.EndDate.ToString("D");
                }
                catch (Exception ex)
                {
                    ErrorException.OnError(ex);
                }
		        //int groupCount = XmlHandle.Groups.Count;
                //选中下拉列表
                for (int i = 0; i < count; i++)
                {
                    if (XmlHandle.Groups[i].Id == jira.GroupID)
                    {
                        this.m_cmbGroup.SelectedIndex = i;
                        List<JCategory> categories = XmlHandle.Groups[i].Categories;
			            int categoriesCount = categories.Count;
                        for (int j = 0; j < categoriesCount; j++)
                        {
                            if (categories[j].Id == jira.CategoryID)
                            {
                                this.m_cmbCategory.SelectedIndex = j;
                            }
                        }
                        break;
                    }
                }
            }
            else
            {
                m_isAdd = true;
                //创建Jira对象
                this.m_jira = new Jira();
                this.m_jira.CreateDate = DateTime.Now;
                //默认显示当前日期
                m_dtpEnd.Text = DateTime.Now.ToString("D");
                m_dtpStart.Text = DateTime.Now.ToString("D");
                try
                {
                    //根据缓存选中下拉列表
                    string cachePath = Path.Combine(XmlHandle.UserDir, "SetCache.txt");
                    string groupID = File.Exists(cachePath) ? File.ReadAllText(cachePath, Encoding.UTF8) : "";
                    if (groupID != null && groupID.Length > 0)
                    {
			            //int count = XmlHandle.Groups.Count;  
                        for (int i = 0; i < count; i++)
                        {
                            if (XmlHandle.Groups[i].Id == groupID)
                            {
                                this.m_cmbGroup.SelectedIndex = i;
                                return;
                            }
                        }
                    }
                    else
                    {
                        //默认选中
                        this.m_cmbGroup.SelectedIndex = 0;
                    }
                }
                catch (Exception ex)
                {
                    ErrorException.OnError(ex);
                }
            }
        }

        /// <summary>
        /// 注册事件
        /// </summary>
        /// <param name="control">控件</param>
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
        /// 数值改变事件
        /// </summary>
        /// <param name="sender">调用者</param>
        private void SelectedIndexChangedEvent(object sender)
        {
            MenuItemA item = m_cmbGroup.GetItems()[m_cmbGroup.SelectedIndex];
            JGroup group = item.Tag as JGroup;
            m_cmbCategory.ClearItems();
            int size = group.Categories.Count;
            for (int i = 0; i < size; i++)
            {
                JCategory category = group.Categories[i];
                MenuItemA item1 = new MenuItemA();
                item1.Text = category.Name;
                item1.Tag = category;
                m_cmbCategory.AddItem(item1);
            }
            m_cmbCategory.SelectedIndex = 0;
        }

        /// <summary>
        /// 修改窗体获取行
        /// </summary>
        public void Update()
        {
            //赋值给文本框
            m_chbDeveloperPass.Checked = m_jira.DeveloperPass;
            m_chbDeveloperReceive.Checked = m_jira.DeveloperReceive;
            m_chbCloseTask.Checked = m_jira.CloseTask;
            m_cmbHurry.Text = m_jira.Hurry;
            m_chbProductPass.Checked = m_jira.ProductPass;
            m_chbPublished.Checked = m_jira.Published;
            m_chbTestPass.Checked = m_jira.TestPass;
            m_chbWaitPublish.Checked = m_jira.WaitPublish;
            m_txtBranches.Text = m_jira.Branches;
            m_rtbDescription.Text = m_jira.Description;
            m_txtCreater.Text = m_jira.Creater;
            m_txtDeveloper.Text = m_jira.Developer;
            m_txtHttpPath.Text = m_jira.HttpPath;
            m_txtJiraID.Text = m_jira.JiraID;
            m_txtRelativeGroup.Text = m_jira.RelativeGroup;
            m_txtTitle.Text = m_jira.Title;
        }
        #endregion
    }

}


