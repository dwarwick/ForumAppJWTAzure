using Microsoft.ML.Data;
using Ganss.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForumAppJWTAzure.Shared.Models
{
    /// <summary>
    /// model input class for PredictTags.
    /// </summary>
    #region model input class
    public class ModelInput
    {
        [ColumnName(@"Title")]
        [Column(1)]
        public string Title { get; set; }

        [ColumnName(@"Body")]
        [Column(2)]
        public string Body { get; set; }

        [ColumnName(@"Tags")]
        [Column(3)]
        public string Tags { get; set; }

    }

    #endregion

    /// <summary>
    /// model output class for PredictTags.
    /// </summary>
    #region model output class
    public class ModelOutput
    {
        [ColumnName(@"Title")]
        public float[] Title { get; set; }

        [ColumnName(@"Body")]
        public float[] Body { get; set; }

        [ColumnName(@"Tags")]
        public uint Tags { get; set; }

        [ColumnName(@"Features")]
        public float[] Features { get; set; }

        [ColumnName(@"PredictedLabel")]
        public string PredictedLabel { get; set; }

        [ColumnName(@"Score")]
        public float[] Score { get; set; }

    }

    #endregion
}
