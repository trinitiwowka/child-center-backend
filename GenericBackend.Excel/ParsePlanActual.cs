using System;
using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using GenericBackend.Core.Extensions;
using GenericBackend.DataModels.Actual;
using GenericBackend.DataModels.Plan;
using GenericBackend.Excel.Generic;

namespace GenericBackend.Excel
{
    public class ParsePlanActual
    {
        public const string PlanSheetName = "plan";
        public const string ActualSheetName = "actual";
        private readonly string _docPath;

        public ParsePlanActual(string docPath)
        {
            _docPath = docPath;
        }

        public PlanSheet ParsePlanSheet()
        {
            var planSheet = new PlanSheet();
            using (var document = SpreadsheetDocument.Open(_docPath, true))
            {
                var sheet =
                    (Sheet)document.WorkbookPart.Workbook.GetFirstChild<DocumentFormat.OpenXml.Spreadsheet.Sheets>()
                        .ChildElements.First(x => x is Sheet && ((Sheet)x).Name.Value.Equals(PlanSheetName, StringComparison.CurrentCultureIgnoreCase));
                planSheet.Name = sheet.Name.Value.ToLower();
                var planItems = new List<PlanSheetItem>();
                
                var workSheetPart =
                    (WorksheetPart)document.WorkbookPart.GetPartById(sheet.Id);

                var sheetData = workSheetPart.Worksheet.ChildElements.First<SheetData>();
                
                var rows = sheetData.Elements<Row>().ToArray();

                var years = ParseYears(rows[0].Descendants<Cell>().ToArray(), document, 17);
                var monthes = ParseMonthes(rows[1].Descendants<Cell>().ToArray(), document, 17);
                
                foreach (var row in rows.Skip(3))
                {
                    var planItem = GetDataRow(row.Descendants<Cell>().ToArray(), document, years, monthes);
                    planItems.Add(planItem);
                    planSheet.PlanItems = planItems;
                }
            }
            
            return planSheet;
        }

        public ActualSheet ParseActualSheet()
        {
            var planSheet = new ActualSheet();
            using (var document = SpreadsheetDocument.Open(_docPath, true))
            {
                var sheet =
                    (Sheet)document.WorkbookPart.Workbook.GetFirstChild<DocumentFormat.OpenXml.Spreadsheet.Sheets>()
                        .ChildElements.First(x => x is Sheet && ((Sheet)x).Name.Value.Equals(ActualSheetName, StringComparison.CurrentCultureIgnoreCase));
                planSheet.Name = sheet.Name.Value.ToLower();
                var planItems = new List<ActualSheetItem>();

                var workSheetPart =
                    (WorksheetPart)document.WorkbookPart.GetPartById(sheet.Id);

                var sheetData = workSheetPart.Worksheet.ChildElements.First<SheetData>();

                var rows = sheetData.Elements<Row>().ToArray();

                var years = ParseActualYears(rows[0].Descendants<Cell>().ToArray(), document, 16);
                var monthes = ParseMonthes(rows[0].Descendants<Cell>().ToArray(), document, 16);

                foreach (var row in rows.Skip(2))
                {
                    var planItem = GetActualDataRow(row.Descendants<Cell>().ToArray(), document, years, monthes);
                    planItems.Add(planItem);
                    planSheet.ActualItems = planItems;
                }
            }

            return planSheet;
        }


        private static PlanSheetItem GetDataRow(IReadOnlyList<Cell> cells, SpreadsheetDocument document, ICollection<int> years, ICollection<int> monthes)
        {
            var planItem = new PlanSheetItem
            {
                Subject = GeneralParsing.GetCellValue(document.WorkbookPart, cells[4]),
                TimelineData = GetData(cells, document, 17, years, monthes)
            };

            return planItem;
        }


        private static ActualSheetItem GetActualDataRow(IReadOnlyList<Cell> cells, SpreadsheetDocument document, ICollection<int> years, ICollection<int> monthes)
        {
            var planItem = new ActualSheetItem
            {
                Subject = GeneralParsing.GetCellValue(document.WorkbookPart, cells[4]),
                TimelineData = GetActualData(cells, document, 16, years, monthes)
            };

            return planItem;
        }

        private static ICollection<PlanTimelineData> GetData(IReadOnlyList<Cell> cells, SpreadsheetDocument document, int startIndex, ICollection<int> years, ICollection<int> monthes)
        {
            var list = new List<PlanTimelineData>();

            for (var j = startIndex; j <= cells.Count - 3; j = j + 3)
            {
                var timeLine = new PlanTimelineData
                {
                    Year = years.ElementAt(j-startIndex),
                    Month = monthes.ElementAt(j-startIndex),
                    Plan = GeneralParsing.GetCellValue(document.WorkbookPart, cells[j]),
                    AccumulatedPlan = GeneralParsing.GetCellValue(document.WorkbookPart, cells[j + 1]),
                    SupervisorComments = GeneralParsing.GetCellValue(document.WorkbookPart, cells[j + 2])
                };

                list.Add(timeLine);
            }

            return list;
        }


        private static ICollection<ActualTimelineData> GetActualData(IReadOnlyList<Cell> cells, SpreadsheetDocument document, int startIndex, ICollection<int> years, ICollection<int> monthes)
        {
            var list = new List<ActualTimelineData>();

            for (var j = startIndex; j <= cells.Count - 5; j = j + 5)
            {
                var timeLine = new ActualTimelineData
                {
                    Year = years.ElementAt(j - startIndex),
                    Month = monthes.ElementAt(j - startIndex),
                    Actual = GeneralParsing.GetCellValue(document.WorkbookPart, cells[j]),
                    UpdateActual = GeneralParsing.GetCellValue(document.WorkbookPart, cells[j + 1]),
                    AccumulatedActual = GeneralParsing.GetCellValue(document.WorkbookPart, cells[j + 2]),
                    AccumulatedUpdate = GeneralParsing.GetCellValue(document.WorkbookPart, cells[j + 3]),
                    SupervisorComments = GeneralParsing.GetCellValue(document.WorkbookPart, cells[j + 4])
                };

                list.Add(timeLine);
            }

            return list;
        }

        private static ICollection<int> ParseActualYears(IEnumerable<Cell> cells, SpreadsheetDocument document, int startIndex)
        {
            return ParseYears(cells, document, startIndex).Select(x => DateTime.FromOADate(x).Year).ToArray();
        }

        private static ICollection<int> ParseMonthes(IEnumerable<Cell> cells, SpreadsheetDocument document, int startIndex)
        {
            return ParseYears(cells, document, startIndex).Select(x => DateTime.FromOADate(x).Month).ToArray();
        }

        private static ICollection<int> ParseYears(IEnumerable<Cell> cells, SpreadsheetDocument document, int startIndex)
        {
            var cellsData = cells.Skip(startIndex).Select(x => GeneralParsing.GetCellValue(document.WorkbookPart, x)).ToArray();

            var knownCell = cellsData[0];

            for(var i=1; i<cellsData.Length; i++)
            {
                if (cellsData[i].IsNullOrEmpty())
                {
                    cellsData[i] = knownCell;
                }
                else
                {
                    knownCell = cellsData[i];
                }
            }

            return cellsData.Select(int.Parse).ToArray();
        }

       
    }
}
