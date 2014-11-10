using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace crossprod
{
    public  class aLine
    {
        public String vertex;
        public String vote;
        public int weight;

        public aLine(String invertex, String invote, int inweight)
        {
            vertex = invertex;
            vote = invote;
            weight = inweight;
        }

    }

    public class apair
    {
        public String vertex1;
        public String vertex2;
        public String vote;
        public int weight;

        public apair(String invertex1, String invertex2, String invote, int inweight)
        {
            vertex1 = invertex1;
            vertex2 = invertex2;
            vote = invote;
            weight = inweight;
        }

    }
    public class Program
    {
        static void Main(string[] args)
        {

            StreamReader sr = new StreamReader(@"c:\tmp\inlist.csv");
            StreamWriter sw = new StreamWriter(@"c:\tmp\outlist.csv");

            List<aLine> inAr = new List<aLine>();
            List<apair> outAr = new List<apair>();

            String inLine;
            String[] inStrings;
            while ((inLine = sr.ReadLine()) != null)
            {
                inStrings = inLine.Split('\t');
                inAr.Add(new aLine(inStrings[0], inStrings[1], Convert.ToInt32(inStrings[2])));
            }
            bool bStart = false;

            foreach (aLine a in inAr)
            {
                bStart = false;
                foreach (aLine b in inAr)
                {
                    if (bStart && (a.vote == b.vote))
                    {
                        apair n = new apair(a.vertex, b.vertex, a.vote, a.weight == b.weight ? 1 : -1);
                        if (a.weight != 0 && b.weight != 0 && n.weight == 1)
                            outAr.Add(n);

                    }
                    if ((a.vertex == b.vertex && a.vote== b.vote))
                    {
                        bStart = true;
                        continue;
                    }
                }
            }
                    
            foreach( apair a in outAr)
            {
                sw.WriteLine(a.vertex1 + ',' + a.vertex2 + ',' + a.vote + ',' + a.weight);
            }
            sw.Flush();
            sw.Close();




        }
    }
}
