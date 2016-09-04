using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace GenericBackend.Excel.Sheets
{
    public class ActualSheet : BaseSheet
    {
        private const int ActualDataStartIndex = 16;
        private const int ActualStep = 5;
        private const int ActualItemStartIndex = 2;
        private const int ActualTitlesIndex = 1;
        private const int ActualNameIndex = 4;


        public ActualSheet(Sheet sheet, WorkbookPart workbookPart, WorksheetPart worksheetPart) : base(sheet, workbookPart, worksheetPart)
        {
        }

        protected override int DataStartIndex => ActualDataStartIndex;
        protected override int Step => ActualStep;
        protected override int ItemStartIndex => ActualItemStartIndex;
        protected override int TitlesIndex => ActualTitlesIndex;
        protected override int NameIndex => ActualNameIndex;
    }
}
