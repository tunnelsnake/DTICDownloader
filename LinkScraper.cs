using System;
using System.Collections.Generic;
using HtmlAgilityPack;

public class LinkScraper
    {
    String baseurl = @"http://www.dtic.mil/dtic/tr/fulltext/";
    String url;
    String outputpath;
    bool writefile;
    bool appendlink;

	public LinkScraper(string catalogpath)
    { 
        this.outputpath = catalogpath;
        this.writefile = true;
        this.appendlink = false;
	}
    public void BuildCatalog()
    {
        this.appendlink = false;
        this.url = this.baseurl;
        List<String> LinkList = Scrape();
        appendlink = true;
        foreach(string s in LinkList)
        {
            this.url = this.baseurl + s;
            System.Console.WriteLine("Debug");
            this.Scrape();
        }
    }

    public List<String> Scrape()
    {
        List<String> links = new List<String>();

        HtmlWeb hw = new HtmlWeb();
        HtmlDocument doc = hw.Load(url);
        foreach (HtmlNode link in doc.DocumentNode.SelectNodes("//a[@href]"))
        {
            if (appendlink)
            {
                links.Add((@url + link.GetAttributeValue("href", string.Empty)));
            }
            else
            {
                links.Add(link.GetAttributeValue("href", string.Empty));
            }
            System.Console.WriteLine("Found Link: " + links[links.Count - 1]);
        }

        if (writefile)
        {
            System.Console.WriteLine("Dumping to text file: " + outputpath);
            System.IO.File.Delete(outputpath);
            System.IO.File.WriteAllLines(outputpath, links);
        }

        return links;
    }
}
