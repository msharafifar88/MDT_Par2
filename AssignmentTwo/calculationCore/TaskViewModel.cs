using AssignmentTwo.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AssignmentTwo.ViewModel
{
    public class TaskViewModel
    {
        public TaskViewModel() { }
        public TaskViewModel (Generator dataSet1)
        {           
            Parallel.ForEach(dataSet1.generators.AsParallel().AsOrdered().Reverse(), genitem =>
            {
                foreach (List<double> dataSet_ in dataSet1.datasets)
                {
                    Debug.WriteLine($@"{DateTime.Now.ToString("HH:mm:ss")} {genitem.Name} {Math.Round(getResult(dataSet_, genitem.Operation), 2)}");
                    Thread.Sleep(genitem.Interval * 1000);
                }

            });
        }

        public List<string> doTask(Generator dataSet1)
        {
            List<string> strings = new List<string>();
            Parallel.ForEach(dataSet1.generators.AsParallel().AsOrdered().Reverse(), genitem =>
            {
                foreach (List<double> dataSet_ in dataSet1.datasets)
                {
                    strings.Add($@"{DateTime.Now.ToString("HH:mm:ss")} {genitem.Name} {Math.Round(getResult(dataSet_, genitem.Operation), 2)}");
                    Thread.Sleep(genitem.Interval * 1000);
                }

            });
            return strings;
        }



        private double getResult(List<double> list_, string operationType = null)
        {
            if (operationType == null)
                throw new Exception("Invalid Operation Type");
            switch (operationType.ToLower())
            {
                case "sum":
                    return list_.Sum();
                case "min":
                    return list_.Min();
                case "max":
                    return list_.Max();
                case "average":
                    return list_.Average();
                default:
                    throw new Exception("Invalid Operation Type");
            }

        }

    }
}
