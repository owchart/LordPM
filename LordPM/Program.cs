using System;
using System.Collections.Generic;
using System.Windows.Forms;
using OwLib;

namespace OwLib
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //DataCenter.Init("http://101.132.165.167:8089/");
            JiraListForm jiraListForm = new JiraListForm();
            jiraListForm.LoadXml("MainFrame");
            Application.Run(jiraListForm);
        }
    }
}