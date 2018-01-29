using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VkPlayer
{
    public partial class FormAuthorization : Form
    {
        public FormAuthorization()
        {
            InitializeComponent();
        }

        private void FormAuthorization_Load(object sender, EventArgs e)
        {
            webBrowser1.Navigate("https://oauth.vk.com/authorize?client_id=4962552&display=page&redirect_uri=https://oauth.vk.com/blank.html&scope=audio&response_type=token&v=5.71");
        }

        private void webBrowser1_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            zagruzkaLabel.Text = "Загрузка";
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            zagruzkaLabel.Text = "Загружено";
            try
            {
                string url = webBrowser1.Url.ToString();
                string tokenMas = url.Split('#')[1];
                if (tokenMas[0] == 'a')
                {
                    Settings1.Default.token = tokenMas.Split('&')[0].Split('=')[1];
                    Settings1.Default.id = tokenMas.Split('=')[3];
                    Settings1.Default.auth = true;
                    MessageBox.Show("Token=" + Settings1.Default.token + "id=" + Settings1.Default.id, "Auth");
                    //this.Close();
                }
            }
            catch
            {
                MessageBox.Show("Чет пошло не так...", "Хм...");
            }
        }
    }
}
