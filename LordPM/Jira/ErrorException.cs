using System;
using System.Collections;
using System.ComponentModel;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Text;
using System.Windows.Forms;

namespace OwLib
{
    /// <summary>
    /// 错误报告
    /// </summary>
    public class ErrorException
    {
        /// <summary>
        /// 错误事件
        /// </summary>
        public static event ErrorEventHandler Error;

        /// <summary>
        /// 触发错误报告
        /// </summary>
        /// <param name="ex">异常</param>
        public static void OnError(Exception ex)
        {
            if (Error != null)
            {
                Error(null, new ErrorEventArgs(ex));
            }
        }
    }
}
