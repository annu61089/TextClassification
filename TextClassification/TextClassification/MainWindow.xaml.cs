using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using TextClassification.ExecuterServiceModule;
using TextClassification.FeatureSelector;
using TextClassification.Logger;

namespace TextClassification
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private LogWriter _logger;
        private OutlayerFetaureService _outlayerFetaure;
        private string _dataSetLocation;
        private List<string> _featureSelected;
        private bool _processingUp;
        public MainWindow()
        {
            _logger = new LogWriter();
            InitializeComponent();
            _outlayerFetaure = OutlayerFetaureService.GetInstance();
            PostWelcomeBanner();
            //rtbLogViewer.Visibility = Visibility.Hidden;          
        }

        private void PostWelcomeBanner()
        {
            var text = File.ReadAllText("../../Content/banner3.txt");
            //paraBanner.FontSize = 6.0;
            //paraBanner.LineHeight = 1;
            //paraBanner.Focus();
            paraBanner.Focus();
            rtbLogViewer.AppendText(text);

            //paraLogs.Focus();
            //rtbLogViewer.AppendText("Blue text");
            //FlowDocument fd = new FlowDocument(p);
            //rtbLogViewer.Document = fd;
            //rtbLogViewer.AppendText(text);
            //TextRange tr = new TextRange(rtbLogViewer.Document.ContentStart, rtbLogViewer.Document.);
            //rtbLogViewer.SelectAll();
            //var selectedText = rtbLogViewer.Selection;            
            //rtbLogViewer.AppendText(text);
            //paraBanner.Foreground = Brushes.Red;
            //var aa = SelectText(rtbLogViewer, 0, 100);
            //rtbLogViewer.Selection.ApplyPropertyValue(TextElement.FontSizeProperty, 6.0);
            //rtbLogViewer.Selection.ApplyPropertyValue(TextElement.FontFamilyProperty, "consolas");
            ////tr.Text = text;
            //paraBanner.LineHeight = 4;
            //TextPointer textPointer = rtbLogViewer.CaretPosition.GetInsertionPosition(LogicalDirection.Forward);
            //Run run = new Run("\n", textPointer);
            ////run.FontFamily = this.CurrentFontFamily;
            //run.FontSize = 14.0;
            //rtbLogViewer.CaretPosition = run.ElementEnd;

            Paragraph paragraph = new Paragraph();
            //rtbLogViewer.Document = new FlowDocument(paragraph);
            paragraph.LineHeight = 4;
            paragraph.FontSize = 11.0;
            //paraBanner.FontSize = 6.0;
            paragraph.Inlines.Add(new Run("\n")
            {
                Foreground = Brushes.Black
            });
            rtbLogViewer.Document.Blocks.Add(paragraph);
            
            //paraBanner.Inlines.Add(new LineBreak());            
            //DataContext = this;
            //paraBanner.Inlines.Add(new Run("\n")
            //{
            //    Foreground = Brushes.Blue
            //});
            //paraBanner.FontSize = 9.0;
            //paraBanner.LineHeight = 4;
            //paraBanner.Focus();
            //DataContext = this;


            //selectedText.ApplyPropertyValue(Paragraph.FontStretchProperty, 6);
            //rtbLogViewer.SelectionFont = new System.Drawing.Font("Tahoma", 10);
        }

        private string SelectText(System.Windows.Controls.RichTextBox rtb, int index, int length)
        {
            TextRange textRange = new TextRange(rtb.Document.ContentStart, rtb.Document.ContentEnd);

            if (textRange.Text.Length >= (index + length))
            {
                TextPointer start = textRange.Start.GetPositionAtOffset(index, LogicalDirection.Forward);
                TextPointer end = textRange.Start.GetPositionAtOffset(index + length, LogicalDirection.Backward);
                rtb.Selection.Select(start, end);
            }
            return rtb.Selection.Text;
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new FolderBrowserDialog();
            dialog.Dispose();
            const string currentExecutionLocation = @"C:\Users\anupams\imp\pers\Shared\CustomData";
            if (Directory.Exists(currentExecutionLocation))
            {
                dialog.SelectedPath = currentExecutionLocation;
            }

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK
                && !string.IsNullOrWhiteSpace(dialog.SelectedPath))
            {
                var baseRequest = new Corpus { DocumentBase = GetTextDocs(dialog.SelectedPath) };
                if (!baseRequest.DocumentBase.Any())
                {
                    System.Windows.Forms.MessageBox.Show(@"No text Document Found!!");
                }
                else
                {
                    _dataSetLocation = dialog.SelectedPath;
                    btnSelectDataset.Visibility = Visibility.Hidden;
                    lblDataSet.Visibility = Visibility.Visible;
                    lblDataSet.Content = dialog.SelectedPath.Length < 80
                        ? dialog.SelectedPath : "..." + dialog.SelectedPath.Substring(dialog.SelectedPath.Length - 75, 75);
                    fileCountTextBox.Text = "Files:  #" + baseRequest.DocumentBase.Count.ToString("#,##0");
                    stcpFileSize.Visibility = Visibility.Visible;
                    stcpFeatureSize.Visibility = Visibility.Visible;
                    changeDatasetLinkBlock.Visibility = Visibility.Visible;
                    lblFeatureCount.Visibility = Visibility.Visible;
                    txtFeatureCount.Visibility = Visibility.Visible;
                    btnSelectFeature.IsEnabled = true;
                    cbxAlgo.IsEnabled = true;
                    if (string.IsNullOrWhiteSpace(txtFeatureCount.Text))
                    {
                        txtFeatureCount.Text = "20";
                    }
                    featureCountTextBox.Text = "Calculating....";
                    UpdateFeatureCount(baseRequest);
                }
            }
        }

        private async void UpdateFeatureCount(Corpus corpus)
        {
            var executerService = new FeatureCountService(_outlayerFetaure, true) { DocCorpus = corpus };
            var featureCount = await executerService.Execute();
            featureCountTextBox.Text = "Features:   #" + featureCount.ToString("#,##0");
            var uriSource = new Uri(@"/images/D-Green.jpg", UriKind.Relative);
            imgFeatureCountImage.Source = new BitmapImage(uriSource);

        }


        private List<string> GetTextDocs(string folderPath)
        {
            var lstDocs = new List<string>();
            rtbLogViewer.AppendText(string.Format("lg:>  Reading dataset folder: {0} \r", folderPath));

            string[] files = Directory.GetFiles(folderPath, "*.*", SearchOption.AllDirectories);
            lstDocs.AddRange(files);
            //lstDocs.ForEach(doc => _logger.Log("FileName:" + doc, WriteLog));

            return lstDocs;
        }

        private int WriteLog(string timeStamp, string log)
        {
            rtbLogViewer.AppendText(string.Format("lg:>  {0}: {1} \r", timeStamp, log));
            rtbLogViewer.ScrollToEnd();
            return 1;
        }



        private void rtbLogViewer_SelectionChanged(object sender, RoutedEventArgs e)
        {
            var cbx = (System.Windows.Controls.CheckBox)sender;
            rtbLogViewer.Visibility = (cbx.IsChecked.HasValue && cbx.IsChecked.Value) ? Visibility.Visible : Visibility.Hidden;
        }

        private void changeDatasetLink_Click(object sender, RoutedEventArgs e)
        {
            changeDatasetLinkBlock.Visibility = Visibility.Hidden;
            btnSelectDataset.Visibility = Visibility.Visible;
            lblDataSet.Visibility = Visibility.Hidden;
            lblDataSet.Content = string.Empty;
            fileCountTextBox.Text = "";
            stcpFileSize.Visibility = Visibility.Hidden;
            stcpFeatureSize.Visibility = Visibility.Hidden;
            featureCountTextBox.Text = "";

        }

        private void btnSelectFeature_Click(object sender, RoutedEventArgs e)
        {
            if (cbxAlgo.SelectedIndex == 0)
            {
                RunACOAlgo();
                switchToTextClassBlock.Visibility = Visibility.Visible;
                lnkResetAllBlock.Visibility = Visibility.Visible;
                btnSelectFeature.IsEnabled = false;
                btnSelectDataset.IsEnabled = false;
            }
        }

        private async void RunACOAlgo()
        {
            //release UI controls
            await ProcessTextClassification(_dataSetLocation);

            //OpenTextClassficationWindow();        
        }

        private void writeFeaturesToLog(List<string> _featureSelected)
        {
            rtbLogViewer.AppendText("\n -----------------------*****************Features Selected*****************-----------------------");
            rtbLogViewer.AppendText("\n" + string.Join(",   ", _featureSelected));
            rtbLogViewer.AppendText("\n -----------------------*****************Features Selected*****************-----------------------");
            rtbLogViewer.ScrollToEnd();
        }

        private async Task<int> ProcessTextClassification(string location)
        {
            if (File.Exists(location + "/SelectedFeatures.txt"))
            {
                File.Delete(location + "/SelectedFeatures.txt");
            }
            //Trigger Feature selection somehow
            _processingUp = true;
            await TrgiggerFeatureSelection(location, FinishEventTrigger);
            return 1;
        }

        private void FinishEventTrigger()
        {
            _processingUp = false;
            if (!Dispatcher.CheckAccess())
            {
                this.Dispatcher.BeginInvoke(new Action(delegate () { FinishEventTrigger(); }), DispatcherPriority.Normal);
            }
            else
            {
                RetreiveFeatures(_dataSetLocation);
            }
        }

        private List<string> RetreiveFeatures(string location)
        {
            if (!File.Exists(location + "/SelectedFeatures.txt"))
            {
                rtbLogViewer.AppendText("\n Could not generate features....see above log to check failure.");
                return null;
            }
            _featureSelected = File.ReadAllText(location + "/SelectedFeatures.txt").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            writeFeaturesToLog(_featureSelected);
            return _featureSelected;
        }

        private async Task<int> TrgiggerFeatureSelection(string location, Action FinishEventTrigger)
        {
            var featureCount = 20;
            if (int.TryParse(txtFeatureCount.Text, out featureCount))
            {
                featureCount = 20;
            }
            JavaProcessFeatureSelectorService service = new JavaProcessFeatureSelectorService();
            service.DatasetLocation = location;
            service.FeatureCount = featureCount;
            service.ALevel = 0.7;
            //service.StandardOuputFunction = WriteToLogBox;
            service.FinishEventTrigger = FinishEventTrigger;
            service.handler = ProcessOutputDataRecievedHandler;
            await rtbLogViewer.Dispatcher.BeginInvoke(
                new Action(delegate () { DispatcherCallbackForProcessOutput(); }),
                DispatcherPriority.Normal);
            var a = await service.Execute();
            return 4;
        }
        private StringBuilder sb = new StringBuilder();
        private static object sharedInstance;
        public static object SharedInstance
        {
            get
            {
                if (sharedInstance == null)
                    Interlocked.CompareExchange(ref sharedInstance, new object(), null);
                return sharedInstance;
            }
        }
        void ProcessOutputDataRecievedHandler(object sender, DataReceivedEventArgs e)
        {
            lock (SharedInstance)
            {
                if (!string.IsNullOrEmpty(e.Data))
                {
                    sb.Append(e.Data ?? string.Empty);
                }
            }
        }

        void DispatcherCallbackForProcessOutput()
        {
            //Trace.WriteLine(e.Data);
            if (rtbLogViewer.Dispatcher.CheckAccess())
            {
                lock (SharedInstance)
                {
                    var str = sb.ToString();
                    if (!string.IsNullOrWhiteSpace(str))
                    {
                        rtbLogViewer.AppendText(Environment.NewLine + str);
                        sb.Clear();
                        rtbLogViewer.ScrollToEnd();
                    }
                }
                //rtbLogViewer.Dispatcher.BeginInvoke(handler);
                if (_processingUp)
                {
                    rtbLogViewer.Dispatcher.BeginInvoke(
                        new Action(delegate () { DispatcherCallbackForProcessOutput(); }), DispatcherPriority.SystemIdle);
                }
            }
            //else
            //{
            //    rtbLogViewer.AppendText(e.Data ?? string.Empty);
            //}                  
        }

        //private void WriteToLogBox(StringBuilder sb)
        //{
        //    rtbLogViewer.AppendText(System.Environment.NewLine + sb.ToString());
        //    rtbLogViewer.ScrollToEnd();
        //}

        private void OpenTextClassficationWindow()
        {
            if (string.IsNullOrWhiteSpace(_dataSetLocation))
            {
                System.Windows.Forms.MessageBox.Show("Please select the dataset first!!!");
                return;
            }
            TextClassificationWindow form = new TextClassificationWindow(_dataSetLocation, _featureSelected);

            WindowInteropHelper wih = new WindowInteropHelper(this);
            wih.Owner = form.Handle;
            form.Show();
            this.Hide();

        }

        private void lnkResetAll_Click(object sender, RoutedEventArgs e)
        {
            _dataSetLocation = string.Empty;
            _featureSelected.Clear();
            btnSelectDataset.Visibility = Visibility.Visible;
            btnSelectFeature.Visibility = Visibility.Visible;
            btnSelectDataset.IsEnabled = true;
            btnSelectFeature.IsEnabled = true;
            changeDatasetLinkBlock.Visibility = Visibility.Hidden;
            cbxAlgo.IsEnabled = false;
            btnSelectFeature.IsEnabled = false;
            lblDataSet.Content = string.Empty;
        }

        private void lnkswitchToTextClassification_Click(object sender, RoutedEventArgs e)
        {
            OpenTextClassficationWindow();
        }
    }
}
