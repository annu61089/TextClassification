using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord.MachineLearning.DecisionTrees;
using Accord.MachineLearning.DecisionTrees.Learning;
using Accord.Math;
using Accord.Statistics.Filters;

namespace Accord.MachineLearning
{
    public class C45TextClassify
    {
        private DataTable _data;
        public DecisionTree CreateTextClassificationTrainingData(string trainingDataLocation, List<string> lstSelectedFeature)
        {
            //string featureFilePath = string.Format("{0}/{1}", featureDirectoryPath, featureFileName);
            if (lstSelectedFeature.Count == 0)
            {
                return null;
            }

            List<string> lstFeatures = lstSelectedFeature;

            var c45DecisionTree = PrepareC45Data(lstFeatures, trainingDataLocation + "/TRAININGDATA");
            var c45 = new C45Learning(c45DecisionTree.tree);
#pragma warning disable 612,618
            double error = c45.Run(c45DecisionTree.inputs, c45DecisionTree.outputs);
            var dTree = c45.Learn(c45DecisionTree.inputs, c45DecisionTree.outputs);
            SetClassesArray(dTree);
#pragma warning restore 612,618
            return dTree;
        }

        public static List<string> GetListOfFeatures(string featureFilePath)
        {
            var featureLines = File.ReadAllLines(featureFilePath);
            var lstFeatures = new List<string>();

            foreach (var featureLine in featureLines)
            {
                //var lineSplits = featureLine.Split('|');
                if (!string.IsNullOrWhiteSpace(featureLine))
                {
                    lstFeatures.AddRange(featureLine.Split(','));
                }
            }
            return lstFeatures;
        }

        private void SetClassesArray(DecisionTree tree)
        {
            var distictData = _data.DefaultView.ToTable(true, "ResultClass");
            List<string> lstClasses = new List<string>();
            foreach (DataRow row in distictData.Rows)
            {
                lstClasses.Add(row["ResultClass"].ToString());
            }
            tree.ClassArray = lstClasses.ToArray();
        }

        private C45Output PrepareC45Data(List<string> lstFeatures, string trainingDataLocation)
        {
            int count = 0;

            var data = GetFeatureInTable(lstFeatures, trainingDataLocation);
            _data = data;
            // Create a new codification codebook to
            // convert strings into integer symbols
            Codification codebook = new Codification(data);

            DecisionVariable[] attributes = new DecisionVariable[lstFeatures.Count];
            count = 0;
            foreach (var feature in lstFeatures)
            {
                ++count;
                attributes[count-1] = new DecisionVariable("FEATURE_" + count, codebook["FEATURE_" + count].NumberOfSymbols); // 2 possible values (true, false)
            }
            //attributes[count] = new DecisionVariable("ResultClass", codebook["ResultClass"].NumberOfSymbols);


            int classCount = codebook["ResultClass"].NumberOfSymbols; // no of classes

            DecisionTree tree = new DecisionTree(attributes, classCount);

            // Extract symbols from data and train the classifier
            DataTable symbols = codebook.Apply(data);

            List<string> lstColumnNames = new List<string>();
            foreach(DataColumn x in data.Columns)
            { 
                if (x.ColumnName.Contains("FEATURE"))
                {
                    lstColumnNames.Add(x.ColumnName);
                }
            };
            C45Output c45Output = new C45Output();
            //inputs = symbols.ToArray("Outlook", "Temperature", "Humidity", "Wind");
            c45Output.tree = tree;
            c45Output.inputs = symbols.ToJagged(lstColumnNames.ToArray());
            c45Output.outputs = symbols.ToArray<int>("ResultClass");
            return c45Output;
        }

        public static DataTable GetFeatureInTable(List<string> lstFeatures, string dataLocation, bool addFileName = false)
        {
            var data = new DataTable("Text_Classification");
            var count = 0;
            //Adding feature Columns
            lstFeatures.ForEach(x => data.Columns.Add("FEATURE_" + ++count, typeof(string)));
            //Add Class Columns
            data.Columns.Add("ResultClass", typeof(string));

            if(addFileName)
            {
                data.Columns.Add("DocumentPath", typeof(string));
            }

            //data.Columns.Add("Day", typeof(string));
            //data.Columns.Add("Outlook", typeof(string));
            //data.Columns.Add("Temperature", typeof(double));
            //data.Columns.Add("Humidity", typeof(double));
            //data.Columns.Add("Wind", typeof(string));
            //data.Columns.Add("PlayTennis", typeof(string));

            foreach (var filePaths in GetTrainingFilePaths(dataLocation))
            {
                count = 0;
                var clss = filePaths.Key;
                var files = filePaths.Value;
                foreach (var file in files)
                {
                    var row = GetDataRowFeatureFile(file, data, lstFeatures);
                    row["ResultClass"] = clss;
                    if(addFileName)
                    {
                        row["DocumentPath"] = file;
                    }
                    data.Rows.Add(row);
                }
            }
            return data;
        }

        public static DataRow GetDataRowFeatureFile(string file, DataTable data, List<string> lstFeatures)
        {
            if(data == null)
            {
                data = GetFeatureDataTable(lstFeatures);
            }
            var text = File.ReadAllText(file);
            var row = data.NewRow();
            int count = 0;
            lstFeatures.ForEach(feature =>
            {
                row["FEATURE_" + ++count] = text.Contains(feature) ? "1" : "0";
            });
            return row;
        }

        private static DataTable GetFeatureDataTable(List<string> lstFeatures)
        {
            var data = new DataTable("Text_Classification");
            var count = 0;
            //Adding feature Columns
            lstFeatures.ForEach(x => data.Columns.Add("FEATURE_" + ++count, typeof(string)));
            //Add Class Columns
            data.Columns.Add("ResultClass", typeof(string));
            return data;
        }

        private static Dictionary<string, List<string>> GetTrainingFilePaths(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                return null;
            }
            var dicFiles = new Dictionary<string, List<string>>();
            foreach (var directory in Directory.GetDirectories(directoryPath))
            {

                string cls = new DirectoryInfo(directory).Name;
                var files = Directory.GetFiles(directory);
                dicFiles.Add(cls, files.AsEnumerable().ToList());
            }
            return dicFiles;
        }


        private class C45Output
        {
            public DecisionTree tree;
            public double[][] inputs;
            public int[] outputs;
        }
    }
}
