using Microsoft.Extensions.Configuration;
using Microsoft.Toolkit.Uwp.Notifications;
using Nasa.Model.Nasa;
using Newtonsoft.Json;
using System.Net;
using System.Timers;
using Windows.Foundation.Collections;
using static Nasa.Core.Utility;

namespace Nasa.Core
{
    public class Deamon : ApplicationContext
    {
        Globals env = new Globals();
        public Deamon(IConfiguration config)
        {
            env.settings = config.GetSection("AppSettings").Get<AppSettings>();
            env.checkForTime = new System.Timers.Timer(TimerInterval(env.settings));
            env.checkForTime.Elapsed += new ElapsedEventHandler(checkForTime_Elapsed);
            env.checkForTime.Enabled = true;

            ToolStripMenuItem active = new ToolStripMenuItem("Active", null, new EventHandler(Active), "Active");
            active.Checked = true;
            ToolStripMenuItem force = new ToolStripMenuItem("Update", null, new EventHandler(Force), "Update");
            ToolStripMenuItem info = new ToolStripMenuItem("Info", null, new EventHandler(Info), "Info");
            ToolStripMenuItem exit = new ToolStripMenuItem("Exit", null, new EventHandler(Exit), "Exit");

            ContextMenuStrip menu = new ContextMenuStrip();
            menu.Items.Add(active);
            menu.Items.Add(force);
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
                        (String.IsNullOrEmpty(copyright) ? "" : Environment.NewLine + Environment.NewLine + copyright.Replace("\n", " ").Replace("\r", " ") + " © " + DateTime.Now.Year.ToString()),
                        env
                        );

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

        private void Force(object? sender, EventArgs e)
        {
            UpdateWallpaper(true);
        }

        private void Info(object? sender, EventArgs e)
        {
            if (File.Exists(Globals.storageFileName))
            {
                string oldJsonAPOD = File.ReadAllText(Globals.storageFileName);
                APOD oldJsonObject = JsonConvert.DeserializeObject<APOD>(oldJsonAPOD);

                string description = oldJsonObject.explanation +
                    (String.IsNullOrEmpty(oldJsonObject.copyright) ? Environment.NewLine + Environment.NewLine + "Nasa © " + DateTime.Now.Year.ToString() : Environment.NewLine + Environment.NewLine + oldJsonObject.copyright.Replace("\n", " ").Replace("\r", " ") + " © " + DateTime.Now.Year.ToString());

                Information infoBox = new Information(oldJsonObject.title, description, env);
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
        private void UpdateWallpaper(bool forced = false)
        {
            try
            {
                if (!IsInternetAvailable(env.settings.Endpoint))
                {
                    throw new Exception("Internet connection was lost");
                }
                NasaAPI nasa = new NasaAPI(env.settings.ApiKey);
                APOD apod = nasa.PictureOfDay(env.settings);

                string tooltip = "Nasa APOD" +
                    Environment.NewLine +
                    "Last: " + Convert.ToDateTime(apod.date).ToString("dd/MM/yyyy") +
                    Environment.NewLine +
                    Environment.NewLine +
                    apod.title +
                    Environment.NewLine +
                    (String.IsNullOrEmpty(apod.copyright) ? Environment.NewLine + "Nasa © " + DateTime.Now.Year : Environment.NewLine + apod.copyright.Replace("\n", " ").Replace("\r", " ") + " © " + DateTime.Now.Year);
                env.trayIcon.Text = tooltip.Truncate(127);

                if (!File.Exists(Globals.storageFileName))
                {
                    SetWallpaper(apod);
                }
                else
                {
                    string oldJsonAPOD = File.ReadAllText(Globals.storageFileName);
                    APOD oldJsonObject = JsonConvert.DeserializeObject<APOD>(oldJsonAPOD);

                   if (Convert.ToDateTime(apod.date) > Convert.ToDateTime(oldJsonObject.date) || forced)
                   {
                        SetWallpaper(apod);
                   }
                }
            }
            catch (Exception ex)
            {
                ToastManager.Error(ex);
            }
        }

        private void SetWallpaper(APOD apod)
        {
            File.WriteAllText(Globals.storageFileName, JsonConvert.SerializeObject(apod));

            Image? wall = null;
            Image img = null;

            if (apod.media_type.ToLower() == "video")
            {
                img = Images.GetVideoThumbnail(apod.url).Result;
            }
            else
            {
                try
                {
                    img = Images.GetImage(apod.hdurl);
                }
                catch(WebException ex)
                {
                    img = Images.GetImage(apod.url);
                }
            }
            
            if (img.Height > img.Width || (img.Height - img.Width) <= env.settings.Ratio)
            {
                if (img.Height >= Screen.PrimaryScreen.Bounds.Height || (Screen.PrimaryScreen.Bounds.Height - img.Height) <= env.settings.ScaleThresholdHeight)
                {
                    img = Images.ScaleImage(img, Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
                }
                if((Screen.PrimaryScreen.Bounds.Width - img.Width) <= env.settings.ScaleThresholdWidth)
                {
                    wall = Images.Save(img);
                    Wallpaper.Set(wall, Wallpaper.Style.Fill);
                }
                else
                {
                    wall = Images.FillImage(img, env.settings);
                    wall = Images.Save(wall);
                    Wallpaper.Set(wall, Wallpaper.Style.Center);
                }
            }
            else
            {
                wall = Images.ScaleImage(img, Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
                wall = Images.Save(wall);
                Wallpaper.Set(wall, Wallpaper.Style.Fill);
            }

            ToastManager.Notify(apod);
        }
    }
}
