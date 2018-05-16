using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTICDownloader
{
    class DTICConsole
    {
        public static string directorypath = @"D:\";

        Downloader d;
        Database db;

        public DTICConsole()
        {
            d = new Downloader(directorypath, 10);
            db = new Database(directorypath + "db.db");
            

            string input;

            while (true)
            {
                System.Console.Write("DTIC DL> ");
                input = System.Console.ReadLine();
                
                switch(input.ToLower())
                {
                    case "build catalog":
                        System.Console.WriteLine("Scraping Links.");
                        LinkScraper ls = new LinkScraper(directorypath + "catalog.txt");
                        ls.BuildCatalog();
                        break;
                    case "download files":
                        System.Console.WriteLine("Downloading Files.");
                        break;
                    case "delete database":
                        System.Console.WriteLine("Are You Sure? [y/n]");
                        input = System.Console.ReadLine();
                        
                        NestedSwitch:
                            switch (input.ToLower())
                            {
                                case "y":
                                    //Delete Database
                                    break;
                                case "n":
                                    //Don't Delete Database
                                    break;
                                default:
                                    System.Console.WriteLine("Type 'y' For Yes or 'n' For No.");
                                goto NestedSwitch;
                            }
                        
                        break;
                    case "build database":
                        break;
                    case "search database":
                        System.Console.WriteLine("Enter The Tag to Search For:");
                        input = System.Console.ReadLine();
                        //search db
                        break;
                    case "exit":
                        System.Environment.Exit(0);
                        break;
                    case "help":
                        System.Console.WriteLine("  build catalog   -- scrapes all links and builds new catalog.");
                        System.Console.WriteLine("  download files  -- downloads files.");
                        System.Console.WriteLine("  delete database -- snipe the entire database");
                        System.Console.WriteLine("  search database -- single tag search only because laziness");
                        System.Console.WriteLine("  build database  -- scrapes entries from catalog for tags, description, and title");
                        System.Console.WriteLine("  exit            -- exits the program");
                        System.Console.WriteLine("  help            -- displays this menu");
                        break;
                    default:
                        System.Console.WriteLine("Invalid Option. Type 'help' For Help.");
                        break;
                }

            }
        }

    }

}
