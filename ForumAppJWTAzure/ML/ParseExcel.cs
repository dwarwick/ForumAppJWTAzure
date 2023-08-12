using Ganss.Excel;
using static ML.PredictTags;

namespace ML
{
    public static class ParseExcel
    {
        public static List<ModelInput> ImportFromXLSX(string filePath)
        {
            IEnumerable<ModelInput> list = new List<ModelInput>();

            list = new ExcelMapper(filePath).Fetch<ModelInput>();

            return list.ToList();            
        }

        public static void OutputToXLSX(string filePath, List<ModelInput> list)
        {
            new ExcelMapper().Save(filePath, list, "Results");
        }
    }
}
