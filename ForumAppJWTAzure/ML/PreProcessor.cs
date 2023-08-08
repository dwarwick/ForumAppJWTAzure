using HtmlAgilityPack;
using Microsoft.ML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using static ML.PredictTags;
using static System.Net.Mime.MediaTypeNames;

namespace ML
{

    public static class PreProcessor
    {
        public static IDataView LoadDataToList(string XlsxPath)
        {
            //Create MLContext
            MLContext mlContext = new MLContext();

            List<ModelInput> list = ParseExcel.ImportFromXLSX(XlsxPath);

            list = RemovePreNodes(list);

            list = RemoveAllButFirstTag(list);


            //Load Data           
            IDataView data = mlContext.Data.LoadFromEnumerable<ModelInput>(list);

            return data;
        }




        public static List<ModelInput> RemovePreNodes(List<ModelInput> list)
        {
            List<ModelInput> output = new();

            foreach (ModelInput input in list)
            {
                if (input.Body == null) continue;

                var htmlDoc = new HtmlDocument();

                htmlDoc.LoadHtml(input.Body);

                HtmlNodeCollection preNodes = htmlDoc.DocumentNode.SelectNodes("//pre");

                if (preNodes == null) continue;

                foreach (HtmlNode preNode in preNodes)
                {
                    preNode.Remove();
                }

                ModelInput outputRow = new()
                {
                    Title = input.Title,
                    Body = htmlDoc.DocumentNode.InnerText,
                    Tags = input.Tags,
                };

                output.Add(outputRow);
            }
            return output;
        }

        private static List<ModelInput> RemoveAllButFirstTag(List<ModelInput> list)
        {
            List<ModelInput> output = new();

            foreach (ModelInput input in list)
            {
                string step1 = input.Tags.Replace("><", " ");
                string step2 = step1.Replace("<", " ");
                string step3 = step2.Replace(">", " ").TrimStart().TrimEnd();

                string tag = step3.Split(" ")[0];

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
