// See https://aka.ms/new-console-template for more information
using Microsoft.ML;
using ML;
using static ML.PredictTags;
using static System.Runtime.InteropServices.JavaScript.JSType;

Console.WriteLine("Hello, World!");
System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

MLContext mlContext = new MLContext();

string inputFileName = @"C:\Users\bgmsd\Downloads\QueryResults (1).xlsx";



// 1 based tag number. Must be 1 or greater.
int tagNumber = 5;

string outputFileName = @$"C:\Users\bgmsd\Downloads\QueryResults (1)_output_{tagNumber}.xlsx";

IDataView dataView = PreProcessor.LoadDataToList(inputFileName, outputFileName, tagNumber);

ITransformer model = PredictTags.RetrainPipeline(mlContext, dataView);

// Save Trained Model

Console.WriteLine($"{DateTime.Now.ToLongTimeString()} Saving model");
mlContext.Model.Save(model, dataView.Schema, $"model_{tagNumber}.zip");