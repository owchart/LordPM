/*****************************************************************************\
*                                                                             *
* Binary.cs -  Binary functions, types, and definitions                       *
*                                                                             *
*               Version 1.00                                                  *
*                                                                             *
*               Copyright (c) 2016-2016, Lord's server. All rights reserved.  *
*                                                                             *
*******************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace OwLib
{
    public class Binary
    {
        #region Lord 2016/07/21
        /// <summary>
        /// ������
        /// </summary>
        public Binary()
        {
            m_outputStream = new MemoryStream();
            m_writer = new BinaryWriter(m_outputStream, Encoding.UTF8);
        }

        /// <summary>
        /// ������
        /// </summary>
        private MemoryStream m_inputStream;

        /// <summary>
        /// �����
        /// </summary>
        private MemoryStream m_outputStream;

        /// <summary>
        /// ��ȡ��
        /// </summary>
        private BinaryReader m_reader;

        /// <summary>
        /// д����
        /// </summary>
        private BinaryWriter m_writer;

        /// <summary>
        /// �ر�
        /// </summary>
        public void Close()
        {
            try
            {
                if (m_reader != null)
                {
                    m_reader.Close();
                }
                if (m_writer != null)
                {
                    m_writer.Close();
                }
                if (m_inputStream != null)
                {
                    m_inputStream.Close();
                    m_inputStream.Dispose();
                }
                if (m_outputStream != null)
                {
                    m_outputStream.Close();
                    m_outputStream.Dispose();
                }
            }
            catch
            {

            }
        }

        /// <summary>
        /// ��ȡBytes������
        /// </summary>
        /// <returns>Bytes������</returns>
        public byte[] GetBytes()
        {
            return m_outputStream.ToArray();
        }

        /// <summary>
        /// ��ȡ����������
        /// </summary>
        /// <returns>����������</returns>
        public bool ReadBool()
        {
            return m_reader.ReadBoolean();
        }

        /// <summary>
        /// ��ȡByte������
        /// </summary>
        /// <returns>Byte������</returns>
        public byte ReadByte()
        {
            return m_reader.ReadByte();
        }

        /// <summary>
        /// ��ȡ������
        /// </summary>
        /// <param name="bytes">������</param>
        public void ReadBytes(byte[] bytes)
        {
            int bytesSize = bytes.Length;
            for (int i = 0; i < bytesSize; i++)
            {
                bytes[i] = m_reader.ReadByte();
            }
        }

        /// <summary>
        /// ��ȡChar����
        /// </summary>
        /// <returns>Char����</returns>
        public char ReadChar()
        {
            return m_reader.ReadChar();
        }

        /// <summary>
        /// ��ȡDouble����
        /// </summary>
        /// <returns>Double����</returns>
        public double ReadDouble()
        {
            return m_reader.ReadDouble();
        }

        /// <summary>
        /// ��ȡFloat����
        /// </summary>
        /// <returns>Float����</returns>
        public float ReadFloat()
        {
            return m_reader.ReadSingle();
        }

        /// <summary>
        /// ��ȡInt����
        /// </summary>
        /// <returns>Int����</returns>
        public int ReadInt()
        {
            return m_reader.ReadInt32();
        }

        /// <summary>
        /// ��ȡShort����
        /// </summary>
        /// <returns>Short����</returns>
        public short ReadShort()
        {
            return m_reader.ReadInt16();
        }

        /// <summary>
        /// ��ȡ�ַ�������
        /// </summary>
        /// <returns>�ַ�������</returns>
        public String ReadString()
        {
            int size = m_reader.ReadInt32();
            byte[] bytes = m_reader.ReadBytes(size);
            return Encoding.UTF8.GetString(bytes);
        }

        /// <summary>
        /// д����
        /// </summary>
        /// <param name="bytes">��</param>
        /// <param name="len">����</param>
        public void Write(byte[] bytes, int len)
        {
            m_inputStream = new MemoryStream(bytes);
            m_reader = new BinaryReader(m_inputStream, Encoding.UTF8);
        }

        /// <summary>
        /// д��Bool������
        /// </summary>
        /// <param name="val">Bool������</param>
        public void WriteBool(bool val)
        {
            m_writer.Write(val);
        }

        /// <summary>
        /// д��Byte������
        /// </summary>
        /// <param name="val">Byte������</param>
        public void WriteByte(byte val)
        {
            m_writer.Write(val);
        }

        /// <summary>
        /// д��Bytes����
        /// </summary>
        /// <param name="val">Bytes����</param>
        public void WriteBytes(byte[] val)
        {
            m_writer.Write(val);
        }

        /// <summary>
        /// д��Char������
        /// </summary>
        /// <param name="val">Char������</param>
        public void WriteChar(char val)
        {
            m_writer.Write(val);
        }

        /// <summary>
        /// д��Double������
        /// </summary>
        /// <param name="val">Double������</param>
        public void WriteDouble(double val)
        {
            m_writer.Write(val);
        }

        /// <summary>
        /// д��Float������
        /// </summary>
        /// <param name="val">Float������</param>
        public void WriteFloat(float val)
        {
            m_writer.Write(val);
        }

        /// <summary>
        /// д��Int������
        /// </summary>
        /// <param name="val">Int������</param>
        public void WriteInt(int val)
        {
            m_writer.Write(val);
        }

        /// <summary>
        /// д��Short������
        /// </summary>
        /// <param name="val">Short������</param>
        public void WriteShort(short val)
        {
            m_writer.Write(val);
        }

        /// <summary>
        /// д���ַ�������
        /// </summary>
        /// <param name="val">�ַ�������</param>
        public void WriteString(String val)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(val);
            m_writer.Write(bytes.Length);
            m_writer.Write(bytes);
        }
        #endregion
    }
}

