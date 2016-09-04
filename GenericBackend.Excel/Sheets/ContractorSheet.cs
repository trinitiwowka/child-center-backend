using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace GenericBackend.Excel.Sheets
{
    public class ContractorSheet : BaseSheet
    {
        private const int ContractorDataStartIndex = 16;
        private const int ContractorStep = 5;
        private const int ContractorItemStartIndex = 2;
        private const int ContractorTitlesIndex = 1;
        private const int ContractorNameIndex = 4;


        public ContractorSheet(Sheet sheet, WorkbookPart workbookPart, WorksheetPart worksheetPart)
            : base(sheet, workbookPart, worksheetPart)
        {

        }

        protected override int DataStartIndex => ContractorDataStartIndex;
        protected override int Step => ContractorStep;
        protected override int ItemStartIndex => ContractorItemStartIndex;
        protected override int TitlesIndex => ContractorTitlesIndex;
        protected override int NameIndex => ContractorNameIndex;
    }
}
