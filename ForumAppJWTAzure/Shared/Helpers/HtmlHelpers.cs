using ForumAppJWTAzure.Shared.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForumAppJWTAzure.Shared.Helpers
{
    public static class HtmlHelpers
    {
        public static List<ModelInput> RemovePreNodes(List<ModelInput> list)
        {
            List<ModelInput> output = new();

            foreach (ModelInput input in list)
            {
                if (input.Body == null) continue;

                var htmlDoc = new HtmlDocument();

                htmlDoc.LoadHtml(input.Body);

                HtmlNodeCollection preNodes = htmlDoc.DocumentNode.SelectNodes("//pre");

                if (preNodes != null)
                {
                    foreach (HtmlNode preNode in preNodes)
                    {
                        preNode.Remove();
                    }
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
    }
}
