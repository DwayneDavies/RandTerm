using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleLibrary
{
    public static class Init
    {

        public static void CheckFiles(string[] fileList, string programName)
        {
          
            foreach (string fileName in fileList)
                if (!File.Exists(fileName))
                    DownloadFiles(fileName, programName);

        }

        private static void DownloadFiles(string fileName, string programName)
        {
            System.Net.WebClient client = new WebClient();

            client.DownloadFile(@"https://raw.githubusercontent.com/DwayneDavies/" + programName + @"/blob/master/files/" + fileName, fileName);
        }

    }
}
