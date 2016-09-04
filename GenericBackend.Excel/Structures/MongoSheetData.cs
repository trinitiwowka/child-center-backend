using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericBackend.Excel.Structures
{
    public class MongoSheetData
    {
        public string Name { get; set; }
        public IEnumerable<int> Years { get; set; }
        public IEnumerable<int> Monthes { get; set; }
        public IEnumerable<SheetItem> Elements { get; set; }

    }

    public class SheetItem
    {
        public string Name { get; set; }
        public Dictionary<string, string[]> Data { get; set; }
    }
}
