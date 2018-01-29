using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Web;
using Newtonsoft.Json.Linq;

namespace VkPlayer
{    
    public partial class FormMain : Form
    {        
        public List<Audio> audioList;
        public FormMain()
        {
            InitializeComponent();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            new FormAuthorization().Show();
            backgroundWorker1.RunWorkerAsync();
        }

        

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            audioList = new List<Audio>
            {
                new Audio(){ aid=0, duration=0, artist="noname", genre=0, lurics_id="noname", owner_id=0, title="noname", url="none" }
            }; //надо инициализировать список в хроме вкладка открыта как это сделать
            while (!Settings1.Default.auth)
            {
                Thread.Sleep(50);
            }
            WebRequest request = WebRequest.Create("https://api.vk.com/method/audio.get?owner_id=" + Settings1.Default.id+"&need_user=0&access_token="+Settings1.Default.token);
            WebResponse response = request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responceFromServer = reader.ReadToEnd();
            reader.Close();
            response.Close();
            responceFromServer = HttpUtility.HtmlDecode(responceFromServer);

            JToken token = JToken.Parse(responceFromServer);
            audioList = token["resonce"].Children().Skip(1).Select(c => c.ToObject<Audio>()).ToList();

            this.Invoke((MethodInvoker)delegate
            {
                for (int i = 0; i < audioList.Count(); i++)
                {
                    listBox1.Items.Add(audioList[i].artist + " - " + audioList[i].title);
                }
            });
        }
    }
}
