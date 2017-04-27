using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextClassification.Logger
{
    public class LogWriter
    {
        public async Task<int> Log(string strLog, Func<string, string, int> loggerViewWriter)
        {
            return await LogIntoView(strLog, loggerViewWriter, DateTime.Now);
        }

        private async Task<int> LogIntoView(string log, Func<string, string, int> loggerViewWriter, DateTime timeStamp)
        {
            return loggerViewWriter(timeStamp.ToString(), log);
        }

        //private async Task<int> CountFeatureFromFiles(IEnumerable<string> files)
        //{
        //    return files.Sum(file => File.ReadAllText(file).Length);
        //}
    }
}
