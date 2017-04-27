using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace TextClassification.ExecuterServiceModule
{
    public class OutlayerFetaureService : IExecuterService<int>
    {
        private string filePath { get; set; }

        public Dictionary<string, bool> OutlayerWithSyncStatus { get; set; }

        private static OutlayerFetaureService _outlayerFetaureService;
        private Timer timer;
        private int interval;

        private OutlayerFetaureService()
        {
            Execute();
        }

        public static OutlayerFetaureService GetInstance()
        {
            return _outlayerFetaureService ?? (_outlayerFetaureService = new OutlayerFetaureService());
        }

        private int Initialize()
        {
            filePath = "../../OutlayerFeatures/outlayer.txt";
            if(!Directory.Exists("../../OutlayerFeatures"))
            {
                Directory.CreateDirectory("../../OutlayerFeatures");
            }

            if (!File.Exists(filePath))
            {
                File.Create(filePath);
            }

            List<string> lines = File.ReadLines(filePath).ToList();
            if (OutlayerWithSyncStatus == null)
            {
                OutlayerWithSyncStatus = new Dictionary<string, bool>();    
            }
            
            OutlayerWithSyncStatus.Clear();
            lines.ForEach(line => OutlayerWithSyncStatus.Add(line.ToLower().Trim(), true));
            timer = new Timer {Enabled = true};
            interval = 5000;
            timer.Interval = interval;
            timer.Elapsed += TimerOnElapsed;
            timer.Start();
            return 1;
        }

        private void TimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            syncOutlayers();
        }

        private void syncOutlayers()
        {
            try
            {
                timer.Stop();
                var unsyncOutlayers = OutlayerWithSyncStatus.Where(x => !x.Value).Select(x => x.Key).ToList();
                unsyncOutlayers.ForEach(x => OutlayerWithSyncStatus[x] = true);
                File.AppendAllLines(filePath, unsyncOutlayers);
            }
            catch (Exception)
            {

            }
            finally
            {
                timer.Start();
            } 

        }
            

        public async Task<int> Execute()
        {
            return await Task.Run(() => Initialize());
        }
    }
}
