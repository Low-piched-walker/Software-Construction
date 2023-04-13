using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace SimpleCrawler
{
    public partial class Form1 : Form
    {
        private SimpleCrawler crawler = new SimpleCrawler();
        private Thread crawlerThread;
        private bool isCrawling = false;

        public Form1()
        {
            InitializeComponent();
            txtUrl.Text = "http://www.cnblogs.com/dstang2000/";
            crawler.OnPageDownloaded += Crawler_OnPageDownloaded;
            crawler.OnError += Crawler_OnError;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (!isCrawling)
            {
                lstUrls.Items.Clear();
                lstErrors.Items.Clear();
                crawlerThread = new Thread(crawler.Crawl);
                crawlerThread.Start(txtUrl.Text.Trim());
                isCrawling = true;
                btnStart.Text = "停止";
            }
            else
            {
                crawlerThread.Abort();
                isCrawling = false;
                btnStart.Text = "开始";
            }
        }

        private void Crawler_OnPageDownloaded(string url)
        {
            Action action = () => lstUrls.Items.Add(url);
            if (InvokeRequired) Invoke(action);
            else action();
        }

        private void Crawler_OnError(string url, string error)
        {
            Action action = () => lstErrors.Items.Add(url + " - " + error);
            if (InvokeRequired) Invoke(action);
            else action();
        }
    }

    class SimpleCrawler
    {
        public event Action<string> OnPageDownloaded;
        public event Action<string, string> OnError;

        private HashSet<string> visited = new HashSet<string>();
        private Queue<string> toCrawl = new Queue<string>();
        private string startUrl;
        private string host;

        public void Crawl(object startUrlObj)
        {
            startUrl = (string)startUrlObj;
            host = new Uri(startUrl).Host;
            toCrawl.Enqueue(startUrl);
            while (toCrawl.Count > 0)
            {
                string url = toCrawl.Dequeue();
                try
                {
                    string html = DownLoad(url);
                    OnPageDownloaded?.Invoke(url);
                    visited.Add(url);
                    Parse(html, url);
                }
                catch (Exception ex)
                {
                    OnError?.Invoke(url, ex.Message);
                }
            }
        }

        public string DownLoad(string url)
        {
            WebClient webClient = new WebClient();
            webClient.Encoding = Encoding.UTF8;
            string html = webClient.DownloadString(url);
            string fileName = visited.Count.ToString();
            File.WriteAllText(fileName, html, Encoding.UTF8);
            return html;
        }

        private void Parse(string html, string currentUrl)
        {
            string strRef = @"(href|HREF)\s*=\s*[""'][^""'#>]+[""']";
            MatchCollection matches = new Regex(strRef).Matches(html);
            foreach (Match match in matches)
            {
                strRef = match.Value.Substring(match.Value.IndexOf('=') + 1)
                          .Trim('"', '\'', '#', ' ', '>');
                if (strRef.Length == 0) continue;
                if (strRef.StartsWith("javascript:")) continue;
                if (strRef.StartsWith("javascript")) continue;
                string absoluteUrl = GetAbsoluteUrl(strRef, currentUrl);
                if (absoluteUrl != null)
                {
                    if (!urls.ContainsKey(absoluteUrl))
                    {
                        urls.Add(absoluteUrl, false);
                    }
                }
            }
        }
        private static string GetAbsoluteUrl(string url, string currentUrl)
        {
            if (url.StartsWith("http")) return url;
            if (url.StartsWith("//")) return "http:" + url;
            if (url.StartsWith("/"))
            {
                Uri uri = new Uri(currentUrl);
                return uri.Scheme + "://" + uri.Host + url;
            }
            if (url.StartsWith("../"))
            {
                int idx = currentUrl.LastIndexOf('/');
                string parentUrl = currentUrl.Substring(0, idx);
                return GetAbsoluteUrl(url.Substring(3), parentUrl);
            }
            if (url.StartsWith("./"))
            {
                return GetAbsoluteUrl(url.Substring(2), currentUrl);
            }
            int end = currentUrl.LastIndexOf("/");
            return currentUrl.Substring(0, end) + "/" + url;
        }

        private void Crawl()
        {
            while (urls.Count > 0 && count <= maxUrlCount)
            {
                string currentUrl = null;
                foreach (string url in urls.Keys)
                {
                    if ((bool)urls[url]) continue;
                    currentUrl = url;
                }

                if (currentUrl == null) continue;

                try
                {
                    string html = DownLoad(currentUrl); // 下载
                    urls[currentUrl] = true;
                    count++;
                    Parse(html, currentUrl);//解析,并加入新的链接
                    UpdateUI(currentUrl, true);
                }
                catch (Exception ex)
                {
                    UpdateUI(currentUrl, false, ex.Message);
                }
            }
        }

        private string DownLoad(string url)
        {
            try
            {
                WebClient webClient = new WebClient();
                webClient.Encoding = Encoding.UTF8;
                string html = webClient.DownloadString(url);
                string fileName = count.ToString();
                File.WriteAllText(fileName, html, Encoding.UTF8);
                return html;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void UpdateUI(string url, bool success, string errorMsg = null)
        {
            string result = url + (success ? "爬取成功" : "爬取失败");
            if (!string.IsNullOrEmpty(errorMsg)) result += "，错误信息：" + errorMsg;
            listBox1.Items.Add(result);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            urls.Clear();
            count = 0;
            maxUrlCount = (int)numericUpDown1.Value;
            listBox1.Items.Clear();

            string startUrl = textBox1.Text.Trim();
            if (string.IsNullOrEmpty(startUrl))
            {
                MessageBox.Show("请输入起始URL");
                return;
            }

            urls.Add(startUrl, false);//加入初始页面
            new Thread(Crawl).Start();
        }
    }
}
