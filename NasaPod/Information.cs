using Nasa.Core;
using Nasa.Model.Nasa;
using System.Diagnostics;
using System.Text.Json;
using static Nasa.Core.Utility;

namespace Nasa
{
    public partial class Information : Form
    {
        Globals _globals = new Globals();
        public Information(string title, string description, Globals env)
        {
            _globals = env;
            InitializeComponent(title, description);
        }

        private void More_Click(object sender, EventArgs e)
        {
            Process browser = new Process();
            try
            {
                browser.StartInfo.UseShellExecute = true;
                browser.StartInfo.FileName = "https://apod.nasa.gov/apod/astropix.html";
                browser.StartInfo.CreateNoWindow = true;
                browser.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void TranslateLANG_Click(object sender, EventArgs e)
        {
            string translated = Utility.Translate(Description.Text, "en", _globals.settings.Lang);
            Description.Text = translated;
            TranslateLANG.Visible = false;
            TranslateEN.Visible = true;
        }
        private void TranslateENG_Click(object sender, EventArgs e)
        {
            string oldJsonAPOD = File.ReadAllText(Globals.storageFileName);
            APOD oldJsonObject = JsonSerializer.Deserialize<APOD>(oldJsonAPOD);

            Description.Text = oldJsonObject.explanation;
            TranslateLANG.Visible = true;
            TranslateEN.Visible = false;
        }
    }
}
