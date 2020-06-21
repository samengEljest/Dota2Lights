using System;
using System.Windows.Forms;

namespace DotaLightApp
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Form1 configForm = new Form1();
            Application.Run(configForm);
        }
    }
}
