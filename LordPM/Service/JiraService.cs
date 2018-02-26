/************************************************************************************\
*                                                                                    *
* IndicatorLayoutService.cs -  Kline layout service functions, types, and definitions*
*                                                                                    *
*               Version 1.00 ★                                                      *
*                                                                                    *
*               Copyright (c) 2016-2016, Client. All rights reserved.                *
*               Created by Lord.                                                     *
*                                                                                    *
*************************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace OwLibCT
{
    /// <summary>
    /// 指标布局服务
    /// </summary>
    public class JiraService : BaseService
    {
        #region Lord 2016/1/12
        /// <summary>
        /// 创建指标服务
        /// </summary>
        public JiraService()
        {
            ServiceID = SERVICEID_JIRA;
        }

        /// <summary>
        /// K线布局服务ID
        /// </summary>
        public const int SERVICEID_JIRA = 6;

        /// <summary>
        /// 添加布局方法ID
        /// </summary>
        public const int FUNCTIONID_JIRA_ADDJIRAS = 0;

        /// <summary>
        /// 删除布局方法ID
        /// </summary>
        public const int FUNCTIONID_JIRA_DELETEJIRAS = 1;

        /// <summary>
        /// 获取所有布局方法ID
        /// </summary>
        public const int FUNCTIONID_JIRA_GETJIRAS = 2;

        /// <summary>
        /// 更新布局方法ID
        /// </summary>
        public const int FUNCTIONID_JIRA_UPDATEJIRAS = 3;

        /// <summary>
        /// 更新排序字段方法ID
        /// </summary>
        public const int FUNCTIONID_JIRA_UPDATEORDERNUM = 4;

        private int m_socketID = 0;

        /// <summary>
        /// 获取或设置套接字ID
        /// </summary>
        public int SocketID
        {
            get { return m_socketID; }
            set { m_socketID = value; }
        }

        /// <summary>
        /// 添加布局
        /// </summary>
        /// <param name="requestID">请求ID</param>
        /// <param name="layout">布局</param>
        /// <returns>状态</returns>
        public int AddLayout(int requestID, IndicatorLayout layout)
        {
            List<IndicatorLayout> layouts = new List<IndicatorLayout>();
            layouts.Add(layout);
            int ret = Send(FUNCTIONID_JIRA_ADDJIRAS, requestID, layouts);
            layouts.Clear();
            return ret > 0 ? 1 : 0;
        }

        /// <summary>
        /// 删除布局
        /// </summary>
        /// <param name="requestID">请求ID</param>
        /// <param name="layout">布局</param>
        /// <returns>状态</returns>
        public int DeleteLayout(int requestID, IndicatorLayout layout)
        {
            List<IndicatorLayout> layouts = new List<IndicatorLayout>();
            layouts.Add(layout);
            int ret = Send(FUNCTIONID_JIRA_DELETEJIRAS, requestID, layouts);
            layouts.Clear();
            return ret > 0 ? 1 : 0;
        }

        /// <summary>
        /// 获取所有布局
        /// </summary>
        /// <param name="requestID">请求ID</param>
        /// <param name="layout">布局</param>
        /// <returns>状态</returns>
        public int GetLayouts(int requestID, IndicatorLayout layout)
        {
            List<IndicatorLayout> layouts = new List<IndicatorLayout>();
            layouts.Add(layout);
            int ret = Send(FUNCTIONID_JIRA_GETJIRAS, requestID, layouts);
            layouts.Clear();
            return ret > 0 ? 1 : 0;
        }

        /// <summary>
        /// 通过流获取布局信息
        /// </summary>
        /// <param name="layouts">指标信息</param>
        /// <param name="body">包体</param>
        /// <param name="bodyLength">包体长度</param>
        public static int GetLayouts(List<IndicatorLayout> layouts, byte[] body, int bodyLength)
        {
            Binary br = new Binary();
            br.Write(body, bodyLength);
            int size = br.ReadInt();
            if (size > 0)
            {
                for (int i = 0; i < size; i++)
                {
                    IndicatorLayout layout = new IndicatorLayout();
                    layout.m_layoutID = br.ReadString();
                    layout.m_userID = br.ReadInt();
                    layout.m_name = br.ReadString();
                    layout.m_text = br.ReadString();
                    layout.m_type = br.ReadInt();
                    layout.m_orderNum = br.ReadInt();
                    layouts.Add(layout);
                }
            }
            br.Close();
            return 1;
        }

        /// <summary>
        /// 接收数据
        /// </summary>
        /// <param name="message">消息</param>
        public override void OnReceive(CMessage message)
        {
            base.OnReceive(message);
            SendToListener(message);
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="functionID">方法ID</param>
        /// <param name="requestID">请求ID</param>
        /// <param name="layouts">布局集合</param>
        /// <returns>状态</returns>
        public int Send(int functionID, int requestID, List<IndicatorLayout> layouts)
        {
            Binary bw = new Binary();
            int layoutsSize = layouts.Count;
            bw.WriteInt(layoutsSize);
            for (int i = 0; i < layoutsSize; i++)
            {
                IndicatorLayout layout = layouts[i];
                bw.WriteString(layout.m_layoutID);
                bw.WriteInt(layout.m_userID);
                bw.WriteString(layout.m_name);
                bw.WriteString(layout.m_text);
                bw.WriteInt(layout.m_type);
                bw.WriteInt(layout.m_orderNum);
            }
            byte[] bytes = bw.GetBytes();
            int ret = Send(new CMessage(GroupID, ServiceID, functionID, SessionID, requestID, m_socketID, 0, CompressType, bytes.Length, bytes));
            bw.Close();
            return ret;
        }

        /// <summary>
        /// 更新模板
        /// </summary>
        /// <param name="requestID">请求ID</param>
        /// <param name="indicator">指标</param>
        /// <returns>状态</returns>
        public int UpdateLayout(int requestID, IndicatorLayout layout)
        {
            List<IndicatorLayout> layouts = new List<IndicatorLayout>();
            layouts.Add(layout);
            int ret = Send(FUNCTIONID_JIRA_UPDATEJIRAS, requestID, layouts);
            layouts.Clear();
            return ret > 0 ? 1 : 0;
        }

        /// <summary>
        /// 更新模版的排序
        /// </summary>
        /// <param name="requestID">请求ID</param>
        /// <param name="userID">用户ID</param>
        /// <param name="ids">ID</param>
        /// <returns>状态</returns>
        public int UpdateOrderNum(int requestID, int userID, List<String> ids)
        {
            String str = "";
            int idsSize = ids.Count;
            for (int i = 0; i < idsSize; i++)
            {
                str += ids[i];
                if (i != idsSize - 1)
                {
                    str += ",";
                }
            }
            IndicatorLayout layout = new IndicatorLayout();
            layout.m_layoutID = str;
            layout.m_userID = userID;
            List<IndicatorLayout> layouts = new List<IndicatorLayout>();
            layouts.Add(layout);
            int ret = Send(FUNCTIONID_JIRA_UPDATEORDERNUM, requestID, layouts);
            layouts.Clear();
            return ret > 0 ? 1 : 0;
        }
        #endregion
    }
}
