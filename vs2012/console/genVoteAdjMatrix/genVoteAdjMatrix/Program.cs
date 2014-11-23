using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;

namespace genVoteAdjMatrix
{
    public class aLine : IEquatable<aLine>, IComparable<aLine>
    {
        public String vertex;
        public String vote;

        public aLine(String invertex, String invote)
        {
            vertex = invertex;
            vote = invote;
        }

        public override string ToString()
        {
            return "voter: " + vertex + "   vote: " + vote;
        }
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            aLine objAsaLine = obj as aLine;
            if (objAsaLine == null) return false;
            else return Equals(objAsaLine);
        }
        public int SortByNameAscending(string name1, string name2)
        {
            return name1.CompareTo(name2);
        }

        // Default comparer for Part type. 
        public int CompareTo(aLine compareaLine)
        {
            // A null value means that this object is greater. 
            if (compareaLine == null)
                return 1;

            else
                return this.vertex.CompareTo(compareaLine.vertex);
        }
        public override int GetHashCode()
        {
            return vertex.GetHashCode();
        }
        public bool Equals(aLine other)
        {
            if (other == null) return false;
            return (this.vertex.Equals(other.vertex));
        }
    }

    public class apair
    {
        public String vertex1;
        public String vertex2;
        public int vote;

        public apair(String invertex1, String invertex2, int invote)
        {
            vertex1 = invertex1;
            vertex2 = invertex2;
            vote = invote;
        }
    }

    public class matrixPair : IEquatable<matrixPair>, IComparable<matrixPair>
    {
        public String Legislator1;
        public String Legislator2;
        public int matches;

        public matrixPair(String leg1, String leg2, int match)
        {
            Legislator1 = leg1;
            Legislator2 = leg2;
            matches = match;
        }
        public override string ToString()
        {
            return Legislator1+"_"+Legislator2 + "_" + matches;
        }
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            matrixPair objAsmatrixPair = obj as matrixPair;
            if (objAsmatrixPair == null) return false;
            else return Equals(objAsmatrixPair);
        }
        public int SortByNameAscending(string name1, string name2)
        {
            return name1.CompareTo(name2);
        }

        // Default comparer for Part type. 
        public int CompareTo(matrixPair comparematrixPair)
        {
            // A null value means that this object is greater. 
            if (comparematrixPair == null)
                return 1;
            else
                return this.ToString().CompareTo(comparematrixPair.ToString());
        }

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }
        public bool Equals(matrixPair other)
        {
            if (other == null) return false;
            return (this.ToString().Equals(other.ToString()));
        }

    }

    public class congress
    {
        public List<matrixPair> senateAM = null;
        public List<matrixPair> houseAM = null;
        int nCongress;

        public congress(int newCongress)
        {
            senateAM = new List<matrixPair>();
            houseAM = new List<matrixPair>();
            nCongress = newCongress;
        }

        
        public void senateUpsert(apair pair)
        {
                senateAM.Add(new matrixPair(pair.vertex1, pair.vertex2, pair.vote));
        }

        public void houseUpsert(apair pair)
        {
                houseAM.Add(new matrixPair(pair.vertex1, pair.vertex2, pair.vote));
        }
    }
    
    class Program
    {
        static void Main(string[] args)
        {
            String congressPath;
            int nCongress;
            bool bCongressDone = false;
            bool bHouseDone = false;
            bool bSenateDone = false;
            List<congress> cons = new List<congress>();
            List<apair> outAr = null;

            for (nCongress = 113; nCongress <= 113; nCongress++)
            {
                congress con = new congress(nCongress);
                bCongressDone = false;
                bHouseDone = false;
                bSenateDone = false;
                int senateVotes = 0;
                int houseVotes = 0;
                // loop through the votes and aggregate the adjacency matrix
                //
                congressPath = @"d:\Coursera\SNA\" + nCongress.ToString();

                var senateFiles = Directory.EnumerateFiles(congressPath, "s*.xml");
                var houseFiles = Directory.EnumerateFiles(congressPath, "h*.xml");

                foreach (string sfile in senateFiles)
                {
                    outAr = ripSFile(sfile);
                    if (outAr.Count > 4)
                    {
                        senateVotes++;
                        foreach (apair pair in outAr)
                        {
                            con.senateUpsert(pair);
                        }
                    }
                }

                //foreach (string hfile in houseFiles)
                //{
                //    outAr = ripSFile(hfile);
                //    foreach (apair pair in outAr)
                //    {
                //        con.houseUpsert(pair);
                //    }
                //}

                congress newCon = new congress(nCongress);

                collapseAM(ref con, ref newCon);
                
                //cons.Add(con);

                writeCongress(nCongress, newCon, senateVotes, houseVotes);

                

            }


        }

        static public void collapseAM(ref congress con, ref congress newCon)
        {
            con.senateAM.Sort();
            con.houseAM.Sort();
            String prevLeg1 = "", prevLeg2 = "";
            int matches = 0;
            bool bRunning = false;
            foreach (matrixPair mp in con.senateAM)
            {
                if (bRunning)
                {
                    if (prevLeg1 != mp.Legislator1 || prevLeg2 != mp.Legislator2)
                    {
                        newCon.senateAM.Add(new matrixPair(prevLeg1, prevLeg2, matches));
                        prevLeg1 = mp.Legislator1;
                        prevLeg2 = mp.Legislator2;
                        matches = mp.matches;
                    }
                    else
                        matches += mp.matches;
                }
                if (!bRunning)
                {
                    prevLeg1 = mp.Legislator1;
                    prevLeg2 = mp.Legislator2;
                    matches = mp.matches;
                    bRunning = true;
                }
            }
            if(bRunning)
                newCon.senateAM.Add(new matrixPair(prevLeg1, prevLeg2, matches));

            prevLeg1 = "";
            prevLeg2 = "";
            matches = 0;
            bRunning = false;
            foreach (matrixPair mp in con.houseAM)
            {
                if (bRunning)
                {
                    if (prevLeg1 != mp.Legislator1 || prevLeg2 != mp.Legislator2)
                    {
                        newCon.houseAM.Add(new matrixPair(prevLeg1, prevLeg2, matches));
                        prevLeg1 = mp.Legislator1;
                        prevLeg2 = mp.Legislator2;
                        matches = mp.matches;
                    }
                    else
                        matches += mp.matches;
                }
                if (!bRunning)
                {
                    prevLeg1 = mp.Legislator1;
                    prevLeg2 = mp.Legislator2;
                    matches = mp.matches;
                    bRunning = true;
                }
            }
            if (bRunning)
                newCon.houseAM.Add(new matrixPair(prevLeg1, prevLeg2, matches));

        }

        static void writeCongress(int nCongress, congress con, int senateVotes, int houseVotes)
        {
            String conPath = @"d:\Coursera\SNA\Votes\";

            StreamWriter sw1 = new StreamWriter(conPath + "senate" + nCongress.ToString() + ".csv");
            StreamWriter sw2 = new StreamWriter(conPath + "house" + nCongress.ToString() + ".csv");

            sw1.WriteLine(String.Format("{0},{1},{2},{3}", "Source", "Target", "Weight","Type"));
            sw2.WriteLine(String.Format("{0},{1},{2},{3}", "Source", "Target", "Weight", "Type"));

            foreach (matrixPair mp in con.senateAM)
            {
                sw1.WriteLine(String.Format("{0},{1},{2},{3}", mp.Legislator1, mp.Legislator2, (float)mp.matches/ senateVotes, "Undirected"));
            }

            foreach (matrixPair mp in con.houseAM)
            {
                sw2.WriteLine(String.Format("{0},{1},{2},{3}", mp.Legislator1, mp.Legislator2, (float)mp.matches / houseVotes, "Undirected"));
            }

            sw1.Flush();
            sw1.Close();
            sw2.Flush();
            sw2.Close();
        }


        static List<apair> ripSFile(string sfile)
        {
            // traverse the XML
            List<aLine> inAr = new List<aLine>();
            List<apair> outAr = new List<apair>();
            int AyeVotes = 0;
            int NayVotes = 0;
            bool bDoThisFile = false;
            XmlReader xr = XmlReader.Create(new StreamReader(sfile));
            // rip it
            while (xr.Read())
            {
                if((xr.NodeType == XmlNodeType.Element) && (xr.Name.CompareTo("roll")==0))
                {
                    if (xr.HasAttributes)
                    {
                        AyeVotes = Convert.ToInt32(xr.GetAttribute("aye"));
                        NayVotes = Convert.ToInt32(xr.GetAttribute("nay"));
                        if (AyeVotes >= 4 && NayVotes >= 4)
                            bDoThisFile = true;
                    }
                }

                if(bDoThisFile && (xr.NodeType == XmlNodeType.Element) && (xr.Name.CompareTo("voter")==0))
                {
                    if(xr.HasAttributes)
                        inAr.Add(new aLine(xr.GetAttribute("id"), xr.GetAttribute("vote")));
                }
            }

            inAr.Sort();
            inAr.Sort(delegate(aLine x, aLine y)
            {
                if (x.vertex == null && y.vertex == null) return 0;
                else if (x.vertex == null) return -1;
                else if (y.vertex == null) return 1;
                else return x.vertex.CompareTo(y.vertex);
            });

            bool bStart = false;
            apair n ;
            foreach (aLine a in inAr)
            {
                bStart = false;
                foreach (aLine b in inAr)
                {
                    if (bStart)
                    {
                        if ((a.vote == b.vote) && (a.vote.CompareTo("+") == 0 || a.vote.CompareTo("-") == 0))
                            n = new apair(a.vertex, b.vertex, 1);
                        else
                            n = new apair(a.vertex, b.vertex, 0);
                        outAr.Add(n);
                    }
                    
                    if ((a.vertex == b.vertex && a.vote== b.vote))
                    {
                        bStart = true;
                        continue;
                    }
                }
            }
            return outAr;
        }
    }
}
