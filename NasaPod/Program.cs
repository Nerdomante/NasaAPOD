using Microsoft.Extensions.Configuration;

namespace Nasa.Core
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            IConfiguration configuration = builder.Build();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            using (Mutex mutex = new Mutex(false, "Global\\" + appGuid))
            {
                if (!mutex.WaitOne(0, false))
                {
                    MessageBox.Show($"{AppDomain.CurrentDomain.FriendlyName}.exe already running.");
                    return;
                }
                Application.Run(new Deamon(configuration));
            }
        }
        private static string appGuid = "7d3122d5-4a48-41a9-b34f-a4bd5dc2d549";
    }
}