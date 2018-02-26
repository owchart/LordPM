using System;
using System.Collections.Generic;
using System.Text;

namespace OwLib
{
    public class Expression
    {
        /// <summary>
        /// 构造函数初始化
        /// </summary>
        /// <param name="name">Lable名</param>
        /// <param name="str">拼接名</param>
        /// <param name="content">文本内容</param>
        public Expression(String name, String str, String content)
        {
            this.m_name = name;
            this.m_str = str;
            this.m_content = content;
        }
         
        private int m_id;
        /// <summary>
        /// 填入表格号
        /// </summary>
        public int Id
        {
            get { return m_id; }
            set { m_id = value; }
        }

        private String m_name;
        /// <summary>
        /// Lable字段
        /// </summary>
        public String Name
        {
            get { return m_name; }
            set { m_name = value; }
        }

        private String m_content;
        /// <summary>
        /// 连接字段
        /// </summary>
        public String Content
        {
            get { return m_content; }
            set { m_content = value; }
        }

        private String m_str;
        /// <summary>
        /// 文本字段
        /// </summary>
        public String Str
        {
            get { return m_str; }
            set { m_str = value; }
        }

        /// <summary>
        /// 拼接表达式
        /// </summary>
        /// <returns></returns>
        public String getExpression()
        {
            return m_name +" "+ m_str +" "+ m_content+" and ";
        }
    }
}
