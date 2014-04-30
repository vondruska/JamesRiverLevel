using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JamesRiverLevel
{
    public class Statistics
    {
        public Trendline CalculateLinearRegression(float[] values)
        {
            var yAxisValues = new List<float>();
            var xAxisValues = new List<float>();

            for (int i = 0; i < values.Length; i++)
            {
                yAxisValues.Add(values[i]);
                xAxisValues.Add(i + 1);
            }

            return new Trendline(yAxisValues, xAxisValues);
        }
    }

    public class Trendline
    {
        private readonly IList<float> xAxisValues;
        private readonly IList<float> yAxisValues;
        private int count;
        private float xAxisValuesSum;
        private float xxSum;
        private float xySum;
        private float yAxisValuesSum;

        public Trendline(IList<float> yAxisValues, IList<float> xAxisValues)
        {
            this.yAxisValues = yAxisValues;
            this.xAxisValues = xAxisValues;

            this.Initialize();
        }

        public float Slope { get; private set; }
        public float Intercept { get; private set; }
        public float Start { get; private set; }
        public float End { get; private set; }

        private void Initialize()
        {
            this.count = this.yAxisValues.Count;
            this.yAxisValuesSum = this.yAxisValues.Sum();
            this.xAxisValuesSum = this.xAxisValues.Sum();
            this.xxSum = 0;
            this.xySum = 0;

            for (int i = 0; i < this.count; i++)
            {
                this.xySum += (this.xAxisValues[i] * this.yAxisValues[i]);
                this.xxSum += (this.xAxisValues[i] * this.xAxisValues[i]);
            }

            this.Slope = this.CalculateSlope();
            this.Intercept = this.CalculateIntercept();
            this.Start = this.CalculateStart();
            this.End = this.CalculateEnd();
        }

        private float CalculateSlope()
        {
            try
            {
                return ((this.count * this.xySum) - (this.xAxisValuesSum * this.yAxisValuesSum)) / ((this.count * this.xxSum) - (this.xAxisValuesSum * this.xAxisValuesSum));
            }
            catch (DivideByZeroException)
            {
                return 0;
            }
        }

        private float CalculateIntercept()
        {
            return (this.yAxisValuesSum - (this.Slope * this.xAxisValuesSum)) / this.count;
        }

        private float CalculateStart()
        {
            return (this.Slope * this.xAxisValues.First()) + this.Intercept;
        }

        private float CalculateEnd()
        {
            return (this.Slope * this.xAxisValues.Last()) + this.Intercept;
        }
    }
}