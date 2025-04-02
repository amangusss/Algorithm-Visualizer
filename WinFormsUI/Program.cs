using System.Windows;

namespace Algorithm_Visualizer.WinFormsUI;

internal static class Program
{
    [STAThread]
    private static void Main() {
        ApplicationConfiguration.Initialize();
        Application.Run(new MainForm());
    }
}