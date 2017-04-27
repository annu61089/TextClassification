using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextClassification.FeatureSelector
{
    public class JavaProcessFeatureSelectorService : ExecuterServiceModule.IExecuterService<int>
    {
        private const string featureSelectorJarFileName = "datumbox-framework-lib.jar";
        private const string featureSelectorProcessCallFormat = "-jar {0} {1} {2} {3}";
        public DataReceivedEventHandler handler;
        public Action FinishEventTrigger;
        private int lineCount = 0;
        public string DatasetLocation { get; set; }
        public int FeatureCount { get; set; }

        public StringBuilder outputString = new StringBuilder();
        public Action<StringBuilder> StandardOuputFunction;

        public double ALevel { get; set; }


        public async Task<int> Execute()
        {
            var process = await prepareProcessContext();
            await startProcessAsync(process);
            //process.WaitForExit();
            //process.Close();
            return 1;
        }

        private async Task<Process> prepareProcessContext()
        {
            string jarFilePath = @"C:\Users\anupams\imp\pers\Shared\" + featureSelectorJarFileName;
            string arguments = string.Format(featureSelectorProcessCallFormat, jarFilePath, DatasetLocation, FeatureCount, ALevel);
            Process process = new Process();
            process.StartInfo.FileName = "java.exe";
            process.StartInfo.Arguments = arguments;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.CreateNoWindow = true;
            process.OutputDataReceived += handler;
            process.ErrorDataReceived += handler;
            process.EnableRaisingEvents = true;
            //process.OutputDataReceived += new DataReceivedEventHandler((sender, e) =>
            //{
            //    // Prepend line numbers to each line of the output.
            //    if (!string.IsNullOrEmpty(e.Data))
            //    {
            //        lineCount++;

            //        outputString.Append("\n[" + lineCount + "]: " + e.Data);
            //    }
            //});
            process.Exited += (sender, args) =>
            {
                FinishEventTrigger();
                process.Dispose();
            };
            return process;
        }

        private async Task<int> startProcessAsync(Process process)
        {
            process.Start();

            // Asynchronously read the standard output of the spawned process. 
            // This raises OutputDataReceived events for each line of output.
            process.BeginOutputReadLine();
            //process.WaitForExit();

            // Write the redirected output to this application's window.
            //StandardOuputFunction?.Invoke(outputString);

            //process.WaitForExit();
            //process.Close();
            return 1;
        }
    }
}
