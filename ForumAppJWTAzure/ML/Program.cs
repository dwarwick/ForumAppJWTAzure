// See https://aka.ms/new-console-template for more information
using Microsoft.ML;
using ML;
using static ML.PredictTags;
using static System.Runtime.InteropServices.JavaScript.JSType;

Console.WriteLine("Hello, World!");
System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

//List<ModelInput> list = ParseExcel.ImportFromXLSX(@"C:\Users\bgmsd\Downloads\QueryResults (1).xlsx");

MLContext mlContext = new MLContext();

IDataView dataView = PreProcessor.LoadDataToList(@"C:\Users\bgmsd\Downloads\QueryResults (1).xlsx");
//IDataView dataView = PreProcessor.LoadData();

ITransformer model = PredictTags.RetrainPipeline(mlContext, dataView);

// Save Trained Model
mlContext.Model.Save(model, dataView.Schema, "model.zip");