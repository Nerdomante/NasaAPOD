using Microsoft.Extensions.Configuration;
using Microsoft.Toolkit.Uwp.Notifications;
using Nasa.Model.Nasa;
using Newtonsoft.Json;
using System.Timers;
using Windows.Foundation.Collections;
using static Nasa.Core.Utility;

namespace Nasa.Core
{
    public class Deamon : ApplicationContext
    {
        private NotifyIcon trayIcon;
        private AppSettings? settings { get; set; }
        private System.Timers.Timer checkForTime { get; set; }
        private const string storageFileName = "last_call.old";
        public Deamon(IConfiguration config)
        {
            settings = config.GetSection("AppSettings").Get<AppSettings>();

            double interval = 60 * 60 * settings.HoursInterval * 1000; // milliseconds to one hour
            checkForTime = new System.Timers.Timer(interval);
            checkForTime.Elapsed += new ElapsedEventHandler(checkForTime_Elapsed);
            checkForTime.Enabled = true;

            ToolStripMenuItem active = new ToolStripMenuItem("Active", null, new EventHandler(Active), "Active");
            active.Checked = true;
            ToolStripMenuItem info = new ToolStripMenuItem("Info", null, new EventHandler(Info), "Info");
            ToolStripMenuItem exit = new ToolStripMenuItem("Exit", null, new EventHandler(Exit), "Exit");

            ContextMenuStrip menu = new ContextMenuStrip();
            menu.Items.Add(active);
            menu.Items.Add(info);
            menu.Items.Add(exit);

            trayIcon = new NotifyIcon()
            {
                Icon = new Icon("Res/icon.ico"),
                ContextMenuStrip = menu,
                Visible = true
            };

            UpdateWallpaper();

            // Listen to notification activation
            ToastNotificationManagerCompat.OnActivated += toastArgs =>
            {
                // Obtain the arguments from the notification
                ToastArguments args = ToastArguments.Parse(toastArgs.Argument);
                // Obtain any user input (text boxes, menu selections) from the notification
                ValueSet userInput = toastArgs.UserInput;
                args.TryGetValue("title", out string title);
                args.TryGetValue("description", out string description);
                args.TryGetValue("copyright", out string copyright);

                if (description != null)
                {
                    Information infoBox = new Information(title, description + Environment.NewLine + Environment.NewLine + copyright + " © " + DateTime.Now.Year.ToString());
                    infoBox.ShowDialog();
                }
            };
        }

        private void Active(object? sender, EventArgs e)
        {
            ((ToolStripMenuItem)sender).Checked = !((ToolStripMenuItem)sender).Checked;

            if (((ToolStripMenuItem)sender).Checked)
            {
                double interval = 60 * 60 * settings.HoursInterval * 1000; // milliseconds to one hour
                checkForTime = new System.Timers.Timer(interval);
                checkForTime.Elapsed += new ElapsedEventHandler(checkForTime_Elapsed);
                checkForTime.Enabled = true;
                UpdateWallpaper();
            }
            else
            {
                checkForTime.Stop();
                checkForTime.Enabled = false;

                new ToastContentBuilder()
                        .AddText("Servizio in pausa", AdaptiveTextStyle.Title)
                        .AddText("Non verranno più cercate immagini del giorno.", AdaptiveTextStyle.CaptionSubtle)
                        .AddAppLogoOverride(new Uri("file:///" + Path.GetFullPath(@"Res\icon.ico")))
                        .Show();
            }
        }

