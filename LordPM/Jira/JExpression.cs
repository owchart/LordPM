using System;
using System.Collections;
using System.ComponentModel;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Text;

namespace OwLib
{
    public class JExpression
    {
        private string m_expression;

        public string Expression
        {
            get { return m_expression; }
            set { m_expression = value; }
        }

        private string m_name;

        public string Name
        {
            get { return m_name; }
            set { m_name = value; }
        }

        public string GetXml()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<Expression>\r\n");
            sb.Append("<Name>" + (m_name != null ? m_name : "") + "</Name>");
            sb.Append("<Expression>" + (m_expression != null ? m_expression : "") + "</Expression>");
            sb.Append("</Expression>");
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
                    case "NAME":
                        m_name = nodeText;
                        break;
                    case "EXPRESSION":
                        m_expression = nodeText;
                        break;
                }
            }
        }
    }
}
