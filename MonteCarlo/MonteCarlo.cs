using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MonteCarlo
{
    class MonteCarlo
    {
        private List<double> _avgApproxValues;
        private List<double> _approximations;
        private int _in;
        private int _all;
        private int _numOfRandPoints;
        private int _numOfTries;
        private int _size;
        private double _avgOfApproxs;
        private double _closest;
        private string _outputString;

        public MonteCarlo()
        {
            Approximations = new List<double>();
            AvgApproxValues = new List<double>();
            In = 0;
            All = 0;
        }

        public List<double> Approximations { get => _approximations; set => _approximations = value; }
        public int In { get => _in; set => _in = value; }
        public int All { get => _all; set => _all = value; }
        public int NumOfRandPoints { get => _numOfRandPoints; set => _numOfRandPoints = value; }
        public int NumOfTries { get => _numOfTries; set => _numOfTries = value; }
        public double AvgOfApproxs { get => _avgOfApproxs; set => _avgOfApproxs = value; }
        public double Closest { get => _closest; set => _closest = value; }
        public int Size { get => _size; set => _size = value; }
        public List<double> AvgApproxValues { get => _avgApproxValues; set => _avgApproxValues = value; }
        public string OutputString { get => _outputString; set => _outputString = value; }

        private double ApproximatePI() => 4 * In * Math.Pow(Size, 2) / All / Math.Pow(Size, 2);

        private double GetMeanOfApproxs() => Approximations.Average();
        private List<double> RemoveMinMax(List<double> list) => list.Where(x => x != list.Max() && x != list.Min()).ToList();
        private double GetClosestApprox() => Approximations.Aggregate((x, y) => Math.Abs(x - Math.PI) < Math.Abs(y - Math.PI) ? x : y);
        private bool CheckIfInside(int x, int y) => Math.Pow(x, 2) + Math.Pow(y, 2) <= Math.Pow(Size, 2);

        public void Init(int N, int P, int T)
        {
            Size = N;
            NumOfRandPoints = P;
            NumOfTries = T;
            OutputString = string.Format("size_{0}_points_{1}_tries_{2}.csv", Size, NumOfRandPoints, NumOfTries);
        }

        private void PrintResults()
        {
            Console.WriteLine("Value of PI: " + Math.PI);
            Console.WriteLine("Average of approximation: " + AvgOfApproxs);
            Console.WriteLine("Closest approximation: " + Closest + "\n");
        }

        private void Reset()
        {
            Approximations.Clear();
            In = 0;
            All = 0;
            AvgOfApproxs = 0;
            Closest = 0;
        }

        private void CreateOutputFile()
        {
            try
            {
                using (StreamWriter sw = File.CreateText(OutputString))
                {
                    sw.WriteLine("NumberOfPoints;DeviationFromPI");
                }
            }
            catch (FileNotFoundException fnfe)
            {
                Console.WriteLine("Hiba történt: " + fnfe.Message);
            }
            catch (IOException ioe)
            {
                Console.WriteLine("Hiba történt: " + ioe.Message);
            }

        }

        private void WriteResultsToFile()
        {
            if (File.Exists(OutputString))
            {
                try
                {
                    using (StreamWriter sw = File.AppendText(OutputString))
                    {
                        sw.WriteLine(NumOfRandPoints + ";" + Math.Abs(Math.PI - AvgOfApproxs));
                    }
                }
                catch (FileNotFoundException fnfe)
                {
                    Console.WriteLine("Hiba történt: " + fnfe.Message);
                }
                catch (IOException ioe)
                {
                    Console.WriteLine("Hiba történt: " + ioe.Message);
                }
            }

        }

        private void RunSimulation()
        {
            for (int i = 0; i < NumOfRandPoints; i++)
            {
                Random r = new Random();
                int x = r.Next(0, Size);
                int y = r.Next(0, Size);
                All++;
                if (CheckIfInside(x, y)) In++;
                //Console.WriteLine(ApproximatePI());
                Approximations.Add(ApproximatePI());

            }
        }

        public void Run()
        {
            CreateOutputFile();
            for (int i = 0; i < NumOfTries; i++)
            {
                RunSimulation();
                //RemoveMinMax(Approximations);
                AvgOfApproxs = GetMeanOfApproxs();
                Closest = GetClosestApprox();
                WriteResultsToFile();
                PrintResults();
                Reset();
            }
        }


    }
}
