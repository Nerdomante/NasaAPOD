using Nasa.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.DataFormats;

namespace Nasa
{
    public partial class Information : Form
    {
        public Information(string title, string description)
        {
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

        private void Translate_Click(object sender, EventArgs e)
        {
            string translated = Utility.Translate(Description.Text);
            Description.Text = translated;
        }
    }
}
