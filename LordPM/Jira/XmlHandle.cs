using System;
using System.Collections;
using System.ComponentModel;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Text;
using System.Reflection;
using System.Data;
using System.Windows.Forms;
using OwLib;

namespace OwLib
{
    public class XmlHandle
    {
        static XmlHandle()
        {
            m_userDir = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "user");
            if (!Directory.Exists(m_userDir))
            {
                Directory.CreateDirectory(m_userDir);
            }
            m_dataDirPath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "data");
            if (File.Exists(Path.Combine(m_userDir, "DataPath.txt")))
            {
                try
                {
                    string newDataDirPath = File.ReadAllText(Path.Combine(m_userDir, "DataPath.txt"), Encoding.UTF8);
                    if (Directory.Exists(newDataDirPath))
                    {
                        m_dataDirPath = newDataDirPath;
                    }
                }
                catch (Exception ex)
                {
                    ErrorException.OnError(ex);
                }
            }
            if (!Directory.Exists(m_dataDirPath))
            {
                Directory.CreateDirectory(m_dataDirPath);
            }
            //m_groupFilePath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "config\\Groups.xml");
            ReadGroups();
        }

        private static string m_groupFilePath;

        private static Dictionary<String, String> m_dataDic = null;

        private static string m_dataDirPath;

        public static string DataDirPath
        {
            get { return XmlHandle.m_dataDirPath; }
            set
            {
                XmlHandle.m_dataDirPath = value;
                if (!Directory.Exists(m_dataDirPath))
                {
                    Directory.CreateDirectory(m_dataDirPath);
                }
                if (!Directory.Exists(m_userDir))
                {
                    Directory.CreateDirectory(m_userDir);
                }
                try
                {
                    File.WriteAllText(Path.Combine(m_userDir, "DataPath.txt"), m_dataDirPath, Encoding.UTF8);
                }
                catch (Exception ex)
                {
                    ErrorException.OnError(ex);
                }
            }
        }

        private static List<JGroup> m_groups = new List<JGroup>();

        public static List<JGroup> Groups
        {
            get { return XmlHandle.m_groups; }
            set { XmlHandle.m_groups = value; }
        }

        private static string m_userDir;

        public static string UserDir
        {
            get { return XmlHandle.m_userDir; }
            set { XmlHandle.m_userDir = value; }
        }

        public static void DeleteJira(string jiraID)
        {
            HttpGetService.Get(DataCenter.ServerAddr + "giraservice?func=deletegira&gid=" + jiraID);
        }

        public static void InitServer()
        {
            m_dataDic = new Dictionary<string, string>();
            Binary binary = HttpGetService.GetAsBinary(DataCenter.ServerAddr + "giraservice?func=getall");
            if (binary != null)
            {
                int dataLength = binary.ReadInt();
                for (int i = 0; i < dataLength; i++)
                {
                    String xmlName = binary.ReadString();
                    int nextBisLen = binary.ReadInt();
                    byte[] byts = new byte[nextBisLen];
                    binary.ReadBytes(byts);
                    String xmlStr = System.Text.Encoding.Default.GetString(byts);
                    m_dataDic[xmlName] = xmlStr;
                }
            }
        }

        public static List<JExpression> GetExpressions()
        {
            List<JExpression> expressions = new List<JExpression>();
            try
            {
                string xmlPath = Path.Combine(m_userDir, "Expression.xml");
                if (File.Exists(xmlPath))
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(xmlPath);
                    XmlNode rootNode = xmlDoc.DocumentElement;
                    foreach (XmlNode node in rootNode.ChildNodes)
                    {
                        if (node.Name.ToUpper() == "EXPRESSION")
                        {
                            JExpression jExp = new JExpression();
                            jExp.ReadXml(node);
                            expressions.Add(jExp);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorException.OnError(ex);
            }
            return expressions;
        }

        public static Jira GetJira(string jiraID)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(m_dataDic[jiraID]);
                XmlNode rootNode = xmlDoc.DocumentElement;
                Jira jira = new Jira();
                jira.ReadXml(rootNode);
                return jira;
            }
            catch (Exception ex)
            {
                ErrorException.OnError(ex);
            }
            return null;
        }

        public static List<Jira> GetJiras()
        {
            InitServer();
            int infoLength = m_dataDic.Count;
            List<Jira> jiras = new List<Jira>(infoLength);
            foreach (String jid in m_dataDic.Keys)
            {
                Jira jira = GetJira(jid);
                if (jira != null)
                {
                    jiras.Add(jira);
                }
            }
            return jiras;
        }

        private static DateTime GetDataTime(string str)
        {
            int year = Convert.ToInt32(str.Substring(0, 4));
            int month = Convert.ToInt32(str.Substring(4, 2));
            int day = Convert.ToInt32(str.Substring(6, 2));
            return new DateTime(year, month, day);
        }

        public static void InsertJiraToDataRow(DataRow dr, Jira jira)
        {
            try
            {
                dr[0] = jira.JiraID;
                dr[1] = jira.Title;
                dr[2] = jira.HttpPath;
                dr[3] = jira.Creater;
                dr[4] = jira.Developer;
                dr[5] = jira.DeveloperReceive ? "是" : "否";
                dr[6] = jira.DeveloperPass ? "是" : "否";
                dr[7] = jira.TestPass ? "是" : "否";
                dr[8] = jira.ProductPass ? "是" : "否";
                dr[9] = jira.WaitPublish ? "是" : "否";
                dr[10] = jira.Published ? "是" : "否";
                dr[11] = jira.CloseTask ? "是" : "否";
                dr[12] = jira.Hurry;
                dr[13] = jira.Description;
                dr[14] = jira.Branches;
                dr[15] = jira.StartDate;
                dr[16] = jira.EndDate;
                dr[17] = jira;
                dr[18] = jira.RelativeGroup;
                for (int x = 0; x < XmlHandle.Groups.Count; x++)
                {
                    if (XmlHandle.Groups[x].Id == jira.GroupID)
                    {
                        dr[19] = XmlHandle.Groups[x].Name;
                        dr[20] = XmlHandle.Groups[x].Manager;
                        List<JCategory> categories = XmlHandle.Groups[x].Categories;
                        for (int j = 0; j < categories.Count; j++)
                        {
                            if (categories[j].Id == jira.CategoryID)
                            {
                                dr[21] = categories[j].Name;
                                break;
                            }
                        }
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorException.OnError(ex);
            }
        }

        private static void ReadGroups()
        {
            try
            {
                Binary bi = HttpGetService.GetAsBinary(DataCenter.ServerAddr + "giraservice?func=readgroup");
                if(bi != null)
                {
                    String xmlString = bi.ReadString();
                    if(xmlString != null)
                    {
                        XmlDocument xmlDoc = new XmlDocument();
                        xmlDoc.LoadXml(xmlString);
                        XmlNode node = xmlDoc.DocumentElement;
                        foreach (XmlNode subNode in node.ChildNodes)
                        {
                            if (subNode.Name.ToUpper() == "GROUP")
                            {
                                JGroup jGroup = new JGroup();
                                jGroup.ReadXml(subNode);
                                m_groups.Add(jGroup);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorException.OnError(ex);
            }
        }

        public static void SaveExpressions(List<JExpression> expressions)
        {
            try
            {
                string xmlPath = Path.Combine(m_userDir, "Expression.xml");
                StringBuilder sb = new StringBuilder();
                sb.Append(@"<?xml version='1.0' encoding='utf-8' ?>");
                sb.Append("\r\n");
                sb.Append("<Expressions>\r\n");
                for (int i = 0; i < expressions.Count; i++)
                {
                    sb.Append(expressions[i].GetXml());
                }
                sb.Append("</Expressions>");
                File.WriteAllText(xmlPath, sb.ToString(), Encoding.UTF8);
            }
            catch (Exception ex)
            {
                ErrorException.OnError(ex);
            }
        }

        public static void SaveJira(Jira jira)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"<?xml version='1.0' encoding='gb2312' ?>");
            sb.Append("\r\n");
            sb.Append(jira.GetXml());
            Binary bi = new Binary();
            bi.WriteBytes(System.Text.Encoding.Default.GetBytes(sb.ToString()));
            byte[] sendData = bi.GetBytes();
            HttpPostService.Post(DataCenter.ServerAddr + "giraservice?func=savegira&gid=" + jira.JiraID, sendData, true);
            m_dataDic[jira.JiraID] = sb.ToString();
        }
    }
}
