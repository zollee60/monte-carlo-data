using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static MonteCarlo.DataGenerator;

namespace MonteCarlo
{
    class DataMerger
    {

        private Dictionary<int, double> _avgValues;
        private List<initValues> _initValue;

        public DataMerger(List<initValues> initValues)
        {
            AvgValues = new Dictionary<int, double>();
            InitValues = initValues;
        }

        public Dictionary<int, double> AvgValues { get => _avgValues; set => _avgValues = value; }
        internal List<initValues> InitValues { get => _initValue; set => _initValue = value; }

        public void ReadData()
        {
            for (int i = 0; i < InitValues.Count; i++)
            {
                try
                {
                    string inputString = string.Format("size_{0}_points_{1}_tries_{2}.csv", InitValues[i].N, InitValues[i].P, InitValues[i].T);
                    using (StreamReader sr = new StreamReader(inputString))
                    {
                        sr.ReadLine();
                        double sum = 0;
                        while (!sr.EndOfStream)
                        {
                            string line = sr.ReadLine();
                            string[] tmp = line.Split(';');
                            sum += Convert.ToDouble(tmp[1]);
                        }
                        AvgValues.Add(InitValues[i].P, sum / InitValues[i].T);
                    }
                }
                catch (FileNotFoundException fnfe)
                {
                    Console.WriteLine("Hiba történt: " + fnfe.Message);
                }
            }
        }

        public void PrintData()
        {
            try
            {
                using (StreamWriter sw = File.CreateText("finalResults.csv"))
                {
                    sw.WriteLine("Szimulációk száma;Átlagos eltérés a PI-től");
                    foreach (var value in AvgValues)
                    {
                        sw.WriteLine(value.Key + ";" + value.Value);
                    }
                }
            }
            catch (FileNotFoundException fnfe)
            {
                Console.WriteLine("Hiba történt: " + fnfe.Message);
            }
        }
    }
}
