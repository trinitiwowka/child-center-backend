using System;

namespace GenericBackend.Excel.Tests.Utils
{
    public static class FilesToTests
    {

        public const string PlanActualFileName = @"FilesToParse\PlanActual.xlsx";

        public static string Actual => AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"..\..\..\" + PlanActualFileName;
    }
}
