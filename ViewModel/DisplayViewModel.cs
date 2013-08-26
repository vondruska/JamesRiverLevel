namespace JamesRiverLevel.ViewModel
{
    using System;

    using Models;

    public class DisplayViewModel
    {
        public float WaterLevel { get; set; }

        public string WaterLevelUnit { get; set; }

        public DateTime WaterLevelWhen { get; set; }

        public WaterLevelAction ActionLevel { get; set; }
    }
}