        private void Info(object? sender, EventArgs e)
        {
            if (File.Exists(storageFileName))
            {
                string oldJsonAPOD = File.ReadAllText(storageFileName);
                APOD oldJsonObject = JsonConvert.DeserializeObject<APOD>(oldJsonAPOD);

                string description = oldJsonObject.explanation +
                    Environment.NewLine +
                    Environment.NewLine +
                    oldJsonObject.copyright +
                    " © " + DateTime.Now.Year.ToString();

                Information infoBox = new Information(oldJsonObject.title, description);
                infoBox.ShowDialog();
            }
        }
        private void Exit(object sender, EventArgs e)
        {
            // Hide tray icon, otherwise it will remain shown until user mouses over it
            if (trayIcon != null)
            {
                trayIcon.Visible = false;
            }
            if (checkForTime != null)
            {
                checkForTime.Stop();
                checkForTime.Enabled = false;
            }
            new ToastContentBuilder()
                        .AddText("Servizio interrotto")
                        .AddText("Non verranno più cercate immagini del giorno.")
                        .AddAppLogoOverride(new Uri("file:///" + Path.GetFullPath(@"Res\icon.ico")))
                        .Show();

            System.Windows.Forms.Application.Exit();
        }
        void checkForTime_Elapsed(object sender, ElapsedEventArgs e)
        {
            UpdateWallpaper();
        }
        private void UpdateWallpaper()
        {
            try
            {
                NasaAPI nasa = new NasaAPI(settings.ApiKey);
                APOD apod = nasa.PictureOfDay(settings);

                trayIcon.Text = "Nasa APOD" +
                    Environment.NewLine +
                    "Last: " + Convert.ToDateTime(apod.date).ToString("dd/MM/yyyy") +
                    Environment.NewLine +
                    "────────────────" +
                    Environment.NewLine +
                    apod.title + 
                    Environment.NewLine +
                    "© " + apod.copyright +
                    Environment.NewLine +
                    "────────────────" +
                    Environment.NewLine;

                if (!File.Exists(storageFileName))
                {

                    File.WriteAllText(storageFileName, JsonConvert.SerializeObject(apod));

                    Wallpaper.Set(new Uri(apod.hdurl), Wallpaper.Style.Center);

                    // Requires Microsoft.Toolkit.Uwp.Notifications NuGet package version 7.0 or greater
                    new ToastContentBuilder()
                        .AddText(apod.title, AdaptiveTextStyle.Subheader)
                        .AddText(apod.explanation, AdaptiveTextStyle.Base)
                        .AddText(apod.copyright, AdaptiveTextStyle.HeaderNumeral)
                        .AddArgument("title", apod.title)
                        .AddArgument("description", apod.explanation)
                        .AddArgument("copyright", apod.copyright)
                        .AddAppLogoOverride(new Uri("file:///" + Path.GetFullPath(@"Res\icon.ico")))
                        .Show();
                }
                else
                {
                    string oldJsonAPOD = File.ReadAllText(storageFileName);
                    APOD oldJsonObject = JsonConvert.DeserializeObject<APOD>(oldJsonAPOD);

                    if (Convert.ToDateTime(apod.date) > Convert.ToDateTime(oldJsonObject.date))
                    {
                        File.WriteAllText(storageFileName, JsonConvert.SerializeObject(apod));

                        Wallpaper.Set(new Uri(apod.hdurl), Wallpaper.Style.Fill);

                        // Requires Microsoft.Toolkit.Uwp.Notifications NuGet package version 7.0 or greater
                        new ToastContentBuilder()
                            .AddText(apod.title, AdaptiveTextStyle.Subheader)
                            .AddText(apod.explanation, AdaptiveTextStyle.Base)
                            .AddText(apod.copyright, AdaptiveTextStyle.HeaderNumeral)
                            .AddArgument("title", apod.title)
                            .AddArgument("description", apod.explanation)
                            .AddArgument("copyright", apod.copyright)
                            .AddAppLogoOverride(new Uri("file:///" + Path.GetFullPath(@"Res\icon.ico")))
                            .Show();
                    }
                }

            }
            catch (Exception ex)
            {
                new ToastContentBuilder()
                   .AddText("Error")
                   .AddText(ex.Message)
                   .AddArgument("title", "Error")
                   .AddArgument("description", ex.Message)
                   .AddAppLogoOverride(new Uri("file:///" + Path.GetFullPath(@"Res\icon.ico")))
                   .Show(); // Not seeing the Show() method? Make sure you have version 7.0, and if you're using .NET 6 (or later), then your TFM must be net6.0-windows10.0.17763.0 or greater
            }
        }
    }
}
