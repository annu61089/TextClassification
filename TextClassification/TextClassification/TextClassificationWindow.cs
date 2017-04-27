using Accord.MachineLearning.DecisionTrees;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TextClassification.Text_Classification;

namespace TextClassification
{
    public partial class TextClassificationWindow : Form
    {
        private string _trainingDataSetLocation { get; set; }
        private List<string> _featuresSelected { get; set; }
        private DecisionTree _c45Tree;
        
        private void TextClassificationWindow_Closing(object sender, FormClosingEventArgs e)
        {
            //   if (string.Equals((sender as Button).Name, @"CloseButton"))
            System.Windows.Forms.Application.Exit();
        }
        public TextClassificationWindow()
        {
            InitializeComponent();         
        }

        public TextClassificationWindow(string trainingDataSetLocation, List<string> featuresSelected)
        {
            InitializeComponent();
            _trainingDataSetLocation = trainingDataSetLocation;
            lblTrainingDataSetLocation.Text = getTrimmedDataSet(trainingDataSetLocation ?? string.Empty);
            _featuresSelected = featuresSelected;
        }
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MainWindow main = new MainWindow();
            App.Current.MainWindow = main;
            Close();
            main.Show();
        }

        private void lnkChangeDataSet_LinkClicked(object sender, EventArgs e)
        {
            if (TrainingDataDialog.ShowDialog() == DialogResult.OK)
            {
                //button1.Visible = false;
                lnkChangeDataSet.Visible = true;
                lblTrainingDataSetLocation.Text = getTrimmedDataSet(TrainingDataDialog.SelectedPath);
                _trainingDataSetLocation = TrainingDataDialog.SelectedPath;
                lblTrainingDataSetLocation.Visible = true;
            }

        }

        private string getTrimmedDataSet(string path)
        {
            return path.Length > 40
                                ? "...." + path.Substring(path.Length - 35, 35)
                                : path;
        }

        //private void lnkChangeDataSet_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        //{
        //    button1.Visible = true;
        //    lnkChangeDataSet.Visible = false;
        //    lblTrainingDataSetLocation.Text = "";
        //    lblTrainingDataSetLocation.Visible = false;
        //}

        private async void btnTrainData_Click(object sender, EventArgs e)
        {
            //Train Data
            var tr = new TrainingTextSamples();
            tr.TrainingDataSetFolderLocation = _trainingDataSetLocation;
            tr.lstSelectedFeature = _featuresSelected;
            _c45Tree = null;
            _c45Tree = await tr.Execute();
            btnClassifyDocument.Visible = btnClassifyFolder.Visible = true;
        }

        private void lnkShowhidefeature_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {            
            rtbLogger.AppendText("\n -----------------------Selected Features----------------------\n");
            rtbLogger.AppendText(string.Join(",   ", _featuresSelected));         
        }

        private async void btnClassifyDocument_Click(object sender, EventArgs e)
        {
            openFileDialog1.CheckFileExists = true;
            openFileDialog1.CheckPathExists = true;
            openFileDialog1.Multiselect = false;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                var filePath = openFileDialog1.FileName;
                ClassifyText ctext = new ClassifyText(_c45Tree, _featuresSelected);
                var result = await ctext.PredictDocumentClass(filePath, PrintResultToLog);
                PrintResultToLog(result);
            }
        }

        private void PrintResultToLog(TextClassificationResult result)
        {
            string logText = string.Format("{0}: File name: {1} ", getLoggableDateTimeString(), result.DocumentName);
            logText += string.Format("          Docuemnt Class: {0}{1}", result.FoundClass , System.Environment.NewLine);
            rtbLogger.AppendText(logText);
        }

        private void PrintResultToLog(List<TextClassificationResult> results)
        {
            int countCorrect = 0, countFailed = 0;
            foreach (var result in results)
            {
                string logText = string.Format("\n{0}:>>> File name:  {1}", getLoggableDateTimeString(), result.DocumentName);
                logText += string.Format("           Expected: {0},        Found: {1}", result.ExpectedClass, result.FoundClass, System.Environment.NewLine);
                rtbLogger.AppendText(logText);
                countCorrect += result.ExpectedClass == result.FoundClass ? 1 : 0;
                countFailed += result.ExpectedClass == result.FoundClass ? 0 : 1;
            }
            rtbLogger.AppendText("\n\n Correctly classified: " + countCorrect);
            rtbLogger.AppendText("\n InCorrectly classified: " + countFailed);
            rtbLogger.AppendText("\n Success %: " + countCorrect * 100/ (countCorrect + countFailed));
            rtbLogger.ScrollToCaret();
        }

        private string getLoggableDateTimeString()
        {
            return DateTime.Now.ToString();
        }

        private async void btnClassifyFolder_Click(object sender, EventArgs e)
        {
            TrainingDataDialog.SelectedPath = @"C:\Users\anupams\imp\pers\Shared\CustomData";
            if (TrainingDataDialog.ShowDialog() == DialogResult.OK)
            {
                var path = TrainingDataDialog.SelectedPath;
                ClassifyText ctext = new ClassifyText(_c45Tree, _featuresSelected);
                var results = await ctext.PredictAllDocumentClass(path, PrintResultToLog);
                PrintResultToLog(results);
            }
        }
    }
}
