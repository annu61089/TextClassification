using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextClassification.Text_Classification
{
    public class TextClassificationResult
    {
        public string DocumentName { get; set; }

        public string DocumentPath { get; set; }

        public string ExpectedClass { get; set; }

        public string FoundClass { get; set; }
    }
}
