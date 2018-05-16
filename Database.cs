using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteDB;

namespace DTICDownloader
{
    public class Entry
    {
        public int Id { get; set; }
        public string Identifier { get; set; }
        public string Title { get; set; }
        public string Abstr { get; set; }
        public string[] Tags { get; set; }
    }

    class Database
    {
        public LiteDatabase db;
        public LiteCollection<Entry> collection;

        public Database(string dbpath)
        {
            db = new LiteDatabase(dbpath);
            collection = db.GetCollection<Entry>("entries");
        }

        public void AddEntry(string identifier, string title, string abstr, string[] tags)
        {
            collection.Insert(new Entry { Identifier = identifier, Title = title, Abstr = abstr, Tags = tags });
        }

        public void AddEntryObject(Entry e)
        {
            collection.Insert(e);
        }

        public void CheckIntegrity()
        {
            collection.EnsureIndex(x => x.Tags, "$.Tags[*]");
            var result = collection.FindAll().Where(x => x.Tags.Any(z => z.Equals(null)));
            System.Console.WriteLine("Database Contains " + collection.Count() + " Entries.");
            foreach (Entry e in result)
            {
                System.Console.WriteLine("Bad Entry With Identifier: " + e.Identifier + " Found. Deleted");
                collection.Delete(e.Id);
            }
            System.Console.WriteLine("Database Integrity Check Complete.");
        }

        public void SearchTags(string term)
        {
            System.Console.WriteLine("Starting Search for Term \"" + term + "\"" + ".");
            collection.EnsureIndex("Tags");
            var result = collection.FindAll().Where(x => x.Tags.Any(z => z.Contains(term)));
            System.Console.WriteLine("Found " + result.Count() + " Results.");
            foreach (Entry e in result)
            {
                System.Console.WriteLine("Result Found With Title: \"" + e.Title + "\" And Identifier: " + e.Identifier);
            }
        }

        public void Delete()
        {
            System.Console.WriteLine("Deleting Database...");
            var result = collection.FindAll();
            foreach(Entry e in result)
            {
                collection.Delete(e.Id);
            }
            System.Console.WriteLine("Database Deleted.");
        }

    }
}

//Example code



//Database d = new Database(@"c:\users\jacob\documents\db1.db");

//List<String> testargs1 = new List<string>
//            {
//                "apple",
//                "orange",
//                "banana"
//            };
//List<String> testargs2 = new List<string>
//            {
//                "red",
//                "yellow",
//                "orange"
//            };
//List<String> testargs3 = new List<string>
//            {
//                "dog",
//                "cat",
//                "mouse"
//            };

//d.AddEntry("one", "description1", testargs1.ToArray());
//            d.AddEntry("two", "description2", testargs2.ToArray());
//            d.AddEntry("three", "description3", testargs3.ToArray());

//            d.collection.EnsureIndex(x => x.Tags, "$.tags[*]");

//            //var result = d.collection.Find(Query.EQ("tags", "red"));
//            var result = d.collection.FindAll().Where(x => x.Tags.Any(z => z.Contains("red")));
//            foreach (Entry e in result)
//            {
//                System.Console.WriteLine("RESULT FOUND WITH ID: " + e.Id);
//                System.Console.ReadKey();
//            }
