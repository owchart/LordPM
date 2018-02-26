/*****************************************************************************\
*                                                                             *
* DataCenter.cs -  Data center functions, types, and definitions.             *
*                                                                             *
*               Version 1.00  ★★★                                          *
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
    /// 处理行情数据
    /// </summary>
    public class DataCenter
    {
        #region Lord 2016/3/10
        private static ChatService m_chatService = new ChatService();

        /// <summary>
        /// 获取或设置聊天服务
        /// </summary>
        public static ChatService ChatService
        {
            get { return m_chatService; }
            set { m_chatService = value; }
        }

        private static bool m_isAppAlive = true;

        /// <summary>
        /// 获取或设置程序是否存活
        /// </summary>
        public static bool IsAppAlive
        {
            get { return DataCenter.m_isAppAlive; }
            set { DataCenter.m_isAppAlive = value; }
        }

        private static JiraService m_jiraService = new JiraService();

        /// <summary>
        /// 获取Jira服务
        /// </summary>
        public static JiraService JiraService
        {
            get { return m_jiraService; }
        }

        /// <summary>
        /// 获取程序路径
        /// </summary>
        /// <returns>程序路径</returns>
        public static String GetAppPath()
        {
            return Application.StartupPath;
        }

        /// <summary>
        /// 启动服务
        /// </summary>
        /// <param name="fileName">文件名</param>
        public static void StartService()
        {
            BaseService.AddService(m_chatService);
            BaseService.AddService(m_jiraService);
        }
        #endregion
    }
}
