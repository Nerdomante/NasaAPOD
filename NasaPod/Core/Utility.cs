using Microsoft.Win32;
using System.Drawing.Imaging;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
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
        public static IEnumerable<string> SplitAndKeep(this string input, char[] delimiters)
        {
            List<string> output = new List<string>();

            int startIndex = 0;
            for (int i = 0; i < input.Length; i++)
            {
                if (delimiters.Contains(input[i]))
                {
                    int length = i - startIndex + 1;
                    string substring = input.Substring(startIndex, length);
                    output.Add(substring);

                    startIndex = i + 1;
                }
            }

            // Aggiunge l'ultima sottostringa (che non termina con un delimitatore)
            if (startIndex < input.Length)
            {
                string substring = input.Substring(startIndex);
                output.Add(substring);
            }

            List<string> reslist = new List<string>();
            // Stampa il risultato
            foreach (string s in output)
            {
                reslist.Add(s);
            }
            return reslist;
        }
    }

    public class Utility
    {
        public class Globals
        {
            public const string storageFileName = "last_call.old";

            public NotifyIcon trayIcon;
            public AppSettings? settings { get; set; }
            public System.Timers.Timer checkForTime { get; set; }
        }

        public static bool IsInternetAvailable(string endpoint)
        {
            try
            {
                using (var client = new WebClient())
                using (var stream = client.OpenRead(endpoint))
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public static double TimerInterval(AppSettings settings)
        {

#if DEBUG
            double timerInterval = 60000; // one minute in debugging
#else
            double timerInterval = 60 * 60 * settings.HoursInterval * 1000; // milliseconds to one hour
#endif
            return timerInterval;

        }

        /// <summary>
        /// Traduce una stringa sfruttando il supporto online
        /// </summary>
        /// <param name="text">text to translate</param>
        /// <param name="fromLang">from language</param>
        /// <param name="toLang">to language</param>
        /// <returns><see cref="string"/> translated text</returns>
        public static async Task<string> Translate(string text, string fromLang, string toLang)
        {
            string[] strArr = text.Split('\n');
            string description = strArr[0].Replace("\n", "").Replace("\r", "");
            string copyright = strArr.Length > 1 ? strArr[2].Replace("\n", "").Replace("\r", "") : "";

            List<String> list = description.SplitAndKeep(new Char[] { '.', '?', '!' }).ToList<string>();
            List<String> translated = new List<String>();

            foreach (string s in list)
            {
                if (!String.IsNullOrEmpty(s.Trim()))
                {
                    string url = $"https://translate.googleapis.com/translate_a/single?client=gtx&sl={fromLang}&tl={toLang}&dt=t&q={HttpUtility.UrlEncode(s)}";
                    
                    var client = new HttpClient();
                    var responseAsync = await client.GetStringAsync(url);

                    string response = responseAsync;

                    response = response.Substring(4, response.IndexOf("\"", 4, StringComparison.Ordinal) - 4);
                    response = response.Replace("\\u200b​​​​​​​​​​​​​​​", "", StringComparison.InvariantCulture);
                    translated.Add(response + " ");
                }
            }
            string translation = "";
            foreach (string s in translated)
            {
                translation += s;
            }
            return translation + 
                (String.IsNullOrEmpty(copyright) ? "" : Environment.NewLine + Environment.NewLine + copyright);
        }

        /// <summary>
        /// Wallpaper utility class
        /// </summary>
        public sealed class Wallpaper
        {
            Wallpaper() { }

            const int SPI_SETDESKWALLPAPER = 20;
            const int SPIF_UPDATEINIFILE = 0x01;
            const int SPIF_SENDWININICHANGE = 0x02;

            [DllImport("user32.dll", CharSet = CharSet.Auto)]
            static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);

            /// <summary>
            /// Wallpaper fit options
            /// </summary>
            public enum Style : int
            {
                Fill,
                Fit,
                Span,
                Stretch,
                Tile,
                Center
            }

            /// <summary>
            /// Set online image as wallpaper
            /// </summary>
            /// <param name="uri"></param>
            /// <param name="style"></param>
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

            /// <summary>
            /// set <see cref="Image"/> to wallpaper
            /// </summary>
            /// <param name="img"><see cref="Image"/></param>
            /// <param name="style"><see cref="Style"/></param>
            public static void Set(Image img, Style style)
            {
                string tempPath = Path.Combine(Path.GetTempPath(), DateTime.Now.ToString("ddMMyyyy") + ".jpg");

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

        public static string GetUrlFromJson(string id)
        {
            string videoUrl = String.Format("http://vimeo.com/api/v2/video/{0}.json", id);
            string videoId = videoUrl.Split('/').Last();

            string apiUrl = $"http://vimeo.com/api/v2/video/{videoId}.json";

            using (WebClient client = new WebClient())
            {
                string json = client.DownloadString(apiUrl);
                using (JsonDocument document = JsonDocument.Parse(json))
                {
                    List<JsonElement> jsonArray = document.RootElement.EnumerateArray().ToList();
                    string thumbnailUrl = (string)jsonArray[0].GetProperty("thumbnail_large").GetString();
                    if (string.IsNullOrEmpty(thumbnailUrl))
                    {
                        throw new Exception("Cannot get vimeo thumbnail");
                    }
                    else
                    {
                        return thumbnailUrl;
                    }
                }
            }
        }

        public static class Images
        {
            public static async Task<Image> GetImageAsync(string imageUrl)
            {
                WebClient client = new WebClient();
                byte[] imageData = await client.DownloadDataTaskAsync(imageUrl);
                Image image = Image.FromStream(new System.IO.MemoryStream(imageData));

                // Visualizziamo le dimensioni dell'immagine
                return image;
            }


            public static Image FillImage(Image image, AppSettings settings)
            {
                // Create a new bitmap with the desired size and resolution
                int width = Screen.PrimaryScreen.Bounds.Width;
                int height = Screen.PrimaryScreen.Bounds.Height;
                Bitmap newImage = new Bitmap(width, height, image.PixelFormat);
                newImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

                // Create a graphics object to draw on the new bitmap
                Graphics graphics = Graphics.FromImage(newImage);

                //Take the placeholder
                Image placeholder = Image.FromFile(settings.FillerPath);
                //scale the image
                Image scaled = Images.ScaleImage(placeholder, Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
                // Blur the image
                if(settings.BlurLevel > 0)
                {
                    scaled = Blur((Bitmap)scaled, settings.BlurLevel);
                }
               
                // Create a texture brush based on the original image
                TextureBrush brush = new TextureBrush(scaled);

                // Define the rectangle that covers the entire new bitmap
                RectangleF rect = new RectangleF(0, 0, width, height);
                rect.Inflate(0, 1);

                // Fill the rectangle with the texture brush
                graphics.FillRectangle(brush, rect);

                // Calculate the position of the original image in the center of the new bitmap
                int x = (width - image.Width) / 2;
                int y = (height - image.Height) / 2;

                // Draw the original image in the center of the new bitmap
                graphics.DrawImage(image, x, y, image.Width, image.Height);

                // Create a new gradient brush with random colors based on the color of the original image
                Color dominant = GetDominantColor(image);
                Color maskColor = Color.FromArgb(settings.FillerTransparency, dominant.R, dominant.G, dominant.B);

                SolidBrush solidbrush = new SolidBrush(maskColor);

                // Fill the top part of the new bitmap with the gradient brush
                RectangleF topRect = new RectangleF(0, 0, width, y);
                topRect.Inflate(0, 0);
                graphics.FillRectangle(solidbrush, topRect);

                // Fill the bottom part of the new bitmap with the gradient brush
                RectangleF bottomRect = new RectangleF(0, y + image.Height, width, height - (y + image.Height));
                bottomRect.Inflate(0, 0);
                graphics.FillRectangle(solidbrush, bottomRect);

                // Fill the left part of the new bitmap with the gradient brush
                RectangleF leftRect = new RectangleF(0, y, x, image.Height);
                leftRect.Inflate(0, 0);
                graphics.FillRectangle(solidbrush, leftRect);

                // Fill the right part of the new bitmap with the gradient brush
                RectangleF rightRect = new RectangleF(x + image.Width, y, width - (x + image.Width), image.Height);
                rightRect.Inflate(0, 0);
                graphics.FillRectangle(solidbrush, rightRect);

                // Clean up resources
                solidbrush.Dispose();
                graphics.Dispose();
                brush.Dispose();
                image.Dispose();

                return newImage;
            }

            public static Image Save(Image image)
            {
                // Save the new image
                string tempPath = Path.Combine(Path.GetTempPath(), DateTime.Now.ToString("ddMMyyyy") + ".jpg");
                image.Save(tempPath);

                // Clean up resources
                image.Dispose();

                return image;
            }

            private static Bitmap Blur(Bitmap image, Int32 blurSize)
            {
                return Blur(image, new RectangleF(0, 0, image.Width, image.Height), blurSize);
            }

            private unsafe static Bitmap Blur(Bitmap image, RectangleF rectangle, Int32 blurSize)
            {
                Bitmap blurred = new Bitmap(image.Width, image.Height);

                // make an exact copy of the bitmap provided
                using (Graphics graphics = Graphics.FromImage(blurred))
                    graphics.DrawImage(image, new RectangleF(0, 0, image.Width, image.Height),
                        new RectangleF(0, 0, image.Width, image.Height), GraphicsUnit.Pixel);

                // Lock the bitmap's bits
                BitmapData blurredData = blurred.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadWrite, blurred.PixelFormat);

                // Get bits per pixel for current PixelFormat
                int bitsPerPixel = Image.GetPixelFormatSize(blurred.PixelFormat);

                // Get pointer to first line
                byte* scan0 = (byte*)blurredData.Scan0.ToPointer();

                // look at every pixel in the blur rectangle
                for (float xx = rectangle.X; xx < rectangle.X + rectangle.Width; xx++)
                {
                    for (float yy = rectangle.Y; yy < rectangle.Y + rectangle.Height; yy++)
                    {
                        int avgR = 0, avgG = 0, avgB = 0;
                        int blurPixelCount = 0;

                        // average the color of the red, green and blue for each pixel in the
                        // blur size while making sure you don't go outside the image bounds
                        for (float x = xx; (x < xx + blurSize && x < image.Width); x++)
                        {
                            for (float y = yy; (y < yy + blurSize && y < image.Height); y++)
                            {
                                // Get pointer to RGB
                                byte* data = scan0 + Convert.ToInt32(y) * blurredData.Stride + Convert.ToInt32(x) * bitsPerPixel / 8;

                                avgB += data[0]; // Blue
                                avgG += data[1]; // Green
                                avgR += data[2]; // Red

                                blurPixelCount++;
                            }
                        }

                        avgR = avgR / blurPixelCount;
                        avgG = avgG / blurPixelCount;
                        avgB = avgB / blurPixelCount;

                        // now that we know the average for the blur size, set each pixel to that color
                        for (float x = xx; x < xx + blurSize && x < image.Width && x < rectangle.Width; x++)
                        {
                            for (float y = yy; y < yy + blurSize && y < image.Height && y < rectangle.Height; y++)
                            {
                                // Get pointer to RGB
                                byte* data = scan0 + Convert.ToInt32(y) * blurredData.Stride + Convert.ToInt32(x) * bitsPerPixel / 8;

                                // Change values
                                data[0] = (byte)avgB;
                                data[1] = (byte)avgG;
                                data[2] = (byte)avgR;
                            }
                        }
                    }
                }

                // Unlock the bits
                blurred.UnlockBits(blurredData);

                return blurred;
            }

            public static Bitmap ScaleImage(Image bmp, int maxWidth, int maxHeight)
            {
                var ratioX = (double)maxWidth / bmp.Width;
                var ratioY = (double)maxHeight / bmp.Height;
                var ratio = Math.Min(ratioX, ratioY);

                var newWidth = (int)(bmp.Width * ratio);
                var newHeight = (int)(bmp.Height * ratio);

                var newImage = new Bitmap(newWidth, newHeight);

                using (var graphics = Graphics.FromImage(newImage))
                    graphics.DrawImage(bmp, 0, 0, newWidth, newHeight);

                return newImage;
            }

            public static Color GetDominantColor(Image imagePath)
            {
                Bitmap bmp = new Bitmap(imagePath);

                // Inizializza i conteggi per ogni componente colore (rosso, verde, blu)
                int r = 0, g = 0, b = 0;

                for (int x = 0; x < bmp.Width; x++)
                {
                    for (int y = 0; y < bmp.Height; y++)
                    {
                        Color pixelColor = bmp.GetPixel(x, y);

                        // Aggiunge il valore di ogni componente colore per il pixel corrente
                        r += pixelColor.R;
                        g += pixelColor.G;
                        b += pixelColor.B;
                    }
                }

                // Calcola la media di ogni componente colore
                int totalPixels = bmp.Width * bmp.Height;
                int avgR = r / totalPixels;
                int avgG = g / totalPixels;
                int avgB = b / totalPixels;

                // Restituisce il colore dominante come un oggetto Color
                return Color.FromArgb(avgR, avgG, avgB);
            }

            public static async Task<Image> GetVideoThumbnailAsync(string videoUrl)
            {
                string id = string.Empty;
                string url = "";

                // YOUTUBE
                Match youtubeMatch = new Regex(@"youtu(?:\.be|be\.com)/(?:.*v(?:/|=)|(?:.*/)?)([a-zA-Z0-9-_]+)").Match(videoUrl);
                if (youtubeMatch.Success)
                {
                    id = youtubeMatch.Groups[1].Value;
                    url = string.Format("http://i.ytimg.com/vi/{0}/maxresdefault.jpg", id);
                }

                // VIMEO
                Match vimeoMatch = new Regex(@"(?:https?:\/\/)?(?:www\.)?vimeo\.com\/(?:channels\/(?:\w+\/)?|groups\/([^\/]*)\/videos\/|album\/(\d+)\/video\/|)(\d+)(?:[?]?.*)").Match(videoUrl);
                if (vimeoMatch.Success)
                {
                    id = vimeoMatch.Groups[3].Value;
                    url = GetUrlFromJson(id);
                }

                // VIMEO 2
                Match vimeoMatch2 = new Regex(@"^//player\.vimeo\.com/video/(\d+)\b").Match(videoUrl);
                if (vimeoMatch2.Success)
                {
                    id = vimeoMatch2.Groups[1].Value;
                    url = GetUrlFromJson(id);
                }

                Image img = await GetImageAsync(url);

                return img;
            }

        }
    }
}
