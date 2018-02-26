using System;
using System.Collections.Generic;
using System.Text;

namespace OwLib
{
    public class Expression
    {
        /// <summary>
        /// ���캯����ʼ��
        /// </summary>
        /// <param name="name">Lable��</param>
        /// <param name="str">ƴ����</param>
        /// <param name="content">�ı�����</param>
        public Expression(String name, String str, String content)
        {
            this.m_name = name;
            this.m_str = str;
            this.m_content = content;
        }
         
        private int m_id;
        /// <summary>
        /// �������
        /// </summary>
        public int Id
        {
            get { return m_id; }
            set { m_id = value; }
        }

        private String m_name;
        /// <summary>
        /// Lable�ֶ�
        /// </summary>
        public String Name
        {
            get { return m_name; }
            set { m_name = value; }
        }

        private String m_content;
        /// <summary>
        /// �����ֶ�
        /// </summary>
        public String Content
        {
            get { return m_content; }
            set { m_content = value; }
        }

        private String m_str;
        /// <summary>
        /// �ı��ֶ�
        /// </summary>
        public String Str
        {
            get { return m_str; }
            set { m_str = value; }
        }

        /// <summary>
        /// ƴ�ӱ��ʽ
        /// </summary>
        /// <returns></returns>
        public String getExpression()
        {
            return m_name +" "+ m_str +" "+ m_content+" and ";
        }
    }
}
