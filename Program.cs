using CinemaManagement;

namespace cinema_project;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.Run(new Login());
    }
}
