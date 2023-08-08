using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ML.PredictTags;
using static System.Net.Mime.MediaTypeNames;

namespace ML
{
    public static class ParseExcel
    {
        public static List<ModelInput> ImportFromXLSX(string filePath)
        {
            List<ModelInput> list = new List<ModelInput>();

            using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                // Auto-detect format, supports:
                //  - Binary Excel files (2.0-2003 format; *.xls)
                //  - OpenXml Excel files (2007 format; *.xlsx, *.xlsb)
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {

                    do
                    {
                        while (reader.Read())
                        {
                            ModelInput model = new()
                            {
                                Title = reader.GetString(0),
                                Body = reader.GetString(1),
                                Tags = reader.GetString(2),
                            };

                            list.Add(model);
                        }
                    } while (reader.NextResult());
                }

                return list;
            }
        }
    }
}
