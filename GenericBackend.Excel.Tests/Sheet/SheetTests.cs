using System.Linq;
using GenericBackend.Excel.Factory;
using GenericBackend.Excel.Sheets;
using GenericBackend.Excel.Tests.Utils;
using NUnit.Framework;

namespace GenericBackend.Excel.Tests.Sheet
{
    [TestFixture]
    public class SheetTests
    {
        private SheetFactory _sheetFactory;
        
        [SetUp]
        public void PrepareSheetToTest()
        {
            _sheetFactory = new SheetFactory(FilesToTests.Actual);
        }

        [Test]
        public void PlanSheetHasCorrectStructure()
        {
            //given
            const string planName = "Plan";
            var planSheet = _sheetFactory.GetSheet(planName, (sheet, workbook, worksheet) => new PlanSheet(sheet, workbook, worksheet));

            //when
            var resultStructure = planSheet.Parse();


            //then
            Assert.That(resultStructure.Name, Is.EqualTo(planName.ToLowerInvariant()));
            Assert.That(resultStructure.Years.Count(), Is.EqualTo(resultStructure.Monthes.Count()));
            Assert.That(resultStructure.Elements.Any(), Is.True);
            foreach (var sheetItem in resultStructure.Elements)
            {
                foreach (var key in sheetItem.Data.Keys)
                {
                    Assert.That(sheetItem.Data[key].Count, Is.EqualTo(resultStructure.Monthes.Count()));
                }
            }
            
        }

        [Test]
        public void ActualSheetHasCorrectStructure()
        {
            //given
            const string planName = "Actual";
            var planSheet = _sheetFactory.GetSheet(planName, (sheet, workbook, worksheet) => new ActualSheet(sheet, workbook, worksheet));

            //when
            var resultStructure = planSheet.Parse();


            //then
            Assert.That(resultStructure.Name, Is.EqualTo(planName.ToLowerInvariant()));
            Assert.That(resultStructure.Years.Count(), Is.EqualTo(resultStructure.Monthes.Count()));
            Assert.That(resultStructure.Elements.Any(), Is.True);
            foreach (var sheetItem in resultStructure.Elements)
            {
                foreach (var key in sheetItem.Data.Keys)
                {
                    Assert.That(sheetItem.Data[key].Count, Is.EqualTo(resultStructure.Monthes.Count()));
                }
            }

        }

        [Test]
        public void ActualAndUpdateSheetHasCorrectStructure()
        {
            //given
            const string planName = "ACTUAL +Update Actual";
            var planSheet = _sheetFactory.GetSheet(planName, (sheet, workbook, worksheet) => new ActualSheet(sheet, workbook, worksheet));

            //when
            var resultStructure = planSheet.Parse();


            //then
            Assert.That(resultStructure.Name, Is.EqualTo(planName.ToLowerInvariant()));
            Assert.That(resultStructure.Years.Count(), Is.EqualTo(resultStructure.Monthes.Count()));
            Assert.That(resultStructure.Elements.Any(), Is.True);
            foreach (var sheetItem in resultStructure.Elements)
            {
                foreach (var key in sheetItem.Data.Keys)
                {
                    Assert.That(sheetItem.Data[key].Count, Is.EqualTo(resultStructure.Monthes.Count()));
                }
            }

        }

        [Test]
        public void ContractorSheetHasCorrectStructure()
        {
            //given
            const string planName = "test";
            var planSheet = _sheetFactory.GetSheet(planName, (sheet, workbook, worksheet) => new ActualSheet(sheet, workbook, worksheet));

            //when
            var resultStructure = planSheet.Parse();


            //then
            Assert.That(resultStructure.Name, Is.EqualTo(planName.ToLowerInvariant()));
            Assert.That(resultStructure.Years.Count(), Is.EqualTo(resultStructure.Monthes.Count()));
            Assert.That(resultStructure.Elements.Any(), Is.True);
            foreach (var sheetItem in resultStructure.Elements)
            {
                foreach (var key in sheetItem.Data.Keys)
                {
                    Assert.That(sheetItem.Data[key].Count, Is.EqualTo(resultStructure.Monthes.Count()));
                }
            }

        }

    }
}
