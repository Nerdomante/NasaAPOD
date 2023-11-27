using Microsoft.Extensions.Configuration;
using Microsoft.Toolkit.Uwp.Notifications;
using Nasa.Model.Nasa;
using System;
using System.Diagnostics;
using System.Net;
using System.Text.Json;
using System.Timers;
using Windows.Foundation.Collections;
using static Nasa.Core.Utility;

namespace Nasa.Core
{
    public class Deamon : ApplicationContext
    {
        Globals env = new Globals();
        IConfigurationRoot _config;
        public Deamon(IConfigurationRoot config)
        {
            _config = config;
            env.settings = config.GetSection("AppSettings").Get<AppSettings>();
            env.checkForTime = new System.Timers.Timer(TimerInterval(env.settings));
            env.checkForTime.Elapsed += new ElapsedEventHandler(checkForTime_Elapsed);
            env.checkForTime.Enabled = true;

            ToolStripMenuItem info = new ToolStripMenuItem("APOD's Info", Image.FromFile(Application.StartupPath + "/Res/UI/telescope.png"), new EventHandler(Info), "APOD's Info");
            info.ImageScaling = ToolStripItemImageScaling.SizeToFit;
            ToolStripMenuItem force = new ToolStripMenuItem("Update", Image.FromFile(Application.StartupPath + "/Res/UI/reload.png"), new EventHandler(Force), "Update");
            force.ImageScaling = ToolStripItemImageScaling.SizeToFit;
            ToolStripMenuItem active = new ToolStripMenuItem("Pause", Image.FromFile(Application.StartupPath + "/Res/UI/pause.png"), new EventHandler(Active), "Pause");
            active.ImageScaling = ToolStripItemImageScaling.SizeToFit;
            active.Checked = true;
            ToolStripMenuItem about = new ToolStripMenuItem("About", Image.FromFile(Application.StartupPath + "/Res/UI/info.png"), new EventHandler(About), "About");
            about.ImageScaling = ToolStripItemImageScaling.SizeToFit;
            ToolStripMenuItem settings = new ToolStripMenuItem("Settings", Image.FromFile(Application.StartupPath + "/Res/UI/gear.png"), new EventHandler(Settings), "Settings");
            settings.ImageScaling = ToolStripItemImageScaling.SizeToFit;
            ToolStripMenuItem exit = new ToolStripMenuItem("Exit", Image.FromFile(Application.StartupPath + "/Res/UI/exit.png"), new EventHandler(Exit), "Exit");
            exit.ImageScaling = ToolStripItemImageScaling.SizeToFit;
            
            ContextMenuStrip menu = new ContextMenuStrip();
            menu.Items.Add(info);
            menu.Items.Add(force);
            menu.Items.Add(active);
            menu.Items.Add(about);
            menu.Items.Add(settings);
            menu.Items.Add(exit);

            env.trayIcon = new NotifyIcon()
            {
                Icon = new Icon("Res/icon.ico"),
                ContextMenuStrip = menu,
                Visible = true,
                Text = $"Updating...".Truncate(127)
            };

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

            UpdateWallpaperAsync();
        }

        private void Info(object? sender, EventArgs e)
        {
            if (File.Exists(Globals.storageFileName))
            {
                string oldJsonAPOD = File.ReadAllText(Globals.storageFileName);
                APOD oldJsonObject = JsonSerializer.Deserialize<APOD>(oldJsonAPOD);

                string description = oldJsonObject.explanation +
                    (String.IsNullOrEmpty(oldJsonObject.copyright) ? Environment.NewLine + Environment.NewLine + "Nasa © " + DateTime.Now.Year.ToString() : Environment.NewLine + Environment.NewLine + oldJsonObject.copyright.Replace("\n", " ").Replace("\r", " ") + " © " + DateTime.Now.Year.ToString());

                Information infoBox = new Information(oldJsonObject.title, description, env);
                infoBox.ShowDialog();
            }
        }

        private async void Force(object? sender, EventArgs e)
        {
            await UpdateWallpaperAsync(true);
        }

