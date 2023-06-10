using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Nasa.Core
{
    public static class VersionChecker
    {
        private const string GitHubApiUrl = "https://api.github.com/repos/{owner}/{repo}/releases/latest";

        public static async Task<Version> GetLatestVersion(string owner, string repo)
        {
            string apiUrl = GitHubApiUrl.Replace("{owner}", owner).Replace("{repo}", repo);

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0)");

                HttpResponseMessage response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    JsonDocument doc = JsonDocument.Parse(json);

                    if (doc.RootElement.TryGetProperty("tag_name", out JsonElement tagElement))
                    {
                        string versionString = tagElement.GetString();
                        if (versionString.StartsWith("v"))
                        {
                            versionString = versionString.Substring(1);
                        }

                        if (Version.TryParse(versionString, out Version version))
                        {
                            return version;
                        }
                    }
                }
            }
            return new Version();
        }

        public static void CheckProjectVersion(string owner, string repo, Version currentVersion)
        {
            Task<Version> getVersionTask = GetLatestVersion(owner, repo);
            getVersionTask.Wait();
            Version latestVersion = getVersionTask.Result;

            if (latestVersion != null)
            {
                if (latestVersion > currentVersion)
                {
                    DialogResult result = MessageBox.Show($"A most recent version of the program is available ({latestVersion})." +
                                                          $"\n" +
                                                          $"\nYou want update?",
                                                          "Update available",
                                                          MessageBoxButtons.YesNo,
                                                          MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        // Chiamata al metodo di aggiornamento dell'applicazione
                        DownloadLatestReleaseFromGitHub(owner, repo);
                    }
                }
            }
            else
            {
                MessageBox.Show("Unable to retrieve the latest version of the project.");
            }
        }

        public static Version GetApplicationVersion()
        {
            // Recupera l'oggetto di assembly per l'assembly corrente
            var assembly = System.Reflection.Assembly.GetEntryAssembly();

            // Recupera l'attributo di versione dell'assembly
            var versionAttribute = assembly?.GetCustomAttributes(typeof(System.Reflection.AssemblyFileVersionAttribute), false)
                as System.Reflection.AssemblyFileVersionAttribute[];

            // Se l'attributo di versione esiste e la versione è valida, restituisce la versione; altrimenti, restituisce null
            if (versionAttribute?.Length > 0 && Version.TryParse(versionAttribute[0].Version, out Version version))
            {
                return version;
            }

            return null;
        }

        public static async Task DownloadLatestReleaseFromGitHub(string owner, string repo)
        {
            string apiUrl = $"https://api.github.com/repos/{owner}/{repo}/releases/latest";
            string downloadUrl = null;

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0)");

                HttpResponseMessage response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    JsonDocument doc = JsonDocument.Parse(json);

                    if (doc.RootElement.TryGetProperty("assets", out JsonElement assetsElement))
                    {
                        if (assetsElement.GetArrayLength() > 0)
                        {
                            JsonElement assetElement = assetsElement[0];
                            if (assetElement.TryGetProperty("browser_download_url", out JsonElement downloadUrlElement))
                            {
                                downloadUrl = downloadUrlElement.GetString();
                            }
                        }
                    }
                }
            }
            try
            {
                if (!string.IsNullOrEmpty(downloadUrl))
                {
                    // Apri il browser e avvia il download
                    ProcessStartInfo startInfo = new ProcessStartInfo
                    {
                        FileName = downloadUrl,
                        UseShellExecute = true
                    };
                    Process.Start(startInfo);
                }
            } 
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
