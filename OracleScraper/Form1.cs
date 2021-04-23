using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsInput.Native;
using WindowsInput;
using PuppeteerSharp;
using System.IO;

namespace OracleScraper
{
    public partial class Form1 : Form
    {
        private string dateString = "";
        private List<PathFile> filePath = new List<PathFile>();

        public Form1()
        {
            InitializeComponent();
            monthCalendar1.DateChanged += monthCalendar1_DateChanged;

        }

        async private void button1_Click(object sender, EventArgs e)
        {
            string content = "";
            if (filePath.Count > 0)
            {
                textBox1.Text += "Программа начала свою работу! Подождите немного" + Environment.NewLine;
                content = await GetInfoFromWeb(dateString, filePath, textBox2);
            }

            MessageBox.Show(content);
        }

        public static async Task<string> GetInfoFromWeb(string dateStr, List<PathFile> Paths, TextBox textBox)
        {

            await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultRevision);

            var option = new LaunchOptions
            {
                Headless = false, ///
                Timeout = 50000
            };

            using (var browser = await Puppeteer.LaunchAsync(option))
            {
                for (int i = 0; i < Paths.Count; i++)
                {
                    using (var page = await browser.NewPageAsync() )
                    {

                        await page.GoToAsync("http://msk-eb-int.ad.ies-holding.com:9000/cm-ekt/login");

                        await page.WaitForTimeoutAsync(5000);

                        var messegeFromMetod = await DoList.Action(page, Paths[i], dateStr);

                        textBox.Text += messegeFromMetod + Environment.NewLine;
                    }
                }
                return "!!!!!!!Finish!!!!!!!!!!";
            }

        }


        private void monthCalendar1_DateChanged(object sender, DateRangeEventArgs e)
        {
            dateString = e.Start.ToString("MM.yyyy");
            label1.Text = dateString;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dateString = monthCalendar1.TodayDate.ToString("MM.yyyy");
            label1.Text = dateString;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            filePath.Clear();
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = true;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = "";
                foreach (var f in ofd.FileNames)
                {
                    textBox1.Text += new FileInfo(f).FullName + "; \n " + Environment.NewLine;
                    var path = new FileInfo(f).FullName;
                    string settingUpload = GetSetting(path);
                    var doc = new PathFile
                    {
                        Path = new FileInfo(f).FullName,
                        SettingUpload = settingUpload
                    };
                    filePath.Add(doc);
                }
            }
            textBox1.Text = textBox1.Text.TrimEnd(';');
        }

        private string GetSetting(string path)
        {
            string searchResult = path.Substring(path.LastIndexOf('\\') + 1);
            var array = PathFile.GetList();
            var answer = array
                .Where(x => searchResult.ToLower().Contains(x.Path.ToLower()))
                .Select(x => x.SettingUpload)
                .OrderBy(x => x).ToList();

            return answer[0];
        }
    }
}
