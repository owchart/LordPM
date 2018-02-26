/*****************************************************************************\
*                                                                             *
* Str.cs -    Str functions, types, and definitions.                          *
*                                                                             *
*               Version 1.00 ������                                       *
*                                                                             *
*               Copyright (c) 2016-2016, Server. All rights reserved.         *
*               Created by Todd.                                              *
*                                                                             *
*******************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace OwLib
{
    public class CStrA
    {
        #region Lord 2016/1/20
        /// <summary>
        /// ����Ʊ����ת��Ϊ�ٶȴ���
        /// </summary>
        /// <param name="code">��Ʊ����</param>
        /// <returns>�ٶȴ���</returns>
        public static String ConvertDBCodeToBaiduCode(String code)
        {
            String securityCode = code;
            int index = securityCode.IndexOf(".");
            if (index > 0)
            {
                securityCode = securityCode.Substring(0, index);
            }
            return securityCode;
        }

        /// <summary>
        /// ����Ʊ����ת��Ϊ�ٶȴ���
        /// </summary>
        /// <param name="code">��Ʊ����</param>
        /// <returns>�ٶȴ���</returns>
        public static String ConvertDBCodeToDzhCode(String code)
        {
            String securityCode = code;
            int index = securityCode.IndexOf(".SH");
            if (index > 0)
            {
                securityCode = "sh" + securityCode.Substring(0, securityCode.IndexOf("."));
            }
            else
            {
                securityCode = "sz" + securityCode.Substring(0, securityCode.IndexOf("."));
            }
            return securityCode;
        }


        /// <summary>
        /// ��ȡ֤ȯ������ļ�����
        /// </summary>
        /// <param name="code">����</param>
        /// <returns>�ļ�����</returns>
        public static String ConvertDBCodeToFileName(String code)
        {
            String fileName = code;
            if (fileName.IndexOf(".") != -1)
            {
                fileName = fileName.Substring(fileName.IndexOf('.') + 1) + fileName.Substring(0, fileName.IndexOf('.'));
            }
            fileName += ".txt";
            return fileName;
        }

        /// <summary>
        /// ����Ʊ����ת��Ϊ���˴���
        /// </summary>
        /// <param name="code">��Ʊ����</param>
        /// <returns>���˴���</returns>
        public static String ConvertDBCodeToSinaCode(String code)
        {
            String securityCode = code;
            int index = securityCode.IndexOf(".SH");
            if (index > 0)
            {
                securityCode = "sh" + securityCode.Substring(0, securityCode.IndexOf("."));
            }
            else
            {
                securityCode = "sz" + securityCode.Substring(0, securityCode.IndexOf("."));
            }
            return securityCode;
        }


        /// <summary>
        /// ����Ʊ����ת��Ϊ���ƴ���
        /// </summary>
        /// <param name="code">��Ʊ����</param>
        /// <returns>���ƴ���</returns>
        public static String ConvertDBCodeToEastmoneyCode(String code)
        {
            String securityCode = code;
            int index = securityCode.IndexOf(".SH");
            if (index > 0)
            {
                securityCode = securityCode.Substring(0, securityCode.IndexOf("."));
            }
            else
            {
                securityCode = securityCode.Substring(0, securityCode.IndexOf("."));
            }
            return securityCode;
        }

        /// <summary>
        /// ����Ʊ����ת��Ϊ��Ѷ����
        /// </summary>
        /// <param name="code">��Ʊ����</param>
        /// <returns>��Ѷ����</returns>
        public static String ConvertDBCodeToTencentCode(String code)
        {
            String securityCode = code;
            int index = securityCode.IndexOf(".");
            if (index > 0)
            {
                index = securityCode.IndexOf(".SH");
                if (index > 0)
                {
                    securityCode = "sh" + securityCode.Substring(0, securityCode.IndexOf("."));
                }
                else
                {
                    securityCode = "sz" + securityCode.Substring(0, securityCode.IndexOf("."));
                }
            }
            return securityCode;
        }

        /// <summary>
        /// �����˴���ת��Ϊ��Ʊ����
        /// </summary>
        /// <param name="code">���˴���</param>
        /// <returns>��Ʊ����</returns>
        public static String ConvertSinaCodeToDBCode(String code)
        {
            int equalIndex = code.IndexOf('=');
            int startIndex = code.IndexOf("var hq_str_") + 11;
            String securityCode = equalIndex > 0 ? code.Substring(startIndex, equalIndex - startIndex) : code;
            securityCode = securityCode.Substring(2) + "." + securityCode.Substring(0, 2).ToUpper();
            return securityCode;
        }

        /// <summary>
        /// �ַ���ת��Ϊ������
        /// </summary>
        /// <param name="str">�ַ���</param>
        /// <returns>��ֵ</returns>
        public static double ConvertStrToDouble(String str)
        {
            double value = 0;
            double.TryParse(str, out value);
            return value;
        }

        /// <summary>
        /// �ַ���ת��Ϊ����
        /// </summary>
        /// <param name="str">�ַ���</param>
        /// <returns>��ֵ</returns>
        public static int ConvertStrToInt(String str)
        {
            int value = 0;
            int.TryParse(str, out value);
            return value;
        }

        /// <summary>
        /// ����Ѷ����ת��Ϊ��Ʊ����
        /// </summary>
        /// <param name="code">��Ѷ����</param>
        /// <returns>��Ʊ����</returns>
        public static String ConvertTencentCodeToDBCode(String code)
        {
            int equalIndex = code.IndexOf('=');
            String securityCode = equalIndex > 0 ? code.Substring(0, equalIndex) : code;
            if (securityCode.StartsWith("v_sh"))
            {
                securityCode = securityCode.Substring(4) + ".SH";
            }
            else if (securityCode.StartsWith("v_sz"))
            {
                securityCode = securityCode.Substring(4) + ".SZ";
            }
            return securityCode;
        }

        /// <summary>
        /// ��ȡ���ݿ�ת���ַ���
        /// </summary>
        /// <param name="str">�ַ���</param>
        /// <returns>ת���ַ���</returns>
        public static String GetDBString(String str)
        {
            return str.Replace("'", "''");
        }

        /// <summary>
        /// ��ȡʱ����ֵ
        /// </summary>
        /// <param name="tm_year">��</param>
        /// <param name="tm_mon">��</param>
        /// <param name="tm_mday">��</param>
        /// <param name="tm_hour">Сʱ</param>
        /// <param name="tm_min">����</param>
        /// <param name="tm_sec">��</param>
        /// <param name="tm_msec">����</param>
        /// <returns>ʱ����ֵ</returns>
        public static double M129(int tm_year, int tm_mon, int tm_mday, int tm_hour, int tm_min, int tm_sec, int tm_msec)
        {
            return (new DateTime(tm_year, tm_mon, tm_mday, tm_hour, tm_min, tm_sec) - new DateTime(1970, 1, 1)).TotalSeconds;
        }

        /// <summary>
        /// ��ȡʱ��
        /// </summary>
        /// <param name="num">��ֵ</param>
        /// <param name="tm_year">��</param>
        /// <param name="tm_mon">��</param>
        /// <param name="tm_mday">��</param>
        /// <param name="tm_hour">Сʱ</param>
        /// <param name="tm_min">����</param>
        /// <param name="tm_sec">��</param>
        /// <param name="tm_msec">����</param>
        public static void M130(double num, ref int tm_year, ref int tm_mon, ref int tm_mday, ref int tm_hour, ref int tm_min, ref int tm_sec, ref int tm_msec)
        {
            DateTime date = new DateTime(new DateTime(1970, 1, 1).Ticks + (long)num * 10000000);
            tm_year = date.Year;
            tm_mon = date.Month;
            tm_mday = date.Day;
            tm_hour = date.Hour;
            tm_min = date.Minute;
            tm_sec = date.Second;
        }

        /// <summary>
        /// �����ж�ȡ�ַ���
        /// </summary>
        /// <param name="reader">��ȡ��</param>
        /// <returns>�ַ���</returns>
        public static String ReadString(BinaryReader reader)
        {
            int size = reader.ReadInt32();
            byte[] bytes = reader.ReadBytes(size);
            return Encoding.UTF8.GetString(bytes);
        }

        /// <summary>
        /// ������д���ַ���
        /// </summary>
        /// <param name="writer">д����</param>
        /// <param name="str">�ַ���</param>
        public static void WriteString(BinaryWriter writer, String str)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(str);
            writer.Write(bytes.Length);
            writer.Write(bytes);
        }
        #endregion
    }
}