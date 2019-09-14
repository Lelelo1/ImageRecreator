using Microsoft.ML;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Diagnostics;
using System.Linq;
using ImageRecreator;

namespace ImageRecreator
{
    public class Consume // use ComsumeExtensions instead?
    {
        public static void Run()
        {
            var mlContext = new MLContext();
            DataViewSchema predictionPipelineSchema;
            ITransformer predictionPipeline = mlContext.Model.Load("./mymodel.zip", out predictionPipelineSchema);
           //  PredictionEngine<Data, OutputData> predictionEngine = mlContext.Model.CreatePredictionEngine<Data, OutputData>(predictionPipeline);


            // var output = predictionEngine.Predict(new Data { low = new float[] { 0.1f, 0.1f }, original = new float[] { 2f, 2f } });

            // Console.WriteLine("prediction: " + output.predicted + " with score: " + output.score);
        }
   
       
    }
}
