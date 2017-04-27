using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextClassification.ExecuterServiceModule
{
    public class FeatureCountService : IExecuterService<int>
    {
        public Corpus DocCorpus { get; set; }
        private OutlayerFetaureService _outlayerFetaureService;
        private readonly string[] _seperators = {" ", ".", ">", ":", "\n", "\r", "\r\n"};
        private readonly int _minFeatueLength = 2;
        private readonly bool _extractFeatureToFile;
        private IList<string> _extractedFeatures;


        public FeatureCountService(OutlayerFetaureService outlayerFetaureService, bool extractFeatureToFile) : this(outlayerFetaureService)
        {
            _extractFeatureToFile = extractFeatureToFile;            
        }
        public FeatureCountService(OutlayerFetaureService outlayerFetaureService)
        {
            _outlayerFetaureService = outlayerFetaureService;            
        }

        public async Task<int> Execute()
        {   
            if(_extractFeatureToFile){
                ExtractTheFeatureToFile(DocCorpus.DocumentBase, true);
            }
            return await Task.Run(() => CountFeatureFromFiles(DocCorpus.DocumentBase));
        }

        private IList<string> ExtractTheFeatureToFile(List<string> list, bool writeToFile)
        {   
            if (_extractedFeatures == null)
            {
                var features = new List<string>();
                foreach (var file in list)
                {
                    features = File.ReadAllText(file)
                             .Split(_seperators, StringSplitOptions.RemoveEmptyEntries)
                             //.Select(x => Regex.Replace(strPara, @"\([0-9]\)", ""))
                             .ToList()
                             .Where(
                                     feature =>
                                         feature.Length >= _minFeatueLength &&
                                         !_outlayerFetaureService.OutlayerWithSyncStatus.Keys.Contains(feature.ToLower()))
                             .Distinct().ToList();
                }
                _extractedFeatures = features;
            }
            
            if (writeToFile)
            {
                var fileName = "../../OutlayerFeatures/" + "featureExtracted.txt";
                if (!Directory.Exists("../../OutlayerFeatures"))
                {
                    Directory.CreateDirectory("../../OutlayerFeatures");
                }
                File.Delete(fileName);
                File.Create(fileName).Close();
                File.AppendAllLines(fileName, _extractedFeatures);
            }
            return _extractedFeatures.Distinct().ToList();
        }

        private async Task<int> CountFeatureFromFiles(IEnumerable<string> files)
        {
            return ExtractTheFeatureToFile(files.ToList(), false).Count();
            //return files.Sum(file => 
            //            File.ReadAllText(file)
            //            .Split(_seperators, StringSplitOptions.RemoveEmptyEntries)
            //            .ToList()
            //            .Where(
            //                    feature => 
            //                        feature.Length >= _minFeatueLength && 
            //                        !_outlayerFetaureService.OutlayerWithSyncStatus.Keys.Contains(feature.ToLower()))
            //            .Distinct()
            //            .Count()
            //        );
        }

    }
}
