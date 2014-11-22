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
            String uri;
            String filePath;
            int nVote;
            bool isDone = false;
            int nYear;
            
            
            int nCongress = 1;
            isDone = false;
            nVote = 1;
            nYear = 1;
            for (nCongress = 1; nCongress <= 76; nCongress++)
            {
                isDone = false;
                nVote = 1;
                nYear = 1;
                int needYear = 2;

                switch (nCongress)
                {
                    case 1:
                    case 5:
                    case 11:
                    case 13:
                    case 25:
                    case 34:
                    case 37:
                    case 40:
                    case 41:
                    case 42:
                    case 45:
                    case 46:
                    case 53:
                    case 55:
                    case 58:
                    case 61:
                    case 62:
                    case 63:
                    case 64:
                    case 65:
                    case 66:
                    case 71:
                    case 75:
                    case 76:
                        needYear = 3;
                        break;
                    case 67:
                        needYear = 4;
                        break;
                    default:
                        needYear = 2;
                        break;
                }

                while(!isDone)
                {
                    filePath = String.Format(@"D:\Coursera\SNA\{0}\h{1}.xml", nCongress.ToString(),nVote.ToString());
                    if (!Directory.Exists(Path.GetDirectoryName(filePath)))
                        Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                    uri = String.Format("https://www.govtrack.us/data/congress/{0}/votes/{1}/h{2}/data.xml", nCongress.ToString(), nYear.ToString(), nVote.ToString());
                    try
                    {
                        getFile(uri, filePath);
                        nVote++;
                    }
                    catch
                    {
                        if (nYear >= needYear)
                           isDone= true;
                        else
                            nYear++;
                    }
                }
                isDone = false;
                nVote = 1;
                nYear = 1;

                while (!isDone)
                {
                    filePath = String.Format(@"D:\Coursera\SNA\{0}\s{1}.xml", nCongress.ToString(),nVote.ToString());
                    if (!Directory.Exists(Path.GetDirectoryName(filePath)))
                        Directory.CreateDirectory(Path.GetDirectoryName(filePath));

                    uri = String.Format("https://www.govtrack.us/data/congress/{0}/votes/{1}/s{2}/data.xml", nCongress.ToString(),nYear.ToString(), nVote.ToString());
                    try
                    {
                        getFile(uri, filePath);
                        nVote++;
                    }
                    catch
                    {
                        if (nYear >= needYear)
                            isDone = true;
                        else
                            nYear++;
                    }
                }
            }
        }

            for (nCongress = 77; nCongress <= 113; nCongress++)
            {
                isDone = false;
                nVote = 1;
                if (nCongress == 77) nVote = 2;
                nYear = (nCongress - 77) * 2 + 1941;
                int needYear = nYear + 1;

                while(!isDone)
                {
                    filePath = String.Format(@"D:\Coursera\SNA\{0}\h{1}.xml", nCongress.ToString(),nVote.ToString());
                    if (!Directory.Exists(Path.GetDirectoryName(filePath)))
                        Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                    uri = String.Format("https://www.govtrack.us/data/congress/{0}/votes/{1}/h{2}/data.xml", nCongress.ToString(), nYear.ToString(), nVote.ToString());
                    try
                    {
                        getFile(uri, filePath);
                        nVote++;
                    }
                    catch
                    {
                        if (nYear >= needYear)
                           isDone= true;
                        else
                            nYear++;
                    }
                }
                isDone = false;
                nVote = 1;
                nYear = (nCongress - 77) * 2 + 1941;
                needYear = nYear + 1;

                while (!isDone)
                {
                    filePath = String.Format(@"D:\Coursera\SNA\{0}\s{1}.xml", nCongress.ToString(),nVote.ToString());
                    if (!Directory.Exists(Path.GetDirectoryName(filePath)))
                        Directory.CreateDirectory(Path.GetDirectoryName(filePath));

                    uri = String.Format("https://www.govtrack.us/data/congress/{0}/votes/{1}/s{2}/data.xml", nCongress.ToString(),nYear.ToString(), nVote.ToString());
                    try
                    {
                        getFile(uri, filePath);
                        nVote++;
                    }
                    catch
                    {
                        if (nYear >= needYear)
                            isDone = true;
                        else
                            nYear++;
                    }
                }
            }
        }

        
        
        static void getFile(String uri, String path)
        {
            WebClient webClient = new WebClient();
            Console.WriteLine(uri + " - " + path);
            try
            {
                webClient.DownloadFile(uri, path);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
