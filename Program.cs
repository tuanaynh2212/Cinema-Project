using System;
using System.Windows.Forms;
using cinema_project.UI;

namespace cinema_project
{
    internal static class Program
    {
        /// <summary>
        /// Điểm khởi nhập chính của ứng dụng Windows Forms CGV Cinema.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new frmMain());
        }
    }
}