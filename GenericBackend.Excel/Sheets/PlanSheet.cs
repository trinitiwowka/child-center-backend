using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace GenericBackend.Excel.Sheets
{
    public class PlanSheet : BaseSheet
    {
        private const int PlanDataStartIndex = 17;
        private const int PlanStep = 3;
        private const int PlanItemStartIndex = 3;
        private const int PlanTitlesIndex = 2;
        private const int PlanNameIndex = 4;


        public PlanSheet(Sheet sheet, WorkbookPart workbookPart, WorksheetPart worksheetPart)
            : base(sheet, workbookPart, worksheetPart)
        {

        }

        protected override int DataStartIndex => PlanDataStartIndex;
        protected override int Step => PlanStep;
        protected override int ItemStartIndex => PlanItemStartIndex;
        protected override int TitlesIndex => PlanTitlesIndex;
        protected override int NameIndex => PlanNameIndex;

    }
}
