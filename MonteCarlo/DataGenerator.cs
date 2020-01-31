using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MonteCarlo
{
    class DataGenerator
    {
        public struct initValues
        {
            public int N, P, T;

            public initValues(string n, string p, string t)
            {
                N = Convert.ToInt32(n);
                P = Convert.ToInt32(p);
                T = Convert.ToInt32(t);
            }
        }

        private List<initValues> _iv;
        private string _inputString;
        private MonteCarlo _mc;

        public List<initValues> Iv { get => _iv; set => _iv = value; }
        public string InputString { get => _inputString; set => _inputString = value; }
        public MonteCarlo Mc { get => _mc; set => _mc = value; }

        public DataGenerator(string inputString)
        {
            InputString = inputString;
            Iv = new List<initValues>();
            Mc = new MonteCarlo();
        }

        public void ReadInitValues()
        {
            try
            {
                using (StreamReader sr = new StreamReader(InputString))
                {
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        string[] tmp = line.Split(';');
                        Iv.Add(new initValues(tmp[0], tmp[1], tmp[2]));
                    }
                }
            }
            catch (FileNotFoundException fnfe)
            {
                Console.WriteLine("Hiba történt: " + fnfe.Message);
            }
        }

        public void RunAllSimulations()
        {
            foreach (var iv in Iv)
            {
                Mc.Init(iv.N, iv.P, iv.T);
                Mc.Run();
            }
        }
    }
}
