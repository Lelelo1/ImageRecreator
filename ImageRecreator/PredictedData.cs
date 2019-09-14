using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageRecreator
{
    public class OutputData
    {
        [ColumnName("Score")]
        public float outputValue;
    }
}
