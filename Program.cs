using System;
using System.Windows.Forms;
using cinema_project.UI;

namespace cinema_project
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new frmMain());
        }
    }
}