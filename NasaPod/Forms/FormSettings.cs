using Microsoft.Extensions.Configuration;
using Nasa.Core;
using System.Dynamic;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Converters;
using System.Windows.Forms;
using System.Xml;

namespace Nasa
{
    public partial class FormSettings : Form
    {
        IConfigurationRoot _config;
        AppSettings _appSettings;
        private Dictionary<string, string> languageDictionary;

        public FormSettings(IConfigurationRoot config)
        {
            _config = config;
            AppSettings settings = config.GetSection("AppSettings").Get<AppSettings>();
            _appSettings = settings;

            InitializeComponent();

            numericBlurLevel.Value = settings.BlurLevel;
            textBoxApiKey.Text = settings.ApiKey;
            textBoxEndpoint.Text = settings.Endpoint;
            numericHoursInterval.Value = settings.HoursInterval;

            textBoxFillerImage.Text = settings.FillerPath;
            numericRatio.Value = settings.Ratio;
            numericScaleThresholdHeight.Value = settings.ScaleThresholdHeight;
            numericScaleThresholdWidth.Value = settings.ScaleThresholdWidth;
            numericFillerTransparency.Value = settings.FillerTransparency;

            this.FormBorderStyle = FormBorderStyle.FixedSingle;

            // Chiama il tuo metodo per ottenere il dizionario delle lingue
            Dictionary<string, string> languageDictionary = Languages.AllAsDictionary();

            // Assegna il dizionario come origine dati per la ListBox
            listBoxLanguages.DataSource = new BindingSource(languageDictionary, null);

            // Imposta le proprietà DisplayMember e ValueMember
            listBoxLanguages.DisplayMember = "Value";
            listBoxLanguages.ValueMember = "Key";

            listBoxLanguages.SelectedValue = settings.Lang;

            checkBoxAutostart.Checked = ExistInAutostart();

            TranslateLabels(settings);
        }

        private bool ExistInAutostart()
        {
            // Ottieni la cartella di avvio
            string cartellaAvvio = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Startup));

            // Ottieni il percorso dell'eseguibile dell'applicazione
            string percorsoEseguibile = Application.ExecutablePath;

            // Crea un collegamento nella cartella di avvio
            string percorsoCollegamento = Path.Combine(cartellaAvvio, "Nasa.lnk");

