using ForumAppJWTAzure.Shared.Helpers;
using ForumAppJWTAzure.Shared.Models;
using HtmlAgilityPack;
using Microsoft.ML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace ML
{

    public static class PreProcessor
    {
        public static IDataView LoadDataToList(string XlsxPath, string outputXlsxPath, int tagNumber)
        {
            //Create MLContext
            MLContext mlContext = new MLContext();

            Console.WriteLine($"{DateTime.Now.ToLongTimeString()} Importing data");
            List<ModelInput> list = ParseExcel.ImportFromXLSX(XlsxPath);
            


            Console.WriteLine($"{DateTime.Now.ToLongTimeString()} Removing pre nodes");
            list = HtmlHelpers.RemovePreNodes(list);

            Console.WriteLine($"{DateTime.Now.ToLongTimeString()} Removing tags");
            list = RemoveAllButFirstTag(list, tagNumber);

            Console.WriteLine($"{DateTime.Now.ToLongTimeString()} Outputing to XLSX: {list.Count} rows");
            ParseExcel.OutputToXLSX(outputXlsxPath, list);

            //Load Data
            Console.WriteLine($"{DateTime.Now.ToLongTimeString()} Creating IDataview");
            IDataView data = mlContext.Data.LoadFromEnumerable<ModelInput>(list);

            return data;
        }        

        private static List<ModelInput> RemoveAllButFirstTag(List<ModelInput> list, int tagNumber)
        {
            List<ModelInput> output = new();

            foreach (ModelInput input in list)
            {
                string step1 = input.Tags.Replace("><", " ");
                string step2 = step1.Replace("<", " ");
                string step3 = step2.Replace(">", " ").TrimStart().TrimEnd();

                string[] tags = step3.Split(" ");

                if (tags.Length < tagNumber) continue;

                string tag = tags[tagNumber - 1];

                ModelInput outputRow = new()
                {
                    Title = input.Title,
                    Body = input.Body,
                    Tags = tag,
                };

                output.Add(outputRow);
            }
            return output;
        }
    }
}
