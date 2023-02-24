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
        Globals env = new Globals();

        private const string storageFileName = "last_call.old";
        public Deamon(IConfiguration config)
        {
            env.settings = config.GetSection("AppSettings").Get<AppSettings>();
            env.checkForTime = new System.Timers.Timer(TimerInterval(env.settings));
            env.checkForTime.Elapsed += new ElapsedEventHandler(checkForTime_Elapsed);
            env.checkForTime.Enabled = true;

            ToolStripMenuItem active = new ToolStripMenuItem("Active", null, new EventHandler(Active), "Active");
            active.Checked = true;
            ToolStripMenuItem info = new ToolStripMenuItem("Info", null, new EventHandler(Info), "Info");
            ToolStripMenuItem exit = new ToolStripMenuItem("Exit", null, new EventHandler(Exit), "Exit");

            ContextMenuStrip menu = new ContextMenuStrip();
            menu.Items.Add(active);
            menu.Items.Add(info);
            menu.Items.Add(exit);

            env.trayIcon = new NotifyIcon()
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
                    Information infoBox = new Information(
                        title, 
                        description + 
                        Environment.NewLine + 
                        Environment.NewLine + 
                        copyright.Replace("\n", " ").Replace("\r", " ") + " © " + DateTime.Now.Year.ToString());

                    infoBox.ShowDialog();
                }
            };
        }

        private void Active(object? sender, EventArgs e)
        {
            ((ToolStripMenuItem)sender).Checked = !((ToolStripMenuItem)sender).Checked;

            if (((ToolStripMenuItem)sender).Checked)
            {
                env.checkForTime = new System.Timers.Timer(TimerInterval(env.settings));
                env.checkForTime.Elapsed += new ElapsedEventHandler(checkForTime_Elapsed);
                env.checkForTime.Enabled = true;
                UpdateWallpaper();
            }
            else
            {
                env.checkForTime.Stop();
                env.checkForTime.Enabled = false;

                ToastManager.Pause();
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
                    oldJsonObject.copyright.Replace("\n", " ").Replace("\r", " ") +
                    " © " + DateTime.Now.Year.ToString();

                Information infoBox = new Information(oldJsonObject.title, description);
                infoBox.ShowDialog();
            }
        }
        private void Exit(object sender, EventArgs e)
        {
            // Hide tray icon, otherwise it will remain shown until user mouses over it
            if (env.trayIcon != null)
            {
                env.trayIcon.Visible = false;
            }
            if (env.checkForTime != null)
            {
                env.checkForTime.Stop();
                env.checkForTime.Enabled = false;
            }
            ToastManager.Stop();

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
                NasaAPI nasa = new NasaAPI(env.settings.ApiKey);
                APOD apod = nasa.PictureOfDay(env.settings);

                string tooltip = "Nasa APOD" +
                    Environment.NewLine +
                    "Last: " + Convert.ToDateTime(apod.date).ToString("dd/MM/yyyy") +
                    Environment.NewLine +
                    "────────────────" +
                    Environment.NewLine +
                    apod.title +
                    Environment.NewLine +
                    "© " + apod.copyright.Replace("\n", " ").Replace("\r", " ") +
                    Environment.NewLine +
                    "────────────────" +
                    Environment.NewLine;
                env.trayIcon.Text = tooltip.Truncate(127);

                if (!File.Exists(storageFileName))
                {
                    File.WriteAllText(storageFileName, JsonConvert.SerializeObject(apod));

                    Wallpaper.Set(new Uri(apod.hdurl), Wallpaper.Style.Fill);

                    ToastManager.Notify(apod);
                }
                else
                {
                    string oldJsonAPOD = File.ReadAllText(storageFileName);
                    APOD oldJsonObject = JsonConvert.DeserializeObject<APOD>(oldJsonAPOD);

                    if (Convert.ToDateTime(apod.date) > Convert.ToDateTime(oldJsonObject.date))
                    {
                        File.WriteAllText(storageFileName, JsonConvert.SerializeObject(apod));

                        Wallpaper.Set(new Uri(apod.hdurl), Wallpaper.Style.Fill);

                        ToastManager.Notify(apod);
                    }
                }
            }
            catch (Exception ex)
            {
                ToastManager.Error(ex);
            }
        }
    }
}
