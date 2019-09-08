using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageRecreator
{
    public class Data
    {
        /*
        [VectorType]
        public float[,] low;
        [VectorType]
        public float[,] original;
        */

        [ColumnName("Features")]
        public float[] low;
        [ColumnName("Label")]

        public float[] original;
    }
    public class OutputData
    {
        [ColumnName("Label")]
        public float[] predicted;
        [ColumnName("Score")]
        public float score;
    }
}
