using Accord.MachineLearning;
using Accord.MachineLearning.DecisionTrees;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextClassification.Text_Classification
{
    public class TrainingTextSamples : ExecuterServiceModule.IExecuterService<DecisionTree>
    {
        public string TrainingDataSetFolderLocation { get; set; }
        public List<string> lstSelectedFeature { get; set; }

        public async Task<DecisionTree> Execute()
        {
            C45TextClassify c45 = new C45TextClassify();            
            return await Task.Run(() => c45.CreateTextClassificationTrainingData(TrainingDataSetFolderLocation, lstSelectedFeature));            
        }
    }
}
