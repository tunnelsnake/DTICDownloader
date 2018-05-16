using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace DTICDownloader
{
    class DescriptionScraper
    {
        public static string Baseurl = @"http://www.dtic.mil/docs/citations/AD";
        public string url;
        public string identifier;
        private bool retry;

        public DescriptionScraper(string fileidentifier, bool retry)
        {
            url = Baseurl + fileidentifier;
            identifier = fileidentifier;
            this.retry = retry;
        }

        public Entry Scrape()
        {
            HtmlWeb hw = new HtmlWeb();
            HtmlDocument doc = hw.Load(url);

            string[] substrings;
            string title = "";
            string abstr = "";
            string[] tags = null;

            if (doc == null) return new Entry { Identifier = "ERROR" };

            try
            {

                foreach (HtmlNode node in doc.DocumentNode.SelectNodes("//p"))
                {
                    substrings = node.InnerText.ToLower().Split(':');
                    substrings[0] = substrings[0].Substring(0, substrings[0].Length - 1);

                    if (substrings[0].Contains("title"))
                    {
                        title = substrings[1].Trim().Replace("&nbsp;", "").Replace("*", "");
                    }

                    if (substrings[0].Contains("abstract"))
                    {
                        abstr = substrings[1].Trim().Replace("&nbsp;", "").Replace("*", "");
                    }

                    if (substrings[0].Contains("descriptors"))
                    {
                        substrings[1] = substrings[1].Trim().ToLower().Replace("&nbsp;", "").Replace("*", "");
                        tags = substrings[1].Split(',');

                        for (int i = 0; i < substrings.Length; i++)
                        {
                            substrings[i] = substrings[i].Trim();
                        }
                    }
                }

                if (title != null && abstr != null && tags != null)
                {
                    return new Entry { Identifier = identifier, Title = title, Abstr = abstr, Tags = tags };
                }
                else
                {
                    return new Entry { Identifier = "ERROR" };
                }
            } 
            catch (Exception e)
            {
                if (!retry)
                {
                    DescriptionScraper retry = new DescriptionScraper(identifier, true);
                    return retry.Scrape();
                }
                else
                {
                    return new Entry { Identifier = "ERROR" };
                }
            }

        }
    }
}
