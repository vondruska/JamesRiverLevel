namespace JamesRiverLevel.Helper
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Xml;
    using System.Xml.Serialization;

    using Models;

    using ViewModel;

    public static class NWS
    {
        public static site GetRiverInformation()
        {
            XmlSerializer ser = new XmlSerializer(typeof(site));
            using (XmlReader reader = XmlReader.Create("http://water.weather.gov/ahps2/hydrograph_to_xml.php?gage=rmdv2&output=xml"))
            {
                try
                {
                    return ser.Deserialize(reader) as site;
                }
                catch (XmlException)
                {
                    return null;
                }
            }
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
            var test = results.observed.OrderByDescending(x => x.valid.Value).First();

            viewModel.WaterLevel = test.primary.Value;
            viewModel.WaterLevelUnit = test.primary.units;
            viewModel.WaterLevelWhen = TimeZoneInfo.ConvertTime(test.valid.Value, TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"));

            if (viewModel.WaterLevel >= 5 && viewModel.WaterLevel < 9)
            {
                viewModel.ActionLevel = WaterLevelAction.LifeJacket;
            }
            else if (viewModel.WaterLevel >= 9)
            {
                viewModel.ActionLevel = WaterLevelAction.Permit;
            }

            return viewModel;
        }
    }
}