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
using System.Diagnostics;

namespace OwLib
{
    /// <summary>
    /// ���ϱ༭����
    /// </summary>
    public class EditWindow : WindowXmlEx
    {
        #region Lord 2016/1/8
        /// <summary>
        /// ��������(���)
        /// </summary>
        /// <param name="native">������</param>
        public EditWindow(INativeBase native)
        {
            Load(native, "EditWindow", "collection");
            //ע�����¼�
            RegisterEvents(m_window);
            //��ȡֵ
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
            //�������
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

        //ȡ����ť
        private ButtonA m_btnCancel;
        //ǰ��ָ����ַ
        private ButtonA m_btnGo;
        //ѡȡ��ť
        private ButtonA m_btnSelectDeveloper;
        //�ύ��ť
        private ButtonA m_btnSubmit;
        //��ѡ��ر�����
        private CheckBoxA m_chbCloseTask;
        //��ѡ�򿪷���ȡ
        private CheckBoxA m_chbDeveloperReceive;
        //��ѡ�򿪷����
        private CheckBoxA m_chbDeveloperPass;
        //��ѡ���Ʒͨ��
        private CheckBoxA m_chbProductPass;
        //��ѡ���Ѿ�����
        private CheckBoxA m_chbPublished;
        //��ѡ�����ͨ��
        private CheckBoxA m_chbTestPass;
        //��ѡ��׼������
        private CheckBoxA m_chbWaitPublish;
        //����������
        private ComboBoxA m_cmbCategory;
        //����������
        private ComboBoxA m_cmbGroup;
        //�����̶�������
        private ComboBoxA m_cmbHurry;
        //����ʱ��
        private DateTimePickerA m_dtpEnd;
        //��ʼʱ��
        private DateTimePickerA m_dtpStart;
        //��֧��
        private TextBoxA m_txtBranches;
        //�����˿�
        private TextBoxA m_txtCreater;
        //������
        private TextBoxA m_rtbDescription;
        //�����߿�
        private TextBoxA m_txtDeveloper;
        //��ַ��
        private TextBoxA m_txtHttpPath;
        //JiraId��
        private TextBoxA m_txtJiraID;
        //������
        private TextBoxA m_txtRelativeGroup;
        //�����
        private TextBoxA m_txtTitle;

        private bool m_isAdd;

        /// <summary>
        /// �Ƿ����
        /// </summary>
        public bool IsAdd
        {
            get { return m_isAdd; }
            set { m_isAdd = value; }
        }
        private bool m_isOverride;

        /// <summary>
        /// ��ȡ�������Ƿ񸲸�
        /// </summary>
        public bool IsOverride
        {
            get { return m_isOverride; }
            set { m_isOverride = value; }
        }

        private Jira m_jira;

        /// <summary>
        /// ��ȡ�����õ�ǰ�༭��Jira
        /// </summary>
        public Jira Jira
        {
            get { return m_jira; }
            set { m_jira = value; }
        }

        private MainFrame m_mainFrame;

        /// <summary>
        /// ��ȡ����MainFrame
        /// </summary>
        public MainFrame MainFrame
        {
            get { return m_mainFrame; }
            set { m_mainFrame = value; }
        }

        /// <summary>
        /// �������
        /// </summary>
        private void AddData()
        {
            if (m_txtJiraID.Text == null || m_txtJiraID.Text.Length == 0)
            {
                MessageBox.Show("ID����Ϊ��,����дID!");
                return;
            }
            if (m_txtCreater.Text == null || m_txtCreater.Text.Length == 0)
            {
                MessageBox.Show("�����˲���Ϊ�գ�����д������!");
                return;
            }
            if (m_txtTitle.Text == null || m_txtTitle.Text.Length == 0)
            {
                MessageBox.Show("����������Ϊ�գ�����д����!");
                return;
            }
            //��������
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
            m_jira.StartDate = Convert.ToDateTime(this.m_dtpStart.Text);//c#�ַ���תdatatime
            m_jira.Title = this.m_txtTitle.Text;
            m_jira.TestPass = this.m_chbTestPass.Checked;
            m_jira.WaitPublish = this.m_chbWaitPublish.Checked;
            //����ID
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
            //����ID
            try
            {
                m_jira.CategoryID = (m_cmbCategory.GetItems()[this.m_cmbCategory.SelectedIndex].Tag as JCategory).Id;
            }
            catch (Exception ex)
            {
                ErrorException.OnError(ex);
            }
            //������
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
                    if (DialogResult.OK != MessageBox.Show("ID�Ѵ���,�Զ�����Ϊ���µ�ID,�������ύ!", "��ʾ", MessageBoxButtons.OKCancel, MessageBoxIcon.Question))
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
                if (name == "btnCancel")
                {
                    this.Close();
                }
                else if (name == "btnGo")
                {
                    try
                    {
                        Process.Start(this.m_txtHttpPath.Text);//ǰ��ָ����ַ
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
        /// ����Jira
        /// </summary>
        /// <param name="date">����</param>
        /// <param name="jira">Jira</param>
        /// <param name="jiras">Jira�б�</param>
        public void Create(Jira jira)
        {
            //��ʼ������
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
                //��ʼ��������Ϣ
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
                //���ڸ�ʽ��
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
                //ѡ�������б�
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
                //����Jira����
                this.m_jira = new Jira();
                this.m_jira.CreateDate = DateTime.Now;
                //Ĭ����ʾ��ǰ����
                m_dtpEnd.Text = DateTime.Now.ToString("D");
                m_dtpStart.Text = DateTime.Now.ToString("D");
                try
                {
                    //���ݻ���ѡ�������б�
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
                        //Ĭ��ѡ��
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
        /// ��ֵ�ı��¼�
        /// </summary>
        /// <param name="sender">������</param>
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
        /// �޸Ĵ����ȡ��
        /// </summary>
        public void Update()
        {
            //��ֵ���ı���
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


