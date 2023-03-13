﻿// This file was auto-generated by ML.NET Model Builder. 

using Microsoft.ML;
using Microsoft.ML.Data;
using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using Microsoft.ML.Transforms.TimeSeries;

namespace AIChat_ConsoleApp1
{
    public partial class AIChat
    {
        /// <summary>
        /// model input class for AIChat.
        /// </summary>
        #region model input class
        public class ModelInput
        {
            [ColumnName(@"col0")]
            public float Col0 { get; set; }

        }

        #endregion

        /// <summary>
        /// model output class for AIChat.
        /// </summary>
        #region model output class
        public class ModelOutput
        {
            [ColumnName(@"col0")]
            public float[] Col0 { get; set; }

            [ColumnName(@"col0_LB")]
            public float[] Col0_LB { get; set; }

            [ColumnName(@"col0_UB")]
            public float[] Col0_UB { get; set; }

        }

        #endregion

        private static string MLNetModelPath = Path.GetFullPath(@"C:\Users\tcraz\source\repos\LessonsBot\LessonsBot_Vk\ML\AIChat.zip");

        public static readonly Lazy<TimeSeriesPredictionEngine<ModelInput, ModelOutput>> PredictEngine = new Lazy<TimeSeriesPredictionEngine<ModelInput, ModelOutput>>(() => CreatePredictEngine(), true);

        /// <summary>
        /// Use this method to predict on <see cref="ModelInput"/>.
        /// </summary>
        /// <param name="input">model input.</param>
        /// <returns><seealso cref=" ModelOutput"/></returns>
        public static ModelOutput Predict(ModelInput? input = null, int? horizon = null)
        {
            var predEngine = PredictEngine.Value;
            return predEngine.Predict(input, horizon);
        }

        private static TimeSeriesPredictionEngine<ModelInput, ModelOutput> CreatePredictEngine()
        {
            var mlContext = new MLContext();
            ITransformer mlModel = mlContext.Model.Load(MLNetModelPath, out var schema);
            return mlModel.CreateTimeSeriesEngine<ModelInput, ModelOutput>(mlContext);
        }
    }
}

