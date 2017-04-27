using Accord.MachineLearning.DecisionTrees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextClassification
{
    public class PreservedResult
    {
        public List<string> SelectedFeatures { get; set; }

        public DecisionTree c45Tree { get; set; }
        public static PreservedResult GetInstance()
        {
            return null;
        }
    }
}
