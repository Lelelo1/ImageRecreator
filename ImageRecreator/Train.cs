using Microsoft.ML;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Diagnostics;
using Microsoft.ML.AutoML;
using Microsoft.ML.Data;
using System.Linq;
using System.Drawing.Drawing2D;
namespace ImageRecreator
{
    public class Train
    {
        // creats model
        public static void Run(List<Bitmap> originalImages)
        {
            
            var imageData = new List<Data>();
            foreach(var original in originalImages)
            {
                // 0.001f takes 2 min
                imageData.AddRange(ImageData(original, original.LowQualityImages(2)));
            }
            /*
            for (int i = 0; i < imageData.Count; i += 100000)
            {
                imageData[i].Print();
            }
            */

            var mlContext = new MLContext();
            /* does not matter
            var schema = SchemaDefinition.Create(typeof(Data));
            schema["Total"].ColumnType = new VectorDataViewType(NumberDataViewType.Single);
            schema["Index"].ColumnType = new VectorDataViewType(NumberDataViewType.Single);
            schema["Value"].ColumnType = NumberDataViewType.Single;
            schema["Label"].ColumnType = NumberDataViewType.Single;
            */
            // varvector complaint

            Debug.WriteLine("imageData count: " + imageData.Count);

            // AutoML(imageData);
            imageData = imageData.OrderBy(a => new Guid()).ToList();
            var partialImageData = new List<Data>();

            var amountToTake = imageData.Count / 100;

            for(int i = 0; i < amountToTake; i++)
            {
                partialImageData.Add(imageData[i]);
            }

            Debug.WriteLine("using " + partialImageData.Count + " of imageData");

            var trainingDataView = mlContext.Data.LoadFromEnumerable<Data>(partialImageData);
            Debug.WriteLine("starting training");
            /*
            var dataProccessPipeline = mlContext.Transforms.CopyColumns("Label", "Label")
                .Append(mlContext.Transforms.Concatenate("Features","Average", "X", "Y", "Value"));
            */
            var dataProccessPipeline = mlContext.Transforms.Concatenate("Features", "Average", "X", "Y", "Value")
                .Append(mlContext.Transforms.SelectColumns("Features", "Label"));

            var trainer = mlContext.Regression.Trainers.Sdca(labelColumnName: "Label", featureColumnName: "Features");

            var trainingPipeline = dataProccessPipeline.Append(trainer);

            var model = trainingPipeline.Fit(trainingDataView);

            Debug.WriteLine("training complete - saving model");
            mlContext.Model.Save(model, trainingDataView.Schema, "./mymodel.zip");
            
            
        }
        static List<Data> ImageData(Bitmap original, List<Bitmap> lowImages, float percentPixelsToTake = 1)
        {
            var imageData = new List<Data>();
            // Debug.WriteLine("original: " + original.Name());
            Debug.WriteLine("original");
            
            for (int i = 0; i < lowImages.Count; i ++)
            {
                // lowImages[i].Name(original.Name() + "q" + i); // for test purpose
                imageData.AddRange(CreateDataFrom(original, lowImages[i]));
            }
            
            Debug.WriteLine("created imageData: " + imageData.Count + " = " + lowImages.Count + " * " + " width: " + original.Width + " * " + " height: " + original.Height);
            return imageData;
        }

        static List<Data> CreateDataFrom(Bitmap original, Bitmap lowImage)
        {
    
            Debug.WriteLine("Creating data...");
            var imageData = new List<Data>();
            var averageColor = original.GetAverageColor();
            var average = new byte[4] { averageColor.R, averageColor.G, averageColor.B, averageColor.A }.toInt();
            for (int x = 0; x < lowImage.Width; x ++)
            {
                for (int y = 0; y < lowImage.Height; y++ )
                {
                    var valueColor = lowImage.GetPixel(x, y);
                    var originalColor = original.GetPixel(x, y);
                    var value = new byte[4] { valueColor.R, valueColor.G, valueColor.B, valueColor.A }.toInt();
                    var originalValue = new byte[4] { originalColor.R, originalColor.G, originalColor.B, originalColor.A }.toInt();
                    /*
                    Debug.WriteLine("average: " + average);
                    Debug.WriteLine("value:" + value);
                    Debug.WriteLine("original: " + originalValue);
                    */
                    // Debug.WriteLine("average: " + average + ", value: " + value + ", original: " + originalValue);
                    var data = new Data()
                    {
                        average = average,
   
                        value = value,

                        x = x,
                        y = y,

                        original = originalValue,
                    };
                    imageData.Add(data);
                    data = null;
                }
            }
            return imageData;
        }


        static void AutoML(List<Data> imageData)
        {
            var mlContext = new MLContext();
            var trainingDataView = mlContext.Data.LoadFromEnumerable<Data>(imageData);

            var settings = new RegressionExperimentSettings();
            settings.MaxExperimentTimeInSeconds = 5 * 60;

            var experiment = mlContext.Auto().CreateRegressionExperiment(settings);

            var result = experiment.Execute(trainingDataView);

            RegressionMetrics metrics = result.BestRun.ValidationMetrics;
            
        }

        private static Random random = new Random();
        static int RandomizedIndex(int length)
        {

            int r = random.Next(0, length - 1);
  
            return r;
        }
        //static Pixel Pixel()
        static void Data()
        {

        }
        // make train already existing model

         /* not having the supported data types */
        static void RunAutoML(List<Data> imageData)
        {
            var mlContext = new MLContext();
            var trainDataView = mlContext.Data.LoadFromEnumerable<Data>(imageData);

            // automl experiment
            var settings = new RegressionExperimentSettings();
            settings.MaxExperimentTimeInSeconds = 10 * 60;
            var experiment = mlContext.Auto().CreateRegressionExperiment(settings);
            Debug.WriteLine("Running expiriment");
            var result = experiment.Execute(trainDataView);
            result.Print();
        }
    }
}
