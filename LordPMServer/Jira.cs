using System;
using System.Collections;
using System.ComponentModel;
using System.Collections.Generic;
using System.Xml;
using System.Text;

namespace OwLibSV
{
    public class Jira
    {
        private string m_branches;

        /// <summary>
        /// 获取或设置分支地址
        /// </summary>
        public string  Branches
        {
            get { return m_branches; }
            set { m_branches = value; }
        }

        private string m_categoryID;

        /// <summary>
        /// 获取或设置类别ID
        /// </summary>
        public string CategoryID
        {
            get { return m_categoryID; }
            set { m_categoryID = value; }
        }

        private bool m_closeTask;

        /// <summary>
        /// 关闭任务
        /// </summary>
        public bool CloseTask
        {
            get { return m_closeTask; }
            set { m_closeTask = value; }
        }

        private DateTime m_createDate = DateTime.Now;

        /// <summary>
        /// 获取或设置创建日期
        /// </summary>
        public DateTime CreateDate
        {
            get { return m_createDate; }
            set { m_createDate = value; }
        }

        private string m_creater;

        /// <summary>
        /// 获取或设置创建者
        /// </summary>
        public string Creater
        {
            get { return m_creater; }
            set { m_creater = value; }
        }

        private string m_description;

        /// <summary>
        /// 获取或设置描述
        /// </summary>
        public string Description
        {
            get { return m_description; }
            set { m_description = value; }
        }

        private string m_developer;

        /// <summary>
        /// 获取或设置开发者
        /// </summary>
        public string Developer
        {
            get { return m_developer; }
            set { m_developer = value; }
        }

        private bool m_developerPass;

        /// <summary>
        /// 获取或设置开发者是否通过
        /// </summary>
        public bool DeveloperPass
        {
            get { return m_developerPass; }
            set { m_developerPass = value; }
        }

        private bool m_developerReceive;

        /// <summary>
        /// 获取或设置开发者是否接受
        /// </summary>
        public bool DeveloperReceive
        {
            get { return m_developerReceive; }
            set { m_developerReceive = value; }
        }

        private DateTime m_endDate = DateTime.Now;

        /// <summary>
        /// 获取或设置结束日期
        /// </summary>
        public DateTime EndDate
        {
            get { return m_endDate; }
            set { m_endDate = value; }
        }

        private string m_groupID;

        /// <summary>
        /// 获取或设置组ID
        /// </summary>
        public string GroupID
        {
            get { return m_groupID; }
            set { m_groupID = value; }
        }

        private string m_httpPath;

        /// <summary>
        /// 获取或设置HTTP地址
        /// </summary>
        public string HttpPath
        {
            get { return m_httpPath; }
            set { m_httpPath = value; }
        }

        private String m_hurry;

        /// <summary>
        /// 获取或设置是否紧急
        /// </summary>
        public String Hurry
        {
            get { return m_hurry; }
            set { m_hurry = value; }
        }

        private string m_jiraID;

        /// <summary>
        /// 获取或设置唯一编号
        /// </summary>
        public string JiraID
        {
            get { return m_jiraID; }
            set { m_jiraID = value; }
        }

        private DateTime m_modifyDate = DateTime.Now;

        /// <summary>
        /// 获取或设置修改日期
        /// </summary>
        public DateTime ModifyDate
        {
            get { return m_modifyDate; }
            set { m_modifyDate = value; }
        }

        private bool m_productPass;

        /// <summary>
        /// 获取或设置产品是否通过
        /// </summary>
        public bool ProductPass
        {
            get { return m_productPass; }
            set { m_productPass = value; }
        }

        private string m_relativeGroup;

        /// <summary>
        /// 获取或设置相关组
        /// </summary>
        public string RelativeGroup
        {
            get { return m_relativeGroup; }
            set { m_relativeGroup = value; }
        }

        private DateTime m_startDate = DateTime.Now;

        /// <summary>
        /// 获取或设置开始日期
        /// </summary>
        public DateTime StartDate
        {
            get { return m_startDate; }
            set { m_startDate = value; }
        }

        private bool m_testPass;

        /// <summary>
        /// 获取或设置测试是否通过
        /// </summary>
        public bool TestPass
        {
            get { return m_testPass; }
            set { m_testPass = value; }
        }

        private string m_title;

        /// <summary>
        /// 获取或设置标题
        /// </summary>
        public string Title
        {
            get { return m_title; }
            set { m_title = value; }
        }

        private bool m_waitPublish;

        /// <summary>
        /// 获取或设置等待上线
        /// </summary>
        public bool WaitPublish
        {
            get { return m_waitPublish; }
            set { m_waitPublish = value; }
        }

        private bool m_published;

        /// <summary>
        /// 获取或设置是否已经上线
        /// </summary>
        public bool Published
        {
            get { return m_published; }
            set { m_published = value; }
        }
    }
}
