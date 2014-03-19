using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyImporter
{
    public class ClassOpenExchangeJsonData
    {
        public string disclaimer
        {
            get;
            set;
        }

        public string license
        {
            get;
            set;
        }

        public string timestamp
        {
            get;
            set;
        }

        public IDictionary<string, string> rates { get; set; }
        
    }
}
