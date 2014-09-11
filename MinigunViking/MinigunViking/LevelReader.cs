using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MinigunViking
{
    class LevelReader
    {
        public LevelReader()
        {
        }

        public String[] readLevel(String level)
        {
            try
            {
                using (StreamReader sr = new StreamReader(level + ".txt"))
                {
                    string[] line = new string[7];
                    int counter = 0;

                    while ((line[counter] = sr.ReadLine()) != null)
                    {
                        counter++;
                    }
                    sr.Close();

                    return line;
                }
            }
            catch (Exception e)
            {
                string[] line = new string[2];
                return line;
            }
        }





    }
}
