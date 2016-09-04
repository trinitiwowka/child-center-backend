using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace GenericBackend.Excel.Generic
{
    public class GeneralParsing
    {
        public static string GetSharedString(Cell theCell, SharedStringTablePart stringPart)
        {
            return string.Empty;
        }

        public static string GetCellValue(WorkbookPart wbPart, Cell theCell)
        {
            var value = string.Empty;
            if (theCell == null)
                return value;

            value = theCell.InnerText;

           
            if (theCell.DataType == null)
                return value;

            switch (theCell.DataType.Value)
            {
                case CellValues.SharedString:

                    
                    var stringTable =
                        wbPart.GetPartsOfType<SharedStringTablePart>()
                            .FirstOrDefault();

                    if (stringTable != null)
                    {
                        value =
                            stringTable.SharedStringTable
                                .ElementAt(int.Parse(value)).InnerText;
                    }
                    break;

                case CellValues.Boolean:
                    switch (value)
                    {
                        case "0":
                            value = "FALSE";
                            break;
                        default:
                            value = "TRUE";
                            break;
                    }
                    break;
            }
            return value;
        }
    }
}
