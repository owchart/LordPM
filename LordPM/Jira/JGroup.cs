using System;
using System.Collections;
using System.ComponentModel;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Text;

namespace OwLib
{
    public class JGroup
    {
        private List<JCategory> m_categories = new List<JCategory>();

        public List<JCategory> Categories
        {
            get { return m_categories; }
            set { m_categories = value; }
        }

        private string m_id;

        public string Id
        {
            get { return m_id; }
            set { m_id = value; }
        }

        private string m_manager;

        public string Manager
        {
            get { return m_manager; }
            set { m_manager = value; }
        }

        private string[] m_members;

        public string[] Members
        {
            get { return m_members; }
            set { m_members = value; }
        }

        private string m_name;

        public string Name
        {
            get { return m_name; }
            set { m_name = value; }
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
                    case "ID":
                        m_id = nodeText;
                        break;
                    case "NAME":
                        m_name = nodeText;
                        break;
                    case "MANAGER":
                        m_manager = nodeText;
                        break;
                    case "MEMBERS":
                        m_members = nodeText.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                        break;
                    case "CATEGORIES":
                        foreach (XmlNode sunNode in subNode.ChildNodes)
                        {
                            if (sunNode.Name.ToUpper() == "CATEGORY")
                            {
                                JCategory category = new JCategory();
                                category.ReadXml(sunNode);
                                this.m_categories.Add(category);
                            }
                        }
                        break;
                }
            }
        }

        public override string ToString()
        {
            return m_name;
        }
    }
}
