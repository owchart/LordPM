/*****************************************************************************\
*                                                                             *
* ChatService.cs -  Login service functions, types, and definitions.          *
*                                                                             *
*               Version 1.00 ★                                               *
*                                                                             *
*               Copyright (c) 2016-2016, Server. All rights reserved.         *
*               Created by QiChunyou.                                         *
*                                                                             *
*******************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data.SQLite;

namespace OwLibSV
{
    /// <summary>
    /// 聊天数据
    /// </summary>
    public class ChatData
    {
        #region 齐春友 2016/6/9
        /// <summary>
        /// 内容
        /// </summary>
        public String m_text = "";

        /// <summary>
        /// 类型
        /// </summary>
        public int m_type;

        /// <summary>
        /// 用户ID
        /// </summary>
        public int m_userID;
        #endregion
    }

    /// <summary>
    /// 聊天服务
    /// </summary>
    public class ChatService:BaseService
    {
        #region 齐春友 2016/06/03
        /// <summary>
        /// 创建聊天服务
        /// </summary>
        public ChatService()
        {
            ServiceID = SERVICEID_CHAT;
        }

        /// <summary>
        /// 锁
        /// </summary>
        private object m_lock = new object();

        /// <summary>
        /// 会话列表
        /// </summary>
        private Dictionary<int, int> m_sessions = new Dictionary<int, int>();

        /// <summary>
        /// 弹幕服务ID
        /// </summary>
        private const int SERVICEID_CHAT = 7;

        /// <summary>
        /// 进入聊天功能ID
        /// </summary>
        public const int FUNCTIONID_CHAT_ENTER = 0;

        /// <summary>
        /// 退出聊天功能ID
        /// </summary>
        public const int FUNCTIONID_CHAT_EXIT = 1;

        /// <summary>
        /// 发送聊天功能ID
        /// </summary>
        public const int FUNCTIONID_CHAT_SEND = 2;

        /// <summary>
        /// 接收聊天功能ID
        /// </summary>
        public const int FUNCTIONID_CHAT_RECV = 3;
        
        /// <summary>
        /// 进入弹幕
        /// </summary>
        /// <param name="message">消息</param>
        /// <returns>状态</returns>
        public int Enter(CMessage message)
        {
            List<ChatData> datas = new List<ChatData>();
            GetChatDatas(datas, message.m_body, message.m_bodyLength);
            int ret = Send(message, datas);
            int sessionID = message.m_sessionID;
            if (ret != -1)
            {              
                lock (m_sessions)
                {
                    //移除原来的套接字
                    m_sessions[message.m_sessionID] = message.m_socketID;
                }
            }
            else
            {
                lock (m_sessions)
                {
                    if (m_sessions.ContainsKey(sessionID))
                    {
                        m_sessions.Remove(sessionID);
                    }
                }
            }
            datas.Clear();
            return ret;        
        }

        /// <summary>
        /// 断开弹幕
        /// </summary>
        /// <param name="message">消息</param>
        /// <returns>状态</returns>
        public int Exit(CMessage message)
        {
            List<ChatData> datas = new List<ChatData>();
            GetChatDatas(datas, message.m_body, message.m_bodyLength);
            int ret = Send(message, datas);
            lock (m_sessions)
            {
                int sessionID = message.m_sessionID;
                if (m_sessions.ContainsKey(sessionID))
                {
                    m_sessions.Remove(SessionID);
                }
            }
            datas.Clear();
            return ret;
        }


        /// <summary>
        /// 获取弹幕信息
        /// </summary>
        /// <param name="loginInfos">弹幕信息</param>
        /// <param name="body">包体</param>
        /// <param name="bodyLength">包体长度</param>
        public int GetChatDatas(List<ChatData> datas, byte[] body, int bodyLength)
        {
            Binary br = new Binary();
            br.Write(body, bodyLength);
            int chatSize = br.ReadInt();
            for (int i = 0; i < chatSize; i++)
            {
                ChatData chat = new ChatData();
                chat.m_userID = br.ReadInt();
                chat.m_type = (int)br.ReadChar();
                chat.m_text = br.ReadString();
                datas.Add(chat);
            }
            br.Close();
            return 1;
        }

        /// <summary>
        /// 客户端关闭方法
        /// </summary>
        /// <param name="socketID">连接ID</param>
        /// <param name="localSID">本地连接ID</param>
        public override void OnClientClose(int socketID, int localSID)
        {
            base.OnClientClose(socketID, localSID);
            lock (m_sessions)
            {
                List<int> removeSessions = new List<int>();
                foreach(int sessionID in m_sessions.Keys)
                {
                    int sid = m_sessions[sessionID];
                    if (sid == socketID)
                    {
                        removeSessions.Add(sessionID);
                    }
                }
                int removeSessionsSize = removeSessions.Count;
                for (int i = 0; i < removeSessionsSize; i++)
                {
                    m_sessions.Remove(removeSessions[i]);
                }
                removeSessions.Clear();
            }
        }

        /// <summary>
        /// 接收数据
        /// </summary>
        /// <param name="message">消息</param>
        public override void OnReceive(CMessage message)
        {
            base.OnReceive(message);
            switch (message.m_functionID)
            {
                case FUNCTIONID_CHAT_ENTER:
                    Enter(message);
                    break;
                case FUNCTIONID_CHAT_EXIT:
                    Exit(message);
                    break;
                case FUNCTIONID_CHAT_SEND:
                    SendToAllClients(message);
                    break;
                default:
                    break;           
            }
        }

        /// <summary>
        /// socket发送
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="loginInfos">登录信息列表</param>
        /// <returns>状态</returns>
        public int Send(CMessage message, List<ChatData> datas)
        {
            Binary bw = new Binary();
            int chatsize = datas.Count;
            bw.WriteInt(chatsize);
            for (int i = 0; i < chatsize; i++)
            {
                ChatData chat = datas[i];
                bw.WriteInt(chat.m_userID);
                bw.WriteChar((char)chat.m_type);
                bw.WriteString(chat.m_text);
            }
            byte[] bytes = bw.GetBytes();
            message.m_body = bytes;
            message.m_bodyLength = bytes.Length;
            int ret = Send(message);
            bw.Close();
            return ret;
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="message">消息</param>
        /// <returns>状态</returns>
        public int SendToAllClients(CMessage message)
        {
            List<ChatData> datas = new List<ChatData>();
            GetChatDatas(datas, message.m_body, message.m_bodyLength);
            message.m_functionID = FUNCTIONID_CHAT_RECV;
            lock (m_sessions)
            {
                List<int> socketlist = new List<int>();
                foreach (int socketID in m_sessions.Values)
                {
                    message.m_socketID = socketID;
                    int ret = Send(message, datas);
                    if (ret == -1)
                    {
                        socketlist.Add(socketID);
                    }
                }
                int listsize = socketlist.Count;
                for (int i = 0; i < listsize; i++)
                {
                    m_sessions.Remove(socketlist[i]);
                }
            }
            datas.Clear();
            return 1;
        }
        #endregion
    }
}
