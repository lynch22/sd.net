using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Threading.Tasks;

namespace getCongressVotes
{
    class Program
    {
        static void Main(string[] args)
        {
            WebClient webClient = new WebClient();
            String uri;
            String filePath;
            for (int nVote = 1; nVote <= 515; nVote++)
            {
                uri = String.Format("https://www.govtrack.us/data/congress/113/votes/2014/h{0}/data.xml", nVote.ToString());
                filePath = String.Format(@"D:\Coursera\SNA\h{0}.xml", nVote.ToString());
                Console.WriteLine(uri + " - " + filePath);
                try
                {
                    webClient.DownloadFile(uri, filePath);
                }
                catch { }
            }
        }
    }
}
