using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace opi_lab2.web
{
    class MoodleScrapper : ILinkScrapper
    {
        public string Url { get; set; }

        public MainWindow MainWindow
        {
            get => default;
            set
            {
            }
        }

        public MoodleScrapper(string url)
        {
            Url = url;
        }

        public List<MoodleLink> GetNews(int amount)
        {
            string sourceHtml = GetHTML(Url);
            var links = ScrapePage(sourceHtml, amount);
            Console.WriteLine($"Total link count: {links.Count}");
            return links;
        }

        private List<MoodleLink> ScrapePage(string html, int amount)
        {
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(html);

            List<MoodleLink> list = new List<MoodleLink>();
            var newsLinks = document.DocumentNode.SelectNodes("//div[contains(@class, \"a-container\")]");

            int nextAmount = amount - newsLinks.Count();
            int current = 0;
            while (current < amount && current != amount - nextAmount)
            {
                // decode &nbsp &laquo &raquo ...
                string header = WebUtility.HtmlDecode(newsLinks[current].ChildNodes[1].InnerText);
                string annotation = WebUtility.HtmlDecode(newsLinks[current].ChildNodes[3].InnerText);

                list.Add(new MoodleLink(
                    "https:" + newsLinks[current].ChildNodes[1].ChildNodes[0].Attributes[0].Value,
                    header,
                    annotation
                    ));
                current++;
            }
            if (nextAmount > 0)
            {
                string nextPageUrl = GetNextPageLink(html);
                string nextPageHtml = GetHTML(nextPageUrl);
                list.AddRange(ScrapePage(nextPageHtml, nextAmount));
            }
            return list;
        }

        private string GetNextPageLink(string html)
        {
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(html);

            var pageLinks = document.DocumentNode.SelectNodes("//div[contains(@class, \"znu12-pagination\")]/a[contains(@style, \"font-size\")]/following-sibling::a");

            return pageLinks == null ? "" : "https:" + pageLinks[0].Attributes[0].Value;
        }

        private string GetHTML(string url)
        {
            WebRequest request = WebRequest.Create(url);
            WebResponse response = request.GetResponse();

            StringBuilder sourceHtml = new StringBuilder();
            using (StreamReader stream = new StreamReader(response.GetResponseStream()))
            {
                string line;
                while ((line = stream.ReadLine()) != null)
                    sourceHtml.Append(line).Append('\n');
            }

            return sourceHtml.ToString();
        }
    }
}