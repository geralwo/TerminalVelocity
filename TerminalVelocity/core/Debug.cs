using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerminalVelocity.core
{
    internal class Debug
    {
        //private static Debug singleton;
        private static List<string> debugEntries = new List<string>();

        public static void AddDebugEntry(string _entry)
        {
            if (debugEntries.Contains(_entry))
                return;
            debugEntries.Add(_entry);
        }

        public static void PrintToFile(string filename)
        {
            string path = Directory.GetCurrentDirectory();
            string filePath = Path.Combine(path, filename);
            // Create a file to write to.
            using (StreamWriter sw = File.CreateText(filePath))
            {
                foreach (string entry in debugEntries)
                {
                    sw.Write(DateTime.Now + ": ");
                    sw.WriteLine(entry);
                }
            }
        }


    }
}
