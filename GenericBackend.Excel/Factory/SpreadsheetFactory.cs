using System;
using System.Linq;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace GenericBackend.Excel.Factory
{
    public class SheetFactory
    {
        private readonly string _docPath;
        private SpreadsheetDocument _doc;

        public SheetFactory(string path)
        {
            _docPath = path;
        }

        private SpreadsheetDocument GetDocument()
        {
            _doc = _doc ?? SpreadsheetDocument.Open(_docPath, false);

            return _doc;
        }

        public T GetSheet<T>(string sheetName, Func<Sheet, WorkbookPart, WorksheetPart, T> createSheet)
        {
            var document = GetDocument();
            

            var sheet =
                (Sheet) document.WorkbookPart.Workbook.GetFirstChild<DocumentFormat.OpenXml.Spreadsheet.Sheets>()
                    .ChildElements.First(
                        x =>
                            x is Sheet &&
                            ((Sheet) x).Name.Value.Equals(sheetName, StringComparison.CurrentCultureIgnoreCase));

            var workSheetPart =
                (WorksheetPart) document.WorkbookPart.GetPartById(sheet.Id);

            return createSheet(sheet, document.WorkbookPart, workSheetPart);
            

        }

        
    }
}
