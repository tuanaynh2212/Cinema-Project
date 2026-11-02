using System;
using System.Windows.Forms;
using cinema_project.Logging;
using cinema_project.UI;
using Microsoft.Extensions.Logging;

namespace cinema_project
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            AppLogging.Initialize();
            ILogger logger = AppLogging.CreateLogger<frmMain>();

            try
            {
                ApplicationConfiguration.Initialize();
                Application.Run(new frmMain());
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, "Ứng dụng dừng do lỗi không được xử lý.");
                throw;
            }
        }
    }
}