﻿using Microsoft.ML;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Diagnostics;
using Microsoft.ML.AutoML;
using Microsoft.ML.Data;
using System.Linq;

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
                imageData.AddRange(ImageData(original, original.LowQualityImages(100), 0.01f));
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
            var trainingDataView = mlContext.Data.LoadFromEnumerable<Data>(imageData);
            Debug.WriteLine("starting training");
            var dataProccessPipeLine = mlContext.Transforms.CopyColumns("Label", "Label")
                .Append(mlContext.Transforms.Concatenate("Features","Average", "X", "Y", "Value"));

            var trainer = mlContext.Regression.Trainers.Sdca(labelColumnName: "Label", featureColumnName: "Features");

            var trainingPipeline = dataProccessPipeLine.Append(trainer);

            var model = trainingPipeline.Fit(trainingDataView);

            Debug.WriteLine("training complete - saving model");
            mlContext.Model.Save(model, trainingDataView.Schema, "./mymodel.zip");

            
        }
        static List<Data> ImageData(Bitmap original, List<Bitmap> lowImages, float percentPixelsToTake = 1)
        {
            var imageData = new List<Data>();
            Debug.WriteLine("original: " + original.Name());
            for(int i = 0; i < lowImages.Count; i ++)
            {
                lowImages[i].Name(original.Name() + "q" + i); // for test purpose
                imageData.AddRange(CreateDataFrom(original, lowImages[i], percentPixelsToTake));

            }

            Debug.WriteLine("created imageData: " + imageData.Count + " = " + lowImages.Count + " * " + " width: " + original.Width + " * " + " height: " + original.Height);
            return imageData;
        }

        static List<Data> CreateDataFrom(Bitmap original, Bitmap lowImage, float percent = 1)
        {
            Debug.WriteLine("Creating data...");
            var imageData = new List<Data>();


            // getting random indexes
            var lowImageArray = lowImage.ToValueArray();
            Debug.WriteLine("lowImageArray created");
            var amount = (int )Math.Round(percent * lowImageArray.Count());
            Debug.WriteLine("Taking amount: " + amount);
            
            int index = 0;
            while(index < amount)
            {
                var randomX = RandomizedIndex(lowImage.Width);
                var randomY = RandomizedIndex(lowImage.Height);
                // Debug.WriteLine("i: " + index + ". x: " + randomX + " and y:" + randomY);
                int value = lowImage.GetPixel(randomX, randomY).ToArgb();
                int originalValue = original.GetPixel(randomX, randomY).ToArgb();
                var data = new Data
                {
                    // total = lowImageArray,
                    x = randomX,
                    y = randomY,
                    value = value,
                    original = originalValue
                };
                if(!imageData.Exists((d) => (int)d.x != (int)data.x && (int)d.y != (int)data.y)) // not taking -1
                {
                    // data.Print();
                    imageData.Add(data);
                    index++;
                }
                data = null;
            }
            Debug.WriteLine("adding total/average to data...");
            
            var average = lowImageArray.Average();
            Debug.WriteLine(average);
            foreach(var d in imageData)
            {
                d.average = average; // <-- added afterwards is faster due to .exists
            }
            Debug.WriteLine("created " + imageData.Count + " of imageData from " + lowImage.Name());
            
            /*
            for (int x = 0; x < lowImage.Width; x++)
            {
                for (int y = 0; y < lowImage.Height; y++)
                {

                    var data = new Data()
                    {
                        total = lowImageArray,
                        x = x,
                        y = y,
                        value = (float)lowImage.GetPixel(x, y).ToArgb(),
                        original = (float)lowImage.GetPixel(x, y).ToArgb()
                    };
                    imageData.Add(data);
                    // data.Print();
                }
            }
            */
            return imageData;
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
