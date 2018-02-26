using System;
using System.Collections;
using System.ComponentModel;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Text;

namespace OwLib
{
    public class JCategory
    {
        private string m_id;

        public string Id
        {
            get { return m_id; }
            set { m_id = value; }
        }

        private string m_name;

        public string Name
        {
            get { return m_name; }
            set { m_name = value; }
        }

        public void ReadXml(XmlNode node)
        {
            foreach (XmlAttribute xmlAtr in node.Attributes)
            {
                string atrName = xmlAtr.Name.ToUpper();
                string atrValue = xmlAtr.Value;
                switch (atrName)
                {
                    case "ID":
                        m_id = atrValue;
                        break;
                    case "NAME":
                        m_name = atrValue;
                        break;
                }
            }
        }

        public override string ToString()
        {
            return m_name.ToString();
        }
    }
}
