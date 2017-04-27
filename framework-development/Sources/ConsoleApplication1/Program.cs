using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord.MachineLearning;

//using Accord.MachineLearning;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            C45TextClassify c45 = new C45TextClassify();
            string featurePath = @"C:\Users\anupams\imp\pers\Shared\Custom Data";
            c45.CreateTextClassificationTrainingData(featurePath, "SelectedFeatures.txt");
        }
    }
}
