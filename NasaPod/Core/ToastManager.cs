using Microsoft.Toolkit.Uwp.Notifications;
using Nasa.Model.Nasa;

namespace Nasa.Core
{
    internal static class ToastManager
    {
        public static void Pause()
        {
            new ToastContentBuilder()
                .AddText("Servizio in pausa", AdaptiveTextStyle.Title)
                .AddText("Non verranno più cercate immagini del giorno.", AdaptiveTextStyle.CaptionSubtle)
                .AddAppLogoOverride(new Uri("file:///" + Path.GetFullPath(@"Res\icon.ico")))
                .Show();
        }
        public static void Stop() 
        {
            new ToastContentBuilder()
                .AddText("Servizio interrotto")
                .AddText("Non verranno più cercate immagini del giorno.")
                .AddAppLogoOverride(new Uri("file:///" + Path.GetFullPath(@"Res\icon.ico")))
                .Show();
        }
        public static void Notify(APOD apod)
        {
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
        public static void Error(Exception ex)
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
