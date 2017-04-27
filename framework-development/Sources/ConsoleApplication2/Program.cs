using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Accord.MachineLearning;
using Accord.MachineLearning.DecisionTrees;
using System.IO;
using System.Data;

namespace ConsoleApplication2
{
    class Program
    {
        static void Main(string[] args)
        {
            //C45TextClassify c45 = new C45TextClassify();
            //string featurePath = @"C:\Users\anupams\imp\pers\Shared\Custom Data";
            //var c45DTree =  c45.CreateTextClassificationTrainingData(featurePath, null);

            //testClassification(featurePath, "SelectedFeatures.txt", c45DTree);

            string location = @"C:\Users\anupams\imp\pers\Annu\Mtech\sem 4\DataSet\20news-19997.tar\20news-19997\20_newsgroups";
            string target = @"C:\Users\anupams\imp\pers\Shared\1";
            PrepareData(location, target);
        }

        private static void testClassification(string featureDirectoryPath, string featureFileName, DecisionTree c45DTree)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(featureDirectoryPath + "/TESTINGDATA");
            string featureFilePath = string.Format("{0}/{1}", featureDirectoryPath, featureFileName);
            List<string> lstFeatures = C45TextClassify.GetListOfFeatures(featureFilePath);
            var data = C45TextClassify.GetFeatureInTable(lstFeatures, directoryInfo.FullName);

            var lstClasses = c45DTree.ClassArray;
            if (File.Exists(featureDirectoryPath + "/TestResult.txt"))
            {
                File.Delete(featureDirectoryPath + "/TestResult.txt");
            }
            var ff = File.Create(featureDirectoryPath + "/TestResult.txt");
            ff.Close();
            foreach (DataRow row in data.Rows)
            {
                var inputdata = GetInputData(row);

                var output = c45DTree.Decide(inputdata);
                var found = lstClasses[output];
                var expected = row["ResultClass"];

                File.AppendAllLines(featureDirectoryPath + "/TestResult.txt", new string[] { "Found:" + found + "| Expected: " + expected });
            }
            //}
        }

        private static double[] GetInputData(DataRow row)
        {
            List<double> input = new List<double>();
            foreach (DataColumn col in row.Table.Columns)
            {
                if (col.ColumnName != "ResultClass")
                {
                    input.Add(Convert.ToDouble(row[col.ColumnName]));
                }
            }
            return input.ToArray();
        }

        public static void PrepareData(string location, string target)
        {
            var experimentFolder = target + "/ExperimentData/";
            if (Directory.Exists(experimentFolder))
            {
                Directory.Delete(experimentFolder, true);
            }
            Directory.CreateDirectory(experimentFolder);


            var trainingFolder = Path.Combine(experimentFolder, "TRAININGDATA");
            var testingFolder = Path.Combine(experimentFolder, "TESTINGDATA");
            var trainingDataPercentage = 70;
            Dictionary<string, int> trainingFilesCountClasswise = new Dictionary<string, int>();
            Dictionary<string, int> trainingFilesCreatedCountClasswise = new Dictionary<string, int>();
            var files = Directory.GetFiles(location, "*.*", SearchOption.AllDirectories);

            foreach (var dir in Directory.GetDirectories(location))
            {
                var cls = Path.GetFileName(dir);
                var count = Directory.GetFiles(dir, "*.*", SearchOption.AllDirectories).Length;
                trainingFilesCountClasswise.Add(cls, (count * trainingDataPercentage) / 100);
            }

            foreach (var file in files)
            {
                var cls = Path.GetFileName(Path.GetDirectoryName(file));
                var classPath = "";
                if (!trainingFilesCreatedCountClasswise.ContainsKey(cls)
                    || trainingFilesCountClasswise[cls] > trainingFilesCreatedCountClasswise[cls])
                {
                    classPath = Path.Combine(trainingFolder, cls);
                }
                else
                {
                    classPath = Path.Combine(testingFolder, cls);
                }
                if (!Directory.Exists(classPath))
                {
                    Directory.CreateDirectory(classPath);
                }
                File.Copy(file, Path.Combine(classPath, Path.GetFileName(file)));                
                DicUpdate(trainingFilesCreatedCountClasswise, cls, 1);
                Console.WriteLine(trainingFilesCreatedCountClasswise[cls] + ":  " + file);
            }
        }

        private static void DicUpdate(Dictionary<string, int> dic, string key, int increasevalue)
        {
            if(dic.ContainsKey(key))
            {
                dic[key] += increasevalue;
            }
            else
            {
                dic.Add(key, increasevalue);
            }
        }
    }
}
