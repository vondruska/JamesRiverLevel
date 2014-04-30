namespace JamesRiverLevel.ViewModel
{
    using System;
    using System.Collections.Generic;

    using Models;

    public class DisplayViewModel
    {
        public float WaterLevel { get; set; }

        public string WaterLevelUnit { get; set; }

        public DateTime WaterLevelWhen { get; set; }

        public DateTime WaterLevelWhenUtc { get; set; }

        public WaterLevelAction ActionLevel { get; set; }

        public FloodingCategoryForecast FloodingCategoryForecast { get; set; }

        public Dictionary<DateTime, float> Future { get; set; }

        public Dictionary<DateTime, float> Observed { get; set; }

        public WaterTrend Trend { get; set; }
    }
}