﻿namespace JamesRiverLevel.Helper
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Xml;
    using System.Xml.Serialization;
    using Models;
    using ViewModel;

    public static class NWS
    {
        public static site GetRiverInformation()
        {
            var request = WebRequest.Create("http://water.weather.gov/ahps2/hydrograph_to_xml.php?gage=rmdv2&output=xml");
            request.Timeout = 1000;
            var response = request.GetResponse();
            return GetRiverInformation(response.GetResponseStream());
        }

        public static site GetRiverInformation(Stream stream)
        {
            XmlSerializer ser = new XmlSerializer(typeof(site));
            using (XmlReader reader = XmlReader.Create(stream))
            {
                try
                {
                    return ser.Deserialize(reader) as site;
                }
                catch (XmlException)
                {
                    return null;
                }
                catch (InvalidOperationException)
                {
                    return null;
                }
            }

        }

        public static DisplayViewModel Parse(site results)
        {
            if (results == null) return null;

            var viewModel = new DisplayViewModel();
            viewModel.DataObtainedAt = DateTime.Now;
            var test = results.observed.OrderByDescending(x => x.valid.Value).First();

            viewModel.WaterLevel = test.primary.Value;
            viewModel.WaterLevelUnit = test.primary.units;
            viewModel.WaterLevelWhen = TimeZoneInfo.ConvertTime(test.valid.Value, TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"));
            viewModel.WaterLevelWhenUtc = test.valid.Value;

            if (viewModel.WaterLevel >= 5 && viewModel.WaterLevel < 9)
            {
                viewModel.ActionLevel = WaterLevelAction.LifeJacket;
            }
            else if (viewModel.WaterLevel >= 9)
            {
                viewModel.ActionLevel = WaterLevelAction.Permit;
            }

            viewModel.Future = results.forecast.datum.ToDictionary(x => x.valid.Value, x => x.primary.Value);
            viewModel.Observed = results.observed.OrderBy(x => x.valid.Value).Where(x => x.valid.Value > DateTime.UtcNow.AddHours(-24)).ToDictionary(x => TimeZoneInfo.ConvertTime(x.valid.Value, TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time")), x => x.primary.Value);

            if (results.forecast.datum.Any(x => results.sigstages.record.Value <= x.primary.Value))
                viewModel.FloodingCategoryForecast = FloodingCategoryForecast.Record;

            else if (results.forecast.datum.Any(x => results.sigstages.major.Value <= x.primary.Value))
                viewModel.FloodingCategoryForecast = FloodingCategoryForecast.Major;

            else if (results.forecast.datum.Any(x => results.sigstages.moderate.Value <= x.primary.Value))
                viewModel.FloodingCategoryForecast = FloodingCategoryForecast.Moderate;

            else if (results.forecast.datum.Any(x => results.sigstages.flood.Value <= x.primary.Value))
                viewModel.FloodingCategoryForecast = FloodingCategoryForecast.Flood;

            else if (results.forecast.datum.Any(x => results.sigstages.bankfull.Value <= x.primary.Value))
                viewModel.FloodingCategoryForecast = FloodingCategoryForecast.Bankful;

            else if (results.forecast.datum.Any(x => results.sigstages.action.Value <= x.primary.Value))
                viewModel.FloodingCategoryForecast = FloodingCategoryForecast.Action;

            //var p = Fit.Line(Array.ConvertAll(Enumerable.Range(0, results.observed.Count()).ToArray(), x => (double)x), results.observed.Select(x => (double)x.primary.Value).ToArray());

            var f = new Statistics().CalculateLinearRegression(results.observed.OrderBy(x => x.valid.Value).Where(x => x.valid.Value > DateTime.UtcNow.AddHours(-15)).Select(x => x.primary.Value).ToArray());
            if (f.Slope > .25)
                viewModel.Trend = WaterTrend.Rising;
            else if (f.Slope < -.25)
                viewModel.Trend = WaterTrend.Falling;
            else
                viewModel.Trend = WaterTrend.Steady;


            return viewModel;
        }
    }
}
