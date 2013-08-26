namespace JamesRiverLevel.Tests
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Text;
    using Helper;
    using Models;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class Tests
    {
        [TestMethod]
        public void KnownGoodXML()
        {
            Assembly asm = Assembly.GetExecutingAssembly();

            Stream xmlStream = asm.GetManifestResourceStream("JamesRiverLevel.Tests.XML.good.xml");

            var test = NWS.GetRiverInformation(xmlStream);
            Assert.IsNotNull(test);
            Assert.AreEqual(test.id, "RMDV2");
        }

        [TestMethod]
        public void KnownInvalid()
        {
            Assembly asm = Assembly.GetExecutingAssembly();

            Stream xmlStream = asm.GetManifestResourceStream("JamesRiverLevel.Tests.XML.invalid.xml");

            var test = NWS.GetRiverInformation(xmlStream);
            Assert.IsNull(test);
        }

        [TestMethod]
        public void BlankXML()
        {
            var test = NWS.GetRiverInformation(new MemoryStream(Encoding.Default.GetBytes(String.Empty)));
            Assert.IsNull(test);
        }

        [TestMethod]
        public void InvalidObject()
        {
            var test = NWS.Parse(null);
            Assert.IsNull(test);
        }

        [TestMethod]
        public void None()
        {
            Assembly asm = Assembly.GetExecutingAssembly();

            Stream xmlStream = asm.GetManifestResourceStream("JamesRiverLevel.Tests.XML.good.xml");

            var test = NWS.GetRiverInformation(xmlStream);
            var viewModel = NWS.Parse(test);
            Assert.AreEqual(WaterLevelAction.None, viewModel.ActionLevel);
        }

        [TestMethod]
        public void Lifejacket()
        {
            Assembly asm = Assembly.GetExecutingAssembly();

            Stream xmlStream = asm.GetManifestResourceStream("JamesRiverLevel.Tests.XML.lifejacket.xml");

            var test = NWS.GetRiverInformation(xmlStream);

            var viewModel = NWS.Parse(test);
            Assert.AreEqual(WaterLevelAction.LifeJacket, viewModel.ActionLevel);
            Assert.AreEqual(5.0f, viewModel.WaterLevel);
            Assert.AreEqual("ft", viewModel.WaterLevelUnit);
        }

        [TestMethod]
        public void Permit()
        {
            Assembly asm = Assembly.GetExecutingAssembly();

            Stream xmlStream = asm.GetManifestResourceStream("JamesRiverLevel.Tests.XML.permit.xml");

            var test = NWS.GetRiverInformation(xmlStream);

            var viewModel = NWS.Parse(test);
            Assert.AreEqual(WaterLevelAction.Permit, viewModel.ActionLevel);
            Assert.AreEqual(9.0f, viewModel.WaterLevel);
            Assert.AreEqual("ft", viewModel.WaterLevelUnit);
        }
    }
}
