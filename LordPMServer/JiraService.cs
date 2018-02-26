/*****************************************************************************************\
*                                                                                         *
* IndicatorLayoutService.cs -  Kline layout service functions, types, and definitions.    *
*                                                                                         *
*               Version 1.00 ��                                                           *
*                                                                                         *
*               Copyright (c) 2016-2016, Server. All rights reserved.                     *
*               Created by Todd.                                                          *
*                                                                                         *
******************************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace OwLibSV
{
    /// <summary>
    /// ָ�겼�ַ���
    /// </summary>
    public class JiraService : BaseService
    {
        #region Lord 2016/5/22
        /// <summary>
        /// ����ָ�����
        /// </summary>
        public JiraService()
        {
            ServiceID = SERVICEID_JIRA;
        }

        /// <summary>
        /// ��
        /// </summary>
        private object m_lock = new object();

        /// <summary>
        /// �׽��ּ���
        /// </summary>
        private Dictionary<int, SocketArray> m_sockets = new Dictionary<int, SocketArray>();

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

        /// <summary>
        /// ��Ӳ���
        /// </summary>
        /// <param name="message">��Ϣ</param>
        /// <returns>״̬</returns>
        public int AddJiras(CMessage message)
        {
            List<Jira> layouts = new List<Jira>();
            GetLayouts(layouts, message.m_body, message.m_bodyLength);
            int layoutsSize = layouts.Count;
            List<IndicatorLayout> addLayouts = new List<IndicatorLayout>();
            for (int i = 0; i < layoutsSize; i++)
            {
                IndicatorLayout receive = layouts[i];
                receive.m_layoutID = Guid.NewGuid().ToString();
                String sql = String.Format("INSERT INTO INDICATORLAYOUT(LAYOUTID, USERID, NAME, TEXT, TYPE, ORDERNUM, CREATETIME, MODIFYTIME) values ('{0}', {1}, '{2}', '{3}', {4}, {5}, '1970-1-1', '1970-1-1')",
                    CStrA.GetDBString(receive.m_layoutID), receive.m_userID, CStrA.GetDBString(receive.m_name), CStrA.GetDBString(receive.m_text), receive.m_type, receive.m_orderNum);
                SQLiteConnection conn = new SQLiteConnection(m_connectStr);
                conn.Open();
                SQLiteCommand cmd = conn.CreateCommand();
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                conn.Close();
                addLayouts.Add(receive);
            }
            int ret = Send(message, addLayouts, true);
            layouts.Clear();
            addLayouts.Clear();
            return ret;
        }

        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="message">��Ϣ</param>
        /// <returns>״̬</returns>
        public int DeleteLayouts(CMessage message)
        {
            List<IndicatorLayout> layouts = new List<IndicatorLayout>();
            GetLayouts(layouts, message.m_body, message.m_bodyLength);
            int layoutsSize = layouts.Count;
            List<IndicatorLayout> deleteLayouts = new List<IndicatorLayout>();
            for (int i = 0; i < layoutsSize; i++)
            {
                IndicatorLayout receive = layouts[i];
                String sql = String.Format("DELETE FROM INDICATORLAYOUT WHERE USERID = {0} AND LAYOUTID = '{1}'", receive.m_userID, CStrA.GetDBString(receive.m_layoutID));
                SQLiteConnection conn = new SQLiteConnection(m_connectStr);
                SQLiteCommand cmd = conn.CreateCommand();
                cmd.CommandText = sql;
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
                deleteLayouts.Add(receive);
            }
            int ret = Send(message, deleteLayouts, true);
            layouts.Clear();
            deleteLayouts.Clear();
            return ret;
        }

        /// <summary>
        /// ��ȡ������Ϣ
        /// </summary>
        /// <param name="layouts">ָ����Ϣ</param>
        /// <param name="body">����</param>
        /// <param name="bodyLength">���峤��</param>
        /// <returns>״̬</returns>
        public int GetJiras(List<Jira> jiras, byte[] body, int bodyLength)
        {
            Binary br = new Binary();
            br.Write(body, bodyLength);
            int indicatorsSize = br.ReadInt();
            for (int i = 0; i < indicatorsSize; i++)
            {
                Jira jira = new Jira();
                layout.m_layoutID = br.ReadString();
                layout.m_userID = br.ReadInt();
                layout.m_name = br.ReadString();
                layout.m_text = br.ReadString();
                layout.m_type = br.ReadInt();
                layout.m_orderNum = br.ReadInt();
                jiras.Add(layout);
            }
            br.Close();
            return 1;
        }

        /// <summary>
        /// ��ȡ������Ϣ
        /// </summary>
        /// <param name="message">��Ϣ</param>
        /// <returns>״̬</returns>
        public int GetLayouts(CMessage message)
        {
            IndicatorLayout receive = new IndicatorLayout();
            List<IndicatorLayout> layouts = new List<IndicatorLayout>();
            GetLayouts(layouts, message.m_body, message.m_bodyLength);
            receive = layouts[0];
            layouts.Clear();
            GetLayouts(layouts, receive.m_userID, receive.m_layoutID);
            int ret = Send(message, layouts, false);
            layouts.Clear();
            return ret;
        }

        /// <summary>
        /// ��ȡ������Ϣ
        /// </summary>
        /// <param name="layouts">������Ϣ</param>
        /// <param name="userID">�û�ID</param>
        /// <param name="layoutID">����ID</param>
        public int GetLayouts(List<IndicatorLayout> layouts, int userID, String layoutID)
        {
            String sql = "";
            if (layoutID != null && layoutID.Length > 0)
            {
                sql = String.Format("SELECT * FROM INDICATORLAYOUT WHERE USERID = {0} AND LAYOUTID = '{1}'", userID, CStrA.GetDBString(layoutID));
            }
            else
            {
                if (userID > 0)
                {
                    sql = String.Format("SELECT * FROM INDICATORLAYOUT WHERE USERID = {0} ORDER BY ORDERNUM", userID);
                }
                else
                {
                    sql = "SELECT * FROM INDICATORLAYOUT ORDER BY ORDERNUM";
                }
            }
            SQLiteConnection conn = new SQLiteConnection(m_connectStr);
            SQLiteCommand cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            conn.Open();
            SQLiteDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                IndicatorLayout layout = new IndicatorLayout();
                layout.m_layoutID = reader.GetString(0);
                layout.m_userID = reader.GetInt32(1);
                layout.m_name = reader.GetString(2);
                layout.m_text = reader.GetString(3);
                layout.m_type = reader.GetInt32(4);
                layout.m_orderNum = reader.GetInt32(5);
                layouts.Add(layout);
            }
            reader.Close();
            conn.Close();
            return 1;
        }

        /// <summary>
        /// �ͻ��˹رշ���
        /// </summary>
        /// <param name="socketID">����ID</param>
        /// <param name="localSID">��������ID</param>
        public override void OnClientClose(int socketID, int localSID)
        {
            base.OnClientClose(socketID, localSID);
            lock (m_sockets)
            {
                foreach(SocketArray socketArray in m_sockets.Values)
                {
                    List<int> sockets = new List<int>();
                    socketArray.GetSocketList(sockets);
                    int socketsSize = sockets.Count;
                    for (int i = 0; i < socketsSize; i++)
                    {
                        if (sockets[i] == socketID)
                        {
                            socketArray.RemoveSocket(socketID);
                        }
                    }
                    sockets.Clear();
                }
            }
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="message">��Ϣ</param>
        public override void OnReceive(CMessage message)
        {
            base.OnReceive(message);
            switch (message.m_functionID)
            {
                case FUNCTIONID_JIRA_ADDJIRAS:
                    AddJiras(message);
                    break;
                case FUNCTIONID_JIRA_DELETEJIRAS:
                    DeleteLayouts(message);
                    break;
                case FUNCTIONID_JIRA_GETJIRAS:
                    GetLayouts(message);
                    break;
                case FUNCTIONID_JIRA_UPDATEJIRAS:
                    UpdateLayout(message);
                    break;
                case FUNCTIONID_JIRA_UPDATEORDERNUM:
                    UpdateOrderNum(message);
                    break;
            }
        }

        /// <summary>
        /// ����ָ��
        /// </summary>
        /// <param name="message">��Ϣ</param>
        /// <param name="layouts">�����б�</param>
        /// <param name="broadCast">�Ƿ�㲥</param>
        /// <returns>״̬</returns>
        public int Send(CMessage message, List<IndicatorLayout> layouts, bool broadCast)
        {
            Binary bw = new Binary();
            int size = layouts.Count;
            bw.WriteInt(size);
            for (int i = 0; i < size; i++)
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
            message.m_bodyLength = bytes.Length;
            message.m_body = bytes;
            int userID = 0;
            if (size > 0)
            {
                userID = layouts[0].m_userID;
            }
            else
            {
                broadCast = false;
            }
            lock (m_sockets)
            {
                SocketArray socketArray = null;
                if (m_sockets.ContainsKey(userID))
                {
                    socketArray = m_sockets[userID];
                }
                else
                {
                    socketArray = new SocketArray();
                    m_sockets[userID] = socketArray;
                }
                if (message.m_sessionID > 0)
                {
                    socketArray.AddSocket(message.m_socketID);
                }
                if (broadCast)
                {
                    List<int> sockets = new List<int>();
                    socketArray.GetSocketList(sockets);
                    int socketsSize = sockets.Count;
                    for (int i = 0; i < socketsSize; i++)
                    {
                        message.m_socketID = sockets[i];
                        int ret = Send(message);
                        if (ret == -1)
                        {
                            socketArray.RemoveSocket(sockets[i]);
                        }
                    }
                }
                else
                {
                    Send(message);
                }
            }
            bw.Close();
            return 1;
        }

        /// <summary>
        /// ���²���
        /// </summary>
        /// <param name="message">��Ϣ</param>
        /// <returns>״̬</returns>
        public int UpdateLayout(CMessage message)
        {
            List<IndicatorLayout> layouts = new List<IndicatorLayout>();
            GetLayouts(layouts, message.m_body, message.m_bodyLength);
            int layoutsSize = layouts.Count;
            List<IndicatorLayout> updateLayouts = new List<IndicatorLayout>();
            for (int i = 0; i < layoutsSize; i++)
            {
                IndicatorLayout receive = layouts[i];
                UpdateLayouts(receive);
                updateLayouts.Add(receive);
            }
            int ret = Send(message, updateLayouts, true);
            updateLayouts.Clear();
            layouts.Clear();
            return ret;
        }

        /// <summary>
        /// ���²���
        /// </summary>
        /// <param name="layout">����</param>
        public void UpdateLayouts(IndicatorLayout layout)
        {
            String sql = String.Format("UPDATE INDICATORLAYOUT SET NAME = '{0}', TEXT = '{1}', TYPE = {2}, ORDERNUM = {3} WHERE USERID = {4} AND LAYOUTID = '{5}'",
            CStrA.GetDBString(layout.m_name), CStrA.GetDBString(layout.m_text), layout.m_type, layout.m_orderNum, layout.m_userID, CStrA.GetDBString(layout.m_layoutID));
            SQLiteConnection conn = new SQLiteConnection(m_connectStr);
            SQLiteCommand cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        /// <summary>
        /// ���������ֶ�
        /// </summary>
        /// <param name="message">��Ϣ</param>
        /// <returns>״̬</returns>
        public int UpdateOrderNum(CMessage message)
        {
            List<IndicatorLayout> layouts = new List<IndicatorLayout>();
            GetLayouts(layouts, message.m_body, message.m_bodyLength);
            int layoutsSize = layouts.Count;
            List<IndicatorLayout> updateLayouts = new List<IndicatorLayout>();
            for (int i = 0; i < layoutsSize; i++)
            {
                IndicatorLayout receive = layouts[i];
                String[] ids = receive.m_layoutID.Split(new String[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                int idsSize = ids.Length;
                for (int j = 0; j < idsSize; j++)
                {
                    String sql = String.Format("UPDATE INDICATORLAYOUT SET ORDERNUM = {0} WHERE USERID = {1} AND LAYOUTID = '{2}'",
                    j, receive.m_userID, CStrA.GetDBString(ids[j]));
                    SQLiteConnection conn = new SQLiteConnection(m_connectStr);
                    SQLiteCommand cmd = conn.CreateCommand();
                    cmd.CommandText = sql;
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
                updateLayouts.Add(receive);
            }
            int ret = Send(message, updateLayouts, true);
            updateLayouts.Clear();
            layouts.Clear();
            return ret;
        }
        #endregion
    }
}
