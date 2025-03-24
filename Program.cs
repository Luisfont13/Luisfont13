using Eto.Forms;

public static class Program
{
    [STAThread]
    public static void Main()
    {
        new Application(new Eto.GtkSharp.Platform()).Run(new MainForm());
    }
}
