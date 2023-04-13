using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace ImprovedCrawler
{
    class ImprovedCrawler
    {
        private HashSet<string> visited = new HashSet<string>();  // 已访问的链接
        private Queue<string> unvisited = new Queue<string>();  // 待访问的链接
        private string domain;  // 爬取的域名
        private int count = 0;  // 记录已访问的页面数量

        static void Main(string[] args)
        {
            ImprovedCrawler myCrawler = new ImprovedCrawler();
            string startUrl = "http://www.cnblogs.com/dstang2000/";
            if (args.Length >= 1) startUrl = args[0];
            myCrawler.domain = GetDomain(startUrl);
            myCrawler.unvisited.Enqueue(startUrl);
            new Thread(myCrawler.Crawl).Start();
        }

        private void Crawl()
        {
            Console.WriteLine("开始爬行了.... ");
            while (unvisited.Count > 0 && count < 10)
            {
                string current = unvisited.Dequeue();
                Console.WriteLine("爬行" + current + "页面!");
                string html = DownLoad(current);  // 下载
                if (html == null) continue;
                visited.Add(current);
                count++;
                if (IsHtmlFile(current))
                {
                    Parse(html, current);  // 解析,并加入新的链接
                }
                Console.WriteLine("爬行结束");
            }
        }

        public string DownLoad(string url)
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
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        private void Parse(string html, string current)
        {
            string strRef = @"(href|HREF)\s*=\s*[""'](?<url>[^""'#>]+)[""']";
            MatchCollection matches = new Regex(strRef).Matches(html);
            foreach (Match match in matches)
            {
                strRef = match.Groups["url"].Value.Trim();
                if (strRef.Length == 0) continue;
                string absoluteUrl = GetAbsoluteUrl(strRef, current);
                if (absoluteUrl == null || visited.Contains(absoluteUrl)) continue;
                if (IsSameDomain(absoluteUrl, domain))
                {
                    unvisited.Enqueue(absoluteUrl);
                }
            }
        }

        private static string GetDomain(string url)
        {
            Uri uri = new Uri(url);
            return uri.Scheme + "://" + uri.Host;
        }

        private static bool IsSameDomain(string url1, string url2)
        {
            return GetDomain(url1) == GetDomain(url2);
        }

        private static bool IsHtmlFile(string url)
        {
            string extension = Path.GetExtension(url).ToLower();
            return extension == ".htm" || extension == ".html" || extension == ".aspx"
                || extension == ".php" || extension == ".jsp";
        }

        private static string GetAbsoluteUrl(string url, string current)
        {
            if (url.StartsWith("http")) return url;
            if (url.StartsWith("//")) return "http:" + url;
            if (url.StartsWith("/"))
            {
                Uri baseUri = new Uri(current);
                return new Uri(baseUri, url).AbsoluteUri;
            }
            int lastSlashIndex = current.LastIndexOf("/");
            string parentPath = current.Substring(0, lastSlashIndex + 1);

            while (url.StartsWith("../"))
            {
                url = url.Substring(3);
                int prevLastSlashIndex = parentPath.TrimEnd('/').LastIndexOf('/');
                parentPath = parentPath.Substring(0, prevLastSlashIndex + 1);
            }

            return parentPath + url;
        }
    }
}
