using Accord.MachineLearning;
using Accord.MachineLearning.DecisionTrees;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextClassification.Text_Classification
{
    public class ClassifyText
    {
        public DecisionTree c45Tree { get; set; }
        public List<string> SelectedFeatures { get; set; }
        
        public ClassifyText(DecisionTree tree, List<string> features)
        {
            c45Tree = tree;
            SelectedFeatures = features;
        }

        public async Task<TextClassificationResult> PredictDocumentClass(string inputDocPath, Action<TextClassificationResult> FollowupProcedure, string expected = null)
        {
            return await Task.Run(
                () =>
                {
                    var result = PredictClass(inputDocPath, expected ?? "UNKNOWN");
                    //FollowupProcedure(result);
                    return result;
                });
        }

        public async Task<List<TextClassificationResult>> PredictAllDocumentClass(string inputFolderPath, Action<List<TextClassificationResult>> FollowupProcedure, string resultFileName = null)
        {
            return await Task.Run(
                () =>
                {
                    var result = testClassification(inputFolderPath, resultFileName ?? "Results.txt");
                    //FollowupProcedure(result);
                    return result;
                });
        }

        private List<TextClassificationResult> testClassification(string featureDirectoryPath, string resultFile)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(featureDirectoryPath + "/TESTINGDATA");
            string resultFilePath = string.Format("{0}/{1}", featureDirectoryPath, resultFile);
            //List<string> lstFeatures = C45TextClassify.GetListOfFeatures(featureFilePath);
            var data = C45TextClassify.GetFeatureInTable(SelectedFeatures, directoryInfo.FullName, true);

            var lstClasses = c45Tree.ClassArray;
            if (File.Exists(resultFilePath))
            {
                File.Delete(resultFilePath);
            }
            var ff = File.Create(resultFilePath);
            ff.Close();
            List<TextClassificationResult> results = new List<TextClassificationResult>();
            foreach (DataRow row in data.Rows)
            {
                var inputdata = GetInputData(row);

                var output = c45Tree.Decide(inputdata);
                var found = "UNKNOWN";
                if(output > -1 && output < lstClasses.Count())
                {
                    found = lstClasses[output];
                }                
                var expected = row["ResultClass"].ToString();
                var docPath = row["DocumentPath"].ToString();
                File.AppendAllLines(resultFilePath, new string[] {"Doc:" +  Path.GetFileName(docPath) + "Found:" + found + "| Expected: " + expected });
                var result = new TextClassificationResult()
                {
                    DocumentName = Path.GetFileName(docPath),
                    DocumentPath = docPath,
                    ExpectedClass = expected,
                    FoundClass = found
                };
                results.Add(result);
            }
            return results;
        }

        private TextClassificationResult PredictClass(string file, string exp) 
        {
            var row = C45TextClassify.GetDataRowFeatureFile(file, null, SelectedFeatures);
            var inputdata = GetInputData(row);

            var output = c45Tree.Decide(inputdata);
            var found = c45Tree.ClassArray[output];            
            return new TextClassificationResult()
            {
                DocumentName = Path.GetFileName(file),
                DocumentPath = file,
                ExpectedClass = exp,
                FoundClass = found
            };
        }

        private static double[] GetInputData(DataRow row)
        {
            List<double> input = new List<double>();
            foreach (DataColumn col in row.Table.Columns)
            {
                if (col.ColumnName.StartsWith("FEATURE_"))
                {
                    input.Add(Convert.ToDouble(row[col.ColumnName]));
                }
            }
            return input.ToArray();
        }
    }
}
