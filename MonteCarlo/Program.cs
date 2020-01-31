using System;

namespace MonteCarlo
{
    class Program
    {
        static void Main(string[] args)
        {
            DataGenerator dg = new DataGenerator("input.csv");
            dg.ReadInitValues();
            dg.RunAllSimulations();

            DataMerger dm = new DataMerger(dg.Iv);
            dm.ReadData();
            dm.PrintData();
        }
    }
}
