using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ImageRecreator
{
    public class Data
    {
        // argb is in float beacuse label must be of float
        /*
        [ColumnName("Total")]
        public float[,] total;
        */

        // public Bitmap total;
        /*
        [ColumnName("Index")]
        public float [,] index;
        */
        [ColumnName("Total")]
        public int[] total;
        [ColumnName("X")]
        public int x;
        [ColumnName("Y")]
        public int y;
        [ColumnName("Value")]
        public int value;
        [ColumnName("Label")]
        public int original;

        /* When running automl... Was also unable to run b total as bitmap
         * System.ArgumentException: 'Only supported feature column types are Boolean, Single, and String. Please change the feature column index of type Vector<Int32> to one of the supported types.
Parameter name: trainData'
*/
    }
}
