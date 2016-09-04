using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using GenericBackend.Excel.Generic;
using GenericBackend.Excel.Structures;
using System;
using GenericBackend.Core.Extensions;

namespace GenericBackend.Excel.Sheets
{
    //Strategy pattern
    public abstract class BaseSheet
    {
        private readonly Sheet _sheet;
        private readonly WorksheetPart _worksheetPart;

        protected readonly WorkbookPart WorkbookPart;

        protected BaseSheet(Sheet sheet, WorkbookPart workbookPart, WorksheetPart worksheetPart)
        {
            _sheet = sheet;
            WorkbookPart = workbookPart;
            _worksheetPart = worksheetPart;
        }

        protected MongoSheetData GetStructure(string name, IEnumerable<Row> rows, ICollection<int> years,
            ICollection<int> monthes)
        {
            return new MongoSheetData
            {
                Name = name.ToLowerInvariant(),
                Years = years,
                Monthes = monthes,
                Elements = GetElements(rows.ToArray())
            };
        }

        protected IEnumerable<SheetItem> GetElements(ICollection<Row> rows)
        {
            var nameCells = GetCellsFrom(rows, DataStartIndex, TitlesIndex).Take(Step).Select(x => GeneralParsing.GetCellValue(WorkbookPart, x)).ToArray();

            var elements = new List<SheetItem>();

            foreach (var source in rows.Skip(ItemStartIndex))
            {
                elements.Add(GetSheetItem(source, nameCells, NameIndex, DataStartIndex, Step));
            }

            return elements;
        }

        protected abstract int DataStartIndex { get; }
        protected abstract int Step { get; }
        protected abstract int ItemStartIndex { get; }
        protected abstract int TitlesIndex { get; }
        protected abstract int NameIndex { get; }

        protected ICollection<int> ParseYears(IEnumerable<Row> rows)
        {
            var cellsData =
                GetYearCells(rows)
                    .Skip(DataStartIndex)
                    .Select(x => GeneralParsing.GetCellValue(WorkbookPart, x))
                    .ToArray();

            var knownCell = cellsData[0];

            for (var i = 1; i < cellsData.Length; i++)
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

            return cellsData.Select(x => DateTime.FromOADate(int.Parse(x)).Year).Where((x, i) => (i % Step) == 0).ToArray();
        }

        protected ICollection<int> ParseMonthes(IEnumerable<Row> rows)
        {
            return GetMonthInts(GetMonthCells(rows)).Select(x => DateTime.FromOADate(x).Month).Where((x, i) => (i % Step) == 0).ToArray();
        }

        protected static IEnumerable<Cell> GetCellsFrom(IEnumerable<Row> rows, int startIndex, int rowIndex)
        {
            return GetCellFromRow(rows.Skip(rowIndex).First(), startIndex).ToArray();
        }

        protected static IEnumerable<Cell> GetCellFromRow(OpenXmlElement row, int startIndex)
        {
            return row.Descendants<Cell>().Skip(startIndex);
        }

        protected SheetItem GetSheetItem(OpenXmlElement source, IEnumerable<string> nameCells, int nameIndex, int dataStartIndex, int step)
        {
            var cells = GetCellFromRow(source, 0).ToArray();

            var name = GeneralParsing.GetCellValue(WorkbookPart, cells.Skip(nameIndex).First());
            var data = nameCells.Select(
                (x, i) =>
                    new
                    {
                        Name = x,
                        Data =
                            cells.Skip(dataStartIndex)
                                .Where((y, j) => j % step == i)
                                .Select(z => GeneralParsing.GetCellValue(WorkbookPart, z))
                                .ToArray()
                    }).ToDictionary(dataKey => dataKey.Name, dataCheck => dataCheck.Data);


            var item = new SheetItem
            {
                Name = name,
                Data = data
            };
            return item;
        }

        public MongoSheetData Parse()
        {

            var name = _sheet.Name.Value.ToLower();
            
            var sheetData = _worksheetPart.Worksheet.ChildElements.First<SheetData>();

            var rows = sheetData.Elements<Row>().ToArray();

            var years = ParseYears(rows);
            var monthes = ParseMonthes(rows);

            return GetStructure(name, rows, years, monthes);    
        }

        #region Private Part

        private static IEnumerable<Cell> GetYearCells(IEnumerable<Row> rows)
        {
            return rows.First().Descendants<Cell>().ToArray();
        }

        private static IEnumerable<Cell> GetMonthCells(IEnumerable<Row> rows)
        {
            return rows.First().Descendants<Cell>().ToArray();
        }

        private IEnumerable<int> GetMonthInts(IEnumerable<Cell> cells)
        {
            var cellsData =
                cells.Skip(DataStartIndex).Select(x => GeneralParsing.GetCellValue(WorkbookPart, x)).ToArray();

            var knownCell = cellsData[0];

            for (var i = 1; i < cellsData.Length; i++)
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
        #endregion
    }
}