            // Copia il collegamento solo se non esiste già
            return File.Exists(percorsoCollegamento);
        }

        private void ManageAutostart(bool autostart)
        {
            try
            {
                if (autostart)
                {   
                    // Copia il collegamento solo se non esiste già
                    if (!ExistInAutostart())
                    {
                        IWshRuntimeLibrary.WshShell shell = new IWshRuntimeLibrary.WshShell();
                        IWshRuntimeLibrary.IWshShortcut shortcut = (IWshRuntimeLibrary.IWshShortcut)shell.CreateShortcut(Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Startup)), "Nasa.lnk"));
                        shortcut.TargetPath = Application.ExecutablePath;
                        shortcut.Save();
                    }
                }
                else
                {
                    File.Delete(Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Startup)), "Nasa.lnk"));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Errore durante l'aggiunta del collegamento all'avvio: " + ex.Message, "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private async void TranslateLabels(AppSettings settings)
        {
            ///BlurLevel
            string labelBlurLevelDesc = "If you want, you can specify a level of blur effect to apply to filler images for those APODs that do not have the correct size to become a nice wallpaper.";
            labelBlurLevelDesc = await Utility.Translate(labelBlurLevelDesc, "en", settings.Lang);
            helperTooltip.SetToolTip(labelBlurLevel, labelBlurLevelDesc);

            ///ApiKey
            string labelApiKeyDesc = "You can leave DEMO_KEY. This default key has a limit of 30 hourly requests per IP address and 50 daily requests per day. This program only makes 24 calls each day, but you can set a higher value in the HoursInterval to make it do less.";
            labelApiKeyDesc = await Utility.Translate(labelApiKeyDesc, "en", settings.Lang);
            helperTooltip.SetToolTip(labelApiKey, labelApiKeyDesc);

            ///Endpoint
            string labelEndpointDesc = "This is the endpoint of the NASA REST API.";
            labelEndpointDesc = await Utility.Translate(labelEndpointDesc, "en", settings.Lang);
            helperTooltip.SetToolTip(labelEndpoint, labelEndpointDesc);

            ///HoursInterval
            string labelHoursIntervalDesc = "This is the interval of time between each request.";
            labelHoursIntervalDesc = await Utility.Translate(labelHoursIntervalDesc, "en", settings.Lang);
            helperTooltip.SetToolTip(labelHoursInterval, labelHoursIntervalDesc);

            ///Lang
            string labelLangDesc = "This is the ISO 639-1 code of the language to translate into (reference: https://en.wikipedia.org/wiki/List_of_ISO_639-1_codes).";
            labelLangDesc = await Utility.Translate(labelLangDesc, "en", settings.Lang);
            helperTooltip.SetToolTip(labelLanguage, labelLangDesc);

            ///FillerPath
            string labelFillerPathDesc = "If you want, you can change the filler image for those APODs that do not have the correct size to become a nice wallpaper.";
            labelFillerPathDesc = await Utility.Translate(labelFillerPathDesc, "en", settings.Lang);
            helperTooltip.SetToolTip(labelFillerPath, labelFillerPathDesc);

            ///Ratio
            string labelRatioDesc = "This is the difference between the height and width of the image. If this result is lower or equal to the value, the image is set as wallpaper in FILL mode.";
            labelRatioDesc = await Utility.Translate(labelRatioDesc, "en", settings.Lang);
            helperTooltip.SetToolTip(labelRatio, labelRatioDesc);

            ///ScaleThresholdHeight
            string labelScaleThresholdHeightDesc = "This is the difference between your screen's height and the height of the image. If this result is lower or equal to the value, the image is not scaled.";
            labelScaleThresholdHeightDesc = await Utility.Translate(labelScaleThresholdHeightDesc, "en", settings.Lang);
            helperTooltip.SetToolTip(labelScaleThresholdHeight, labelScaleThresholdHeightDesc);

            ///ScaleThresholdWidth
            string labelScaleThresholdWidthDesc = "This is the difference between your screen's width and the width of the image. If this result is lower or equal to the value, the image is not scaled.";
            labelScaleThresholdWidthDesc = await Utility.Translate(labelScaleThresholdWidthDesc, "en", settings.Lang);
            helperTooltip.SetToolTip(labelScaleThresholdWidth, labelScaleThresholdWidthDesc);

            ///FillerTransparency
            string labelFillerTransparencyDesc = "This is the transparency effect (alpha channel) of a solid color mask applied to the image.";
            labelFillerTransparencyDesc = await Utility.Translate(labelFillerTransparencyDesc, "en", settings.Lang);
            helperTooltip.SetToolTip(labelFillerTransparency, labelFillerTransparencyDesc);
        }

        private async void buttonSaveSettings_Click(object sender, EventArgs e)
        {
            // Percorso del file appsettings.json
            string appSettingsPath = "appsettings.json";

            // Leggi il valore dei controlli
            string apiKey = textBoxApiKey.Text;
            string endpoint = textBoxEndpoint.Text;
            string hoursInterval = numericHoursInterval.Text;
            string lang = listBoxLanguages.SelectedValue.ToString();
            string blurLevel = numericBlurLevel.Text;
            string fillerPath = textBoxFillerImage.Text;
            string ratio = numericRatio.Text;
            string scaleThresholdH = numericScaleThresholdHeight.Text;
            string scaleThresholdW = numericScaleThresholdWidth.Text;
            string fillerTransparency = numericFillerTransparency.Text;

            ManageAutostart(checkBoxAutostart.Checked);

            // Modifica il parametro specifico in appsettings.json
            _config["ApiKey"] = apiKey;
            _config["Endpoint"] = endpoint;
            _config["HoursInterval"] = hoursInterval;
            _config["Lang"] = lang;
            _config["BlurLevel"] = blurLevel;
            _config["FillerPath"] = fillerPath;
            _config["Ratio"] = ratio;
            _config["ScaleThresholdHeight"] = scaleThresholdH;
            _config["ScaleThresholdWidth"] = scaleThresholdW;
            _config["FillerTransparency"] = fillerTransparency;

            AppSettings appSettings = new AppSettings()
            {
                ApiKey = apiKey,
                Endpoint = endpoint,
                HoursInterval = Convert.ToInt32(hoursInterval),
                Lang = lang,
                BlurLevel = Convert.ToInt32(blurLevel),
                FillerPath = fillerPath,
                Ratio = Convert.ToInt32(ratio),
                ScaleThresholdHeight = Convert.ToInt32(scaleThresholdH),
                ScaleThresholdWidth = Convert.ToInt32(scaleThresholdW),
                FillerTransparency = Convert.ToInt32(fillerTransparency)
            };
            JsonConfig config = new JsonConfig();
            config.AppSettings = appSettings;

            // Serializza la configurazione in formato JSON
            string jsonConfig = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });

            // Salva le modifiche nel file appsettings.json
            File.WriteAllText(appSettingsPath, jsonConfig);

            string savedMessage = await Utility.Translate("Changes Saved. Will take effect upon restarting the application.", "en", _appSettings.Lang);
            MessageBox.Show(savedMessage);
        }
    }
}
