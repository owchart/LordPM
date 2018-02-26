/*****************************************************************************\
*                                                                             *
* DataCenter.cs -  Data center functions, types, and definitions.             *
*                                                                             *
*               Version 1.00  ����                                          *
*                                                                             *
*               Copyright (c) 2016-2016, OwPlan. All rights reserved.      *
*               Created by Lord 2016/3/10.                                    *
*                                                                             *
*******************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using OwLib;
using System.IO;
using System.Net;
using System.Threading;
using System.Xml;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Forms;
using OwLibSV;

namespace node.gs
{
    /// <summary>
    /// ������������
    /// </summary>
    public class DataCenter
    {
        #region Lord 2016/3/10
        private static ChatService m_chatService = new ChatService();

        /// <summary>
        /// ��ȡ�������������
        /// </summary>
        public static ChatService ChatService
        {
            get { return m_chatService; }
            set { m_chatService = value; }
        }

        private static bool m_isAppAlive = true;

        /// <summary>
        /// ��ȡ�����ó����Ƿ���
        /// </summary>
        public static bool IsAppAlive
        {
            get { return DataCenter.m_isAppAlive; }
            set { DataCenter.m_isAppAlive = value; }
        }

        private static JiraService m_jiraService = new JiraService();

        /// <summary>
        /// ��ȡJira����
        /// </summary>
        public static JiraService JiraService
        {
            get { return m_jiraService; }
        }

        /// <summary>
        /// ��ȡ����·��
        /// </summary>
        /// <returns>����·��</returns>
        public static String GetAppPath()
        {
            return Application.StartupPath;
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="fileName">�ļ���</param>
        public static void StartService()
        {
            BaseService.AddService(m_chatService);
            BaseService.AddService(m_jiraService);
        }
        #endregion
    }
}
