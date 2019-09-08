using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Microsoft.ML;
using Microsoft.ML.AutoML;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms.Image;
using System.Diagnostics;
namespace ImageRecreator
{
    // https://github.com/dotnet/machinelearning-samples/blob/master/samples/csharp/end-to-end-apps/ObjectDetection-Onnx/OnnxObjectDetection/ML/OnnxModelConfigurator.cs
    // https://github.com/dotnet/machinelearning-samples/tree/master/samples/csharp/getting-started/Regression_TaxiFarePrediction
    public class CreateTrainingSet
    {

        // using lin

        public static void Create()
        {
            var context = new MLContext();

        https://github.com/dotnet/machinelearning/issues/164
            // var img = new Bitmap(null);
            // var vectorSizes = new Dictionary<string, float[,]>();
            // vectorSizes.Add("Features", f);
            // new float[0, 0]
            /*
            var f = new float[2, 2];
            f[0, 0] = 0;
            f[0, 1] = 1;
            f[1, 0] = 2;
            f[1, 1] = 3;
            */
            var f = new float[2];
            f[0] = 0;
            f[1] = 1;
            /*
            var img = new Bitmap(null);
            var pixel = img.GetPixel(2, 3);
            */
            // Color.FromArgb()
            // creating schema https://github.com/dotnet/machinelearning/issues/164
            // var schemaDef = SchemaDefinition.Create(typeof(ImageInput));
            /*
            schemaDef["Features"].ColumnType = new VectorDataViewType(NumberDataViewType.Single, 10)
            schemaDef["Label"].ColumnType = new VectorDataViewType(NumberDataViewType.Single, 10);
            */
            /*
            schemaDef["Features"].ColumnType = new VectorDataViewType(NumberDataViewType.Single, 2);
            schemaDef["Label"].ColumnType = new VectorDataViewType(NumberDataViewType.Single, 2);
            */
            /*
            var pipeline = context.Transforms.

            var trainingDataView = context.Data.LoadFromEnumerable<ImageInput>(new ImageInput[] { });
            // var trainer = context.;
            var model = pipeline.Fit(trainingDataView);

            var predictionEngine = context.Model.CreatePredictionEngine<Data, OutputData>(model);
            */
            //  var trainedModel = trainingPipeline.Fit(trainingDataView);
            // context.Model.Save(trainedModel, trainingDataView.Schema, "./mymodel.zip");
            /* crash */
            // using automl api https://docs.microsoft.com/en-us/dotnet/machine-learning/how-to-guides/how-to-use-the-automl-api
            /*
            var settings = new RegressionExperimentSettings();
            settings.MaxExperimentTimeInSeconds = 5 * 60;
            var cancel = new System.Threading.CancellationTokenSource();
            settings.CancellationToken = cancel.Token;
            RegressionExperiment reggresion = context.Auto().CreateRegressionExperiment(settings);
            var result = reggresion.Execute(trainingDataview);
            
            Debug.WriteLine("metrics best run: " + result.BestRun.ValidationMetrics);
            */

        }
        void CancelOnKeyPressed(System.Threading.CancellationTokenSource cancel) {
            Console.CancelKeyPress += (object sender, ConsoleCancelEventArgs e) =>
            {
                System.Diagnostics.Debug.WriteLine("cancelling");
                // cancel.Cancel();
            };
        }
    
    }
}



// if using model that has many dynamic number or columns: https://stackoverflow.com/questions/52822696/dynamic-training-test-classes-with-ml-net
// https://docs.microsoft.com/en-us/dotnet/api/microsoft.ml.imageestimatorscatalog.extractpixels?view=ml-dotnet
// https://docs.microsoft.com/en-us/dotnet/machine-learning/resources/transforms