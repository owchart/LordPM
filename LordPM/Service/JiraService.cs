/************************************************************************************\
*                                                                                    *
* IndicatorLayoutService.cs -  Kline layout service functions, types, and definitions*
*                                                                                    *
*               Version 1.00 ��                                                      *
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
    /// ָ�겼�ַ���
    /// </summary>
    public class JiraService : BaseService
    {
        #region Lord 2016/1/12
        /// <summary>
        /// ����ָ�����
        /// </summary>
        public JiraService()
        {
            ServiceID = SERVICEID_JIRA;
        }

        /// <summary>
        /// K�߲��ַ���ID
        /// </summary>
        public const int SERVICEID_JIRA = 6;

        /// <summary>
        /// ��Ӳ��ַ���ID
        /// </summary>
        public const int FUNCTIONID_JIRA_ADDJIRAS = 0;

        /// <summary>
        /// ɾ�����ַ���ID
        /// </summary>
        public const int FUNCTIONID_JIRA_DELETEJIRAS = 1;

        /// <summary>
        /// ��ȡ���в��ַ���ID
        /// </summary>
        public const int FUNCTIONID_JIRA_GETJIRAS = 2;

        /// <summary>
        /// ���²��ַ���ID
        /// </summary>
        public const int FUNCTIONID_JIRA_UPDATEJIRAS = 3;

        /// <summary>
        /// ���������ֶη���ID
        /// </summary>
        public const int FUNCTIONID_JIRA_UPDATEORDERNUM = 4;

        private int m_socketID = 0;

        /// <summary>
        /// ��ȡ�������׽���ID
        /// </summary>
        public int SocketID
        {
            get { return m_socketID; }
            set { m_socketID = value; }
        }

        /// <summary>
        /// ��Ӳ���
        /// </summary>
        /// <param name="requestID">����ID</param>
        /// <param name="layout">����</param>
        /// <returns>״̬</returns>
        public int AddLayout(int requestID, IndicatorLayout layout)
        {
            List<IndicatorLayout> layouts = new List<IndicatorLayout>();
            layouts.Add(layout);
            int ret = Send(FUNCTIONID_JIRA_ADDJIRAS, requestID, layouts);
            layouts.Clear();
            return ret > 0 ? 1 : 0;
        }

        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="requestID">����ID</param>
        /// <param name="layout">����</param>
        /// <returns>״̬</returns>
        public int DeleteLayout(int requestID, IndicatorLayout layout)
        {
            List<IndicatorLayout> layouts = new List<IndicatorLayout>();
            layouts.Add(layout);
            int ret = Send(FUNCTIONID_JIRA_DELETEJIRAS, requestID, layouts);
            layouts.Clear();
            return ret > 0 ? 1 : 0;
        }

        /// <summary>
        /// ��ȡ���в���
        /// </summary>
        /// <param name="requestID">����ID</param>
        /// <param name="layout">����</param>
        /// <returns>״̬</returns>
        public int GetLayouts(int requestID, IndicatorLayout layout)
        {
            List<IndicatorLayout> layouts = new List<IndicatorLayout>();
            layouts.Add(layout);
            int ret = Send(FUNCTIONID_JIRA_GETJIRAS, requestID, layouts);
            layouts.Clear();
            return ret > 0 ? 1 : 0;
        }

        /// <summary>
        /// ͨ������ȡ������Ϣ
        /// </summary>
        /// <param name="layouts">ָ����Ϣ</param>
        /// <param name="body">����</param>
        /// <param name="bodyLength">���峤��</param>
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
        /// ��������
        /// </summary>
        /// <param name="message">��Ϣ</param>
        public override void OnReceive(CMessage message)
        {
            base.OnReceive(message);
            SendToListener(message);
        }

        /// <summary>
        /// ������Ϣ
        /// </summary>
        /// <param name="functionID">����ID</param>
        /// <param name="requestID">����ID</param>
        /// <param name="layouts">���ּ���</param>
        /// <returns>״̬</returns>
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
        /// ����ģ��
        /// </summary>
        /// <param name="requestID">����ID</param>
        /// <param name="indicator">ָ��</param>
        /// <returns>״̬</returns>
        public int UpdateLayout(int requestID, IndicatorLayout layout)
        {
            List<IndicatorLayout> layouts = new List<IndicatorLayout>();
            layouts.Add(layout);
            int ret = Send(FUNCTIONID_JIRA_UPDATEJIRAS, requestID, layouts);
            layouts.Clear();
            return ret > 0 ? 1 : 0;
        }

        /// <summary>
        /// ����ģ�������
        /// </summary>
        /// <param name="requestID">����ID</param>
        /// <param name="userID">�û�ID</param>
        /// <param name="ids">ID</param>
        /// <returns>״̬</returns>
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
