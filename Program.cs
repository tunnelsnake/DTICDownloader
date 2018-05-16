using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using LiteDB;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.IO;
using System.Threading.Tasks;

namespace DTICDownloader
{
    class DTICDownloader
    {
        string dbpath = @"D:\Database.db";
        string catalogpath = @"D:\catalog.txt";
        int numthreads;
        public Database db;

        public DTICDownloader(int numthreads)
        {
            this.numthreads = numthreads;
            db = new Database(dbpath);
        }
        static void Main(string[] args)
        {
            DTICConsole c = new DTICConsole();
        }

        public void BuildDatabase()
        {
            System.Console.WriteLine("Building Database");
            List<string> lines = new List<string>();
            string[] fileContent = System.IO.File.ReadAllLines(catalogpath);
            lines.AddRange(fileContent);
            if (db.collection.Count() == lines.Count) return;
            for (int i = db.collection.Count()-1; i < lines.Count; i++) //resume from the last entry in the database
            {
                lines[i] = lines[i].Substring(40).Replace(".pdf", "").ToUpper();
            }

            Parallel.ForEach(lines, new ParallelOptions { MaxDegreeOfParallelism = numthreads }, CreateDatabaseEntry);
        }

        public void CreateDatabaseEntry(string fileidentifier)
        {
            DescriptionScraper ds = new DescriptionScraper(fileidentifier, false);
            Entry e = ds.Scrape();
            if (e.Identifier != "ERROR")
            {
                System.Console.WriteLine("Entry Added For " + e.Identifier);
                this.db.AddEntryObject(e);
            } else
            {
                System.Console.WriteLine("An Error Occurred.");
            }
        }
    }
}