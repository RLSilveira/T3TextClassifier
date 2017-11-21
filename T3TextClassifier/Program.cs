using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;

namespace T3TextClassifier
{
    class Program
    {
        static void Main(string[] args)
        {

            var file = @"D:\Pucrs\IA\T3TextClassifier\T3TextClassifier\Textos\CORPUS DG ESPORTES - final.txt";

            if (args.Length > 0)
            {
                file = args[0];
            }

            var textList = extractTexts(file);

            foreach (var text in textList)
            {
                
            }

            var site = @"http://visl.sdu.dk/visl/pt/parsing/automatic/parse.php";


            readHTML(site);



        }

        class Text
        {
            public string Title { get; set; }
            public string FullText { get; set; }
            public string NormalizedText { get; set; }


            public void addLine(string line)
            {
                FullText += '\n' + line;
            }

            public override string ToString()
            {
                return $"{{{Title} - {FullText.Substring(0, Math.Min(FullText.Length, 200))} }}";
            }
        }

        static List<Text> extractTexts(string path)
        {

            if (!File.Exists(path)) return null;

            var text = File.ReadAllText(path);

            var ret = new List<Text>();
            Text current = null;

            foreach (var line in text.Split('\n'))
            {

                current?.addLine(line);

                if (line.StartsWith("TEXTO "))
                {
                    if (current != null) ret.Add(current);
                    current = new Text() { Title = line };
                }
            }

            if (current != null) ret.Add(current);

            return ret;

        }




        static void readHTML(string url)
        {
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(url);
            myRequest.Method = "GET";
            WebResponse myResponse = myRequest.GetResponse();
            StreamReader sr = new StreamReader(myResponse.GetResponseStream(), System.Text.Encoding.UTF8);
            string result = sr.ReadToEnd();
            sr.Close();
            myResponse.Close();

            
        }


    }
}
