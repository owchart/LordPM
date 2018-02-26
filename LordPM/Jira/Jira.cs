using System;
using System.Collections;
using System.ComponentModel;
using System.Collections.Generic;
using System.Xml;
using System.Text;

namespace OwLib
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

        public string GetXml()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<Jira>\r\n");
            sb.Append("<Branches>" + (m_branches != null ? m_branches : "") + "</Branches>\r\n");
            sb.Append("<CategoryID>" + (m_categoryID != null ? m_categoryID : "") + "</CategoryID>\r\n");
            sb.Append("<CreateDate>" + m_createDate.ToString("yyyy-MM-dd") + "</CreateDate>\r\n");
            sb.Append("<Description><![CDATA[" + (m_description != null ? m_description : "") + "]]></Description>\r\n");
            sb.Append("<Developer>" + (m_developer != null ? m_developer : "") + "</Developer>\r\n");
            sb.Append("<EndDate>" + m_endDate.ToString("yyyy-MM-dd") + "</EndDate>\r\n");
            sb.Append("<GroupID>" + (m_groupID != null ? m_groupID : "") + "</GroupID>\r\n");
            sb.Append("<HttpPath>" + (m_httpPath != null ? m_httpPath : "") + "</HttpPath>\r\n");
            sb.Append("<Hurry>" + (m_hurry != null ? m_hurry : "") + "</Hurry>\r\n");
            sb.Append("<JiraID>" + (m_jiraID != null ? m_jiraID : "") + "</JiraID>\r\n");
            sb.Append("<ModifyDate>" + m_modifyDate.ToString() + "</ModifyDate>\r\n");
            sb.Append("<PassTest>" + (m_productPass ? "1" : "0") + "</PassTest>\r\n");
            sb.Append("<RelativeGroup>" + (m_relativeGroup != null ? m_relativeGroup : "") + "</RelativeGroup>\r\n");
            sb.Append("<StartDate>" + m_startDate.ToString("yyyy-MM-dd") + "</StartDate>\r\n");
            sb.Append("<Title><![CDATA[" + (m_title != null ? m_title : "") + "]]></Title>\r\n");
            sb.Append("<TestPass>" + (m_testPass ? "1" : "0") + "</TestPass>\r\n");
            sb.Append("<DeveloperReceive>" + (m_developerReceive ? "1" : "0") + "</DeveloperReceive>\r\n");
            sb.Append("<DeveloperPass>" + (m_developerPass ? "1" : "0") + "</DeveloperPass>\r\n");
            sb.Append("<ProductPass>" + (m_productPass ? "1" : "0") + "</ProductPass>\r\n");
            sb.Append("<WaitPublish>" + (m_waitPublish ? "1" : "0") + "</WaitPublish>\r\n");
            sb.Append("<Published>" + (m_published ? "1" : "0") + "</Published>\r\n");
            sb.Append("<CloseTask>" + (m_closeTask ? "1" : "0") + "</CloseTask>\r\n");
            sb.Append("<Creater>" + (m_creater != null ? m_creater : "") + "</Creater>\r\n");
            sb.Append("</Jira>\r\n");
            return sb.ToString();
        }

        public void ReadXml(XmlNode node)
        {
            foreach (XmlNode subNode in node.ChildNodes)
            {
                string nodeName = subNode.Name.ToUpper();
                string nodeText = subNode.InnerText.Trim();
                if (nodeText.StartsWith("\n"))
                {
                    nodeText = nodeText.Substring(1);
                }
                switch (nodeName)
                {
                    case "BRANCHES":
                        m_branches = nodeText;
                        break;
                    case "CATEGORYID":
                        m_categoryID = nodeText;
                        break;
                    case "CREATEDATE":
                        DateTime.TryParse(nodeText, out m_createDate);
                        break;
                    case "CREATER":
                        m_creater = nodeText;
                        break;
                    case "DESCRIPTION":
                        m_description = nodeText;
                        break;
                    case "DEVELOPER":
                        m_developer = nodeText;
                        break;
                    case "ENDDATE":
                        DateTime.TryParse(nodeText, out m_endDate);
                        break;
                    case "GROUPID":
                        m_groupID = nodeText;
                        break;
                    case "HTTPPATH":
                        m_httpPath = nodeText;
                        break;
                    case "HURRY":
                        m_hurry = nodeText;
                        break;
                    case "JIRAID":
                        m_jiraID = nodeText;
                        break;
                    case "TESTPASS":
                        if (nodeText == "1")
                        {
                            m_testPass = true;
                        }
                        else
                        {
                            m_testPass = false;
                        }
                        break;
                    case "DEVELOPERPASS":
                        if (nodeText == "1")
                        {
                            m_developerPass = true;
                        }
                        else
                        {
                            m_developerPass = false;
                        }
                        break;
                    case "DEVELOPERRECEIVE":
                        if (nodeText == "1")
                        {
                            m_developerReceive = true;
                        }
                        else
                        {
                            m_developerReceive = false;
                        }
                        break;
                    case "CLOSETASK":
                        if (nodeText == "1")
                        {
                            m_closeTask = true;
                        }
                        else
                        {
                            m_closeTask = false;
                        }
                        break;
                    case "MODIFYDATE":
                        DateTime.TryParse(nodeText, out m_modifyDate);
                        break;
                    case "PRODUCTPASS":
                        if (nodeText == "1")
                        {
                            m_productPass = true;
                        }
                        else
                        {
                            m_productPass = false;
                        }
                        break;
                    case "RELATIVEGROUP":
                        m_relativeGroup = nodeText;
                        break;
                    case "STARTDATE":
                        DateTime.TryParse(nodeText, out m_startDate);
                        break;
                    case "TITLE":
                        m_title = nodeText;
                        break;
                    case "WAITPUBLISH":
                        if (nodeText == "1")
                        {
                            m_waitPublish = true;
                        }
                        else
                        {
                            m_waitPublish = false;
                        }
                        break;
                    case "PUBLISHED":
                        if (nodeText == "1")
                        {
                            Published = true;
                        }
                        else
                        {
                            Published = false;
                        }
                        break;
                }
            }
        }
    }
}
