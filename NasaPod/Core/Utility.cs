using Microsoft.Win32;
using System.Net;
using System.Runtime.InteropServices;
using System.Web;

namespace Nasa.Core
{
    internal static class StringExt
    {
        public static string Truncate(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }
    }

    internal class Utility
    {
        public class Globals
        {
            public NotifyIcon trayIcon;
            public AppSettings? settings { get; set; }
            public System.Timers.Timer checkForTime { get; set; }
        }        

        public static double TimerInterval(AppSettings settings)
        {
            double timerInterval = 60 * 60 * settings.HoursInterval * 1000; // milliseconds to one hour
            return timerInterval;
        }

        /// <summary>
        /// Traduce una stringa sfruttando il supporto online
        /// </summary>
        /// <param name="text">Testo da tradurre</param>
        /// <param name="from">Lingua di partenza</param>
        /// <param name="to">Lingua in cui tradurre</param>
        /// <returns>Testo tradotto <see cref="string"/></returns>
        public static string Translate(string text)
        {
            string[] strArr = text.Split('\n');
            string description = strArr[0].Replace("\n", "").Replace("\r", "");
            string copyright = strArr[2].Replace("\n", "").Replace("\r", "");

            string[] splittedDescription = description.Split('.');
            List<String> list = splittedDescription.ToList<string>();
            List<String> translated = new List<String>();

            foreach (string s in list)
            {
                if (!String.IsNullOrEmpty(s))
                {
                    string fromLanguage = "en";
                    string toLanguage = "it";
                    string url = $"https://translate.googleapis.com/translate_a/single?client=gtx&sl={fromLanguage}&tl={toLanguage}&dt=t&q={HttpUtility.UrlEncode(s+".")}";
                    WebClient webClient = new WebClient
                    {
                        Encoding = System.Text.Encoding.UTF8
                    };
                    string result = webClient.DownloadString(url);
                    result = result.Substring(4, result.IndexOf("\"", 4, StringComparison.Ordinal) - 4);
                    translated.Add(result + " ");
                }
            }
            string translation = "";
            foreach (string s in translated)
            {
                translation += s;
            }
            return translation + Environment.NewLine + Environment.NewLine + copyright;
        }

        public sealed class Wallpaper
        {
            Wallpaper() { }

            const int SPI_SETDESKWALLPAPER = 20;
            const int SPIF_UPDATEINIFILE = 0x01;
            const int SPIF_SENDWININICHANGE = 0x02;

            [DllImport("user32.dll", CharSet = CharSet.Auto)]
            static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);

            public enum Style : int
            {
                Fill,
                Fit,
                Span,
                Stretch,
                Tile,
                Center
            }

            public static void Set(Uri uri, Style style)
            {
                string filename = uri.Segments.LastOrDefault();
                System.IO.Stream s = new System.Net.WebClient().OpenRead(uri.ToString());

                System.Drawing.Image img = System.Drawing.Image.FromStream(s);
                string tempPath = Path.Combine(Path.GetTempPath(), filename);
                img.Save(tempPath);

                RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", true);
                if (style == Style.Fill)
                {
                    key.SetValue(@"WallpaperStyle", 10.ToString());
                    key.SetValue(@"TileWallpaper", 0.ToString());
                }
                if (style == Style.Fit)
                {
                    key.SetValue(@"WallpaperStyle", 6.ToString());
                    key.SetValue(@"TileWallpaper", 0.ToString());
                }
                if (style == Style.Span) // Windows 8 or newer only!
                {
                    key.SetValue(@"WallpaperStyle", 22.ToString());
                    key.SetValue(@"TileWallpaper", 0.ToString());
                }
                if (style == Style.Stretch)
                {
                    key.SetValue(@"WallpaperStyle", 2.ToString());
                    key.SetValue(@"TileWallpaper", 0.ToString());
                }
                if (style == Style.Tile)
                {
                    key.SetValue(@"WallpaperStyle", 0.ToString());
                    key.SetValue(@"TileWallpaper", 1.ToString());
                }
                if (style == Style.Center)
                {
                    key.SetValue(@"WallpaperStyle", 0.ToString());
                    key.SetValue(@"TileWallpaper", 0.ToString());
                }

                SystemParametersInfo(SPI_SETDESKWALLPAPER,
                    0,
                    tempPath,
                    SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);
            }
        }
    }
}
