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
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    var data = dataArray[x, y];
                    var output = predictionEngine.Predict(dataArray[x, y]);
                    /*
                    if (original == null)
                    {
                        Debug.WriteLine("value: " + data.value + " -> " + " output: " + output.outputValue);
                    }
                    else
                    {
                        Debug.WriteLine("value: " + data.value + " -> " + " output: " 
                            + output.outputValue + ". original: " + original.GetPixel(x, y).ToArgb());
                    }
                    */
                    predictedBitmap.SetPixel(x, y, Color.FromArgb((int) output.outputValue));
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

            
            var average = lowQuality.ToValueArray().Average();
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    var data = new Data() // when actually consuming in the future it must be better to make prediction here
                    {
                        average = average,
                        x = x,
                        y = y,
                        value = lowQuality.GetPixel(x, y).ToArgb()
                    };
                    dataArray[x, y] = data;
                    data = null;
                }
            }
            return dataArray;
        }
    }
}
