using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DTICDownloader
{
    class Downloader
    {
        string catalogpath;
        string destinationpath;
        string panicpath;

        int numthreads; //number of downloader threads

        public Downloader(string directorypath, int numthreads)
        {
            catalogpath += @"\catalog.txt";
            destinationpath += @"\Documents\";
            panicpath += @"\panic.txt";

            this.numthreads = numthreads;
        }

        public void GetCatalogStatistics()
        {
            String[] catalog = System.IO.File.ReadAllLines(catalogpath);
            System.Console.WriteLine("Catalog Contains " + catalog.Length + " Entries.");
            System.Console.WriteLine("Press a Key to Continue.");
            System.Console.ReadKey();
        }

        public void DownloadFiles()
        {
            List<string> lines = new List<string>();
            string[] fileContent = System.IO.File.ReadAllLines(catalogpath);
            lines.AddRange(fileContent);
            Parallel.ForEach(lines, new ParallelOptions{MaxDegreeOfParallelism = numthreads}, DownloadFile);

        }

        public void ResumeDownload(string filename) //i.e. xxxxxxx
        {
            List<string> lines = new List<string>();
            string[] fileContent = System.IO.File.ReadAllLines(catalogpath);
            foreach (string y in fileContent)
            {
                lines.Add(y.Substring(40));
            }
            int total_length = lines.Count;
            for (int i = 0; i < lines.Count; i++)
            {
                if (lines[i].Replace(".pdf", "") == filename)
                {
                    lines.RemoveRange(0, lines.Count);
                    lines.AddRange(fileContent.ToList().GetRange(i, total_length - i));
                }
            }

            Parallel.ForEach(lines, new ParallelOptions { MaxDegreeOfParallelism = numthreads }, DownloadFile);
        }

        private void DownloadFile(String url)
        {
            WebClient wc = new WebClient();
            String filename;

            filename = url.Substring(40);
            try
            {
                Console.WriteLine("Downloading File " + filename + ".");
                wc.DownloadFile(new System.Uri(url),
                destinationpath + filename);
                Console.WriteLine("Download of File " + filename + " Complete.");

            }
            catch (Exception e)
            {
                System.Console.WriteLine("Download of " + filename + " Failed. Trying Again.");
                DownloadFile(url);
            }

        }

        public static List<T>[] Splitlist<T>(List<T> list, int totalPartitions)
        {
            if (list == null)
                throw new ArgumentNullException("list");

            if (totalPartitions < 1)
                throw new ArgumentOutOfRangeException("totalPartitions");

            List<T>[] partitions = new List<T>[totalPartitions];

            int maxSize = (int)Math.Ceiling(list.Count / (double)totalPartitions);
            int k = 0;

            for (int i = 0; i < partitions.Length; i++)
            {
                partitions[i] = new List<T>();
                for (int j = k; j < k + maxSize; j++)
                {
                    if (j >= list.Count)
                        break;
                    partitions[i].Add(list[j]);
                }
                k += maxSize;
            }

            return partitions;
        }

        public void DumpLinks()
        {
            LinkScraper ls = new LinkScraper(catalogpath);
            ls.BuildCatalog();
        }


    }
}
