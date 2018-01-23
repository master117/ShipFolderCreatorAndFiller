using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Imouto.BooruParser;
using Imouto.BooruParser.Loaders;
using Imouto.BooruParser.Model.Base;
using static Imouto.BooruParser.Model.Base.SearchResult;

namespace ShipFolderCreatorAndFiller
{
    class Program
    {
        private static Random random = new Random();

        //This tool creates and fills folders according to all size 2 permutations of a list, in alphabetic order, with images from sankakucomplex
        static void Main(string[] args)
        {
            //Create List of ship candidates
            Dictionary<string, string> candidates = new Dictionary<string, string>()
            {
                { "Honoka", "kousaka_honoka" },
                { "Kotori", "minami_kotori" },
                { "Umi", "sonoda_umi" },
                { "Hanayo", "koizumi_hanayo" },
                { "Maki", "nishikino_maki" },
                { "Rin", "hoshizora_rin" },
                { "Nico", "yazawa_nico" },
                { "Nozomi", "toujou_nozomi" },
                { "Eli", "ayase_eli" },
                { "Chika", "takami_chika" },
                { "You", "watanabe_you" },
                { "Riko", "sakurauchi_riko" },
                { "Hanamaru", "kunikida_hanamaru" },
                { "Ruby", "kurosawa_ruby" },
                { "Yoshiko", "tsushima_yoshiko" },
                { "Mari", "ohara_mari" },
                { "Dia", "kurosawa_dia" },
                { "Kanan", "matsuura_kanan" }
            };

            //Add additional tags, note: these are only added if an api key exists, gold members are limited to 4 extra tags, platinum to 10, adjust length accordingly
            List<string> additionalTags = new List<string>()
            {
                "2girls",
                "rating:safe",
                "order:score"
            };

            //Sort Alphabetically
            List<string> sortedCan = candidates.Keys.ToList();
            sortedCan.Sort();

            //Aquire username and pass
            string user = null;
            string pass = null;
            if (File.Exists("user.txt") && File.Exists("api.txt")))
            {
                user = File.ReadAllText("user.txt");
                pass = File.ReadAllText("api.txt");
            }

            //2 Loops for size 2
            // Go 1 -> n-1
            int counter = 0;
            for (int i = 0; i < sortedCan.Count - 1; i++)
            {
                //Go 2 -> n
                for (int j = i + 1; j < sortedCan.Count; j++)
                {
                    //Get Image File
                    List<string> tags = new List<string>();
                    tags.Add(candidates[sortedCan[i]]);
                    tags.Add(candidates[sortedCan[j]]);

                    //if Premium user, add tags
                    if (pass != null)
                        tags.AddRange(additionalTags);

                    string link = BooruHandler.GetFileLink(tags, "DANBOORU", user, pass);

                    //Build Directory Name
                    string directory = sortedCan[i] + " x " + sortedCan[j];

                    //Proceed if image exists
                    if (link != null)
                    {                       
                        //Delete old Directory 
                        if (Directory.Exists(directory))
                            Directory.Delete(directory);

                        //Create Dictionary
                        Console.WriteLine("Creating: " + sortedCan[i] + " x " + sortedCan[j]);
                        Directory.CreateDirectory(directory);
                        counter++;

                        //Download Image into directory
                        using (WebClient client = new WebClient())
                        {
                            //TODO don't use random string for filename, instead use hash use Booru object to transfer
                            string path = directory + "/" + RandomString(20) + link.Substring(link.Count() - 4);
                            Console.WriteLine("Downloading: " + link + " to " + path);
                            client.DownloadFile(new Uri(link), path);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Nothing found for: " + directory);
                    }
                }
            }

            //Wait for read
            Console.WriteLine("Finished...");
            Console.ReadLine();
        }
        
        //Returns a random string of length length
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