        private void Active(object? sender, EventArgs e)
        {
            ((ToolStripMenuItem)sender).Checked = !((ToolStripMenuItem)sender).Checked;

            if (((ToolStripMenuItem)sender).Checked)
            {
                ((ToolStripMenuItem)sender).Image = Image.FromFile(Application.StartupPath + "/Res/UI/pause.png");
                ((ToolStripMenuItem)sender).Text = "Pause";
                ((ToolStripMenuItem)sender).ImageScaling = ToolStripItemImageScaling.SizeToFit;

                env.checkForTime = new System.Timers.Timer(TimerInterval(env.settings));
                env.checkForTime.Elapsed += new ElapsedEventHandler(checkForTime_Elapsed);
                env.checkForTime.Enabled = true;
                UpdateWallpaperAsync();
            }
            else
            {
                env.checkForTime.Stop();
                env.checkForTime.Enabled = false;

                ((ToolStripMenuItem)sender).Image = Image.FromFile(Application.StartupPath + "/Res/UI/resume.png");
                ((ToolStripMenuItem)sender).Text = "Resume";
                ((ToolStripMenuItem)sender).ImageScaling = ToolStripItemImageScaling.SizeToFit;

                ToastManager.Pause();
            }
        }

        private void About(object? sender, EventArgs e)
        {
            string url = "https://github.com/Nerdomante/NasaAPOD";

            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                // Gestisci eventuali eccezioni
                Console.WriteLine("Errore durante l'apertura dell'URL: " + ex.Message);
            }
        }

        private void Settings(object? sender, EventArgs e)
        {
            FormSettings settingsBox = new FormSettings(_config);
            if (settingsBox.Visible == false)
            {
                settingsBox.ShowDialog();
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

        private async void checkForTime_Elapsed(object sender, ElapsedEventArgs e)
        {
            await UpdateWallpaperAsync();
        }

        private async Task UpdateWallpaperAsync(bool forced = false)
        {
            try
            {
                if (!IsInternetAvailable(env.settings.Endpoint))
                {
                    throw new Exception("Internet connection was lost");
                }
                NasaAPI nasa = new NasaAPI(env.settings.ApiKey);
                APOD apod = new APOD();

                if (!File.Exists(Globals.storageFileName))
                {
                    apod = await nasa.PictureOfDayAsync(env.settings);
                    await SetWallpaperAsync(apod);
                }
                else
                {
                    string oldJsonAPOD = File.ReadAllText(Globals.storageFileName);
                    apod = JsonSerializer.Deserialize<APOD>(oldJsonAPOD);
                    APOD oldapod = JsonSerializer.Deserialize<APOD>(oldJsonAPOD);

                    if (Convert.ToDateTime(DateTime.Now.Date) > Convert.ToDateTime(oldapod.date) || forced)
                    {
                        apod = await nasa.PictureOfDayAsync(env.settings);
                        if (Convert.ToDateTime(oldapod.date) < Convert.ToDateTime(apod.date) || forced)
                        {
                            await SetWallpaperAsync(apod);
                        }
                    }
                }

                string tooltip = "Nasa APOD" +
                    Environment.NewLine +
                    "Last: " + Convert.ToDateTime(apod.date).ToString("dd/MM/yyyy") +
                    Environment.NewLine +
                    Environment.NewLine +
                    apod.title +
                    Environment.NewLine +
                    (String.IsNullOrEmpty(apod.copyright) ? Environment.NewLine + "Nasa © " + DateTime.Now.Year : Environment.NewLine + apod.copyright.Replace("\n", " ").Replace("\r", " ") + " © " + DateTime.Now.Year);
                env.trayIcon.Text = tooltip.Truncate(127);

            }
            catch (Exception ex)
            {
                ToastManager.Error(ex);
            }
        }

        private async Task SetWallpaperAsync(APOD apod)
        {
            File.WriteAllText(Globals.storageFileName, JsonSerializer.Serialize(apod));

            Image? wall = null;
            Image img = null;

            if (apod.media_type.ToLower() == "video")
            {
                img = await Images.GetVideoThumbnailAsync(apod.url);
            }
            else
            {
                try
                {
                    img = await Images.GetImageAsync(apod.hdurl);
                }
                catch (WebException ex)
                {
                    img = await Images.GetImageAsync(apod.url);
                }
            }

            if (img.Height > img.Width || (img.Height - img.Width) <= env.settings.Ratio)
            {
                if (img.Height >= Screen.PrimaryScreen.Bounds.Height || (Screen.PrimaryScreen.Bounds.Height - img.Height) <= env.settings.ScaleThresholdHeight)
                {
                    img = Images.ScaleImage(img, Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
                }
                if ((Screen.PrimaryScreen.Bounds.Width - img.Width) <= env.settings.ScaleThresholdWidth)
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
