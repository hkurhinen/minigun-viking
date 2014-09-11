using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MinigunViking
{
    class IO
    {
        public IO()
        {
        }

        public void writeSettings(List<String> asetukset)
        {
            string mydocpath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < asetukset.Count; i++)
            {
                sb.AppendLine(asetukset[i]);
            }


            using (StreamWriter outfile = new StreamWriter("Viking_config.txt"))
            {
                outfile.Write(sb.ToString());
            }

        }


        public String readSettings(String setting)
        {
            try
            {
                using (StreamReader sr = new StreamReader("Viking_config.txt"))
                {
                    int counter = 0;
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line.Contains(setting))
                        {

                            String numericalvalue = line.Replace(setting, "");
                            return numericalvalue;
                        }
                        counter++;
                    }
                    sr.Close();

                    return "notfound";
                }
            }
            catch (Exception e)
            {
                return "notfound";
            }
        }




    }
}
