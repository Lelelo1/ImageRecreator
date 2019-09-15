using Microsoft.ML;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;

namespace ImageRecreator
{
    public static class ConsumeExtensions
    {
        public static Bitmap Restore(this Bitmap lowQuality, Bitmap original = null)
        {

            var dataArray = lowQuality.To2DDataArray();

            var width = lowQuality.Width;
            var height = lowQuality.Height;
            var predictedBitmap = new Bitmap(width, height);

            var mlContext = new MLContext();
            DataViewSchema predictionPipelineSchema;
            ITransformer predictionPipeline = mlContext.Model.Load("./mymodel.zip", out predictionPipelineSchema);
            PredictionEngine<Data, OutputData> predictionEngine = mlContext.Model.CreatePredictionEngine<Data, OutputData>(predictionPipeline);

            Debug.WriteLine("Predicting and constructing bitmap...");
            for (int x = 300; x < width; x++)
            {
                for (int y = 200; y < height; y++)
                {
                    var data = dataArray[x, y];
                    var output = predictionEngine.Predict(data);

                    // just to to log comparison
                    var originalColor = original.GetPixel(x, y);
                    var originalColorValue = new byte[4] { originalColor.R, originalColor.G, originalColor.B, originalColor.A }.toInt();
                    /*
                    if (original == null)
                    {
                        Debug.WriteLine("value: " + data.value + " -> " + " output: " + output.outputValue);
                    }
                    else
                    {
                        Debug.WriteLine("value: " + (int)data.value + " -> " + " output: "
                            + (int)output.outputValue + ". original: " + (int)originalColorValue);
                    }
                    */
                    var byteArray = BitConverter.GetBytes(output.outputValue);

                    predictedBitmap.SetPixel(x, y, Color.FromArgb(byteArray[3] ,byteArray[0], byteArray[1], byteArray[2]));
                }
            }
            Debug.WriteLine("completed");
            return predictedBitmap;
            //  PredictionEngine<Data, OutputData> predictionEngine = mlContext.Model.CreatePredictionEngine<Data, OutputData>(predictionPipeline);


            // var output = predictionEngine.Predict(new Data { low = new float[] { 0.1f, 0.1f }, original = new float[] { 2f, 2f } });

            // Console.WriteLine("prediction: " + output.predicted + " with score: " + output.score);
        }

        static Data[,] To2DDataArray(this Bitmap lowQuality)
        {
            Debug.WriteLine("turning lowQuality image " + lowQuality.Name() + " into list of data");
            var width = lowQuality.Width;
            var height = lowQuality.Height;

            var dataArray = new Data[width, height];

            var averageColor = lowQuality.GetAverageColor();
            var average = new byte[4] { averageColor.R, averageColor.G, averageColor.B, averageColor.A }.toInt();

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    var valueColor = lowQuality.GetPixel(x, y);
                    // Debug.WriteLine("r: " + valueColor.R + ", g: " + valueColor.G + ", b: " + valueColor.B + ", a: " + valueColor.A);
                    var value = new byte[4] { valueColor.R, valueColor.G, valueColor.B, valueColor.A }.toInt();


                    // Debug.WriteLine("value: " + value);
                    // A/transparent 255 causes NaN / its rgba
                    var data = new Data() // when actually consuming in the future it must be better to make prediction here
                    {
                        average = average,
                        x = x,
                        y = y,
                        value = value
                    };
                    dataArray[x, y] = data;
                    data = null;
                    
                }
            }
            return dataArray;
        }
    }
}
