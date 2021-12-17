using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NUnit.Framework;
using PTSC.Communication.Controller;
using PTSC.Communication.Model;

namespace _PTSC.Tests
{
    public class DataControllerTests
    {
        DataController dataController;

        [SetUp]
        public void Setup()
        {
            dataController = new DataController();
        }
        [TearDown]
        public void Teardown()
        {
            dataController = null;
        }

        [Test]
        public void Test_SerializeDriverData()
        {
            string serializedData = string.Empty;
            var data = new DriverDataModel();
            data.head = new List<double> { 0.1f, 0.2f, 0.3f };
            string result = "head;" + data.head[0] + ";" + data.head[1] + ";" + data.head[2] + ";";
            Assert.DoesNotThrow(() =>
            {
                serializedData = dataController.SerializeDriverData(data);
            });
            Assert.Multiple(() =>
            {
                Assert.IsNotEmpty(serializedData);
                Assert.AreEqual(serializedData, result);
            });

        }

        [Test]
        public void Test_SerializeProperty()
        {
            string serializedProperty = string.Empty;
            string keyWord = "Test"; 
            List<double> coordinates = new List<double> { 0.1f, 0.2f, 0.3f };
            string result = keyWord + ";" + coordinates[0] + ";" + coordinates[1] + ";" + coordinates[2] + ";";
            Assert.DoesNotThrow(() =>
            {
                serializedProperty = dataController.SerializeProperty(keyWord, coordinates);
            });
            Assert.Multiple(() =>
            {
                Assert.IsNotEmpty(serializedProperty);
                Assert.AreEqual(serializedProperty, result);
            });
        }

        [Test]
        public void Test_SerializeProperty_EmptyCoordinates()
        {
            string serializedProperty = string.Empty;
            string keyWord = "Test";
            List<double> coordinates = null;
            Assert.DoesNotThrow(() =>
            {
                serializedProperty = dataController.SerializeProperty(keyWord, coordinates);
            });
            Assert.IsEmpty(serializedProperty);
        }

        [Test]
        public void Test_Deserialization()
        {
            // list of coordinates as string
            List<double> left_shoulder_coordinates = new List<double> { 0.1f, 0.2f, 0.3f };
            List<double> right_shoulder_coordinates = new List<double> { 0.4f, 0.5f, 0.6f };
            string left_shoulder_data = JsonConvert.SerializeObject(left_shoulder_coordinates);
            string right_shoulder_data = JsonConvert.SerializeObject(right_shoulder_coordinates);
            string data = "{ \"left_shoulder\": " + left_shoulder_data.ToString() + ", \"right_shoulder\": " + right_shoulder_data.ToString() + "}";
            ModuleDataModel moduleData = new ModuleDataModel();
            Assert.DoesNotThrow(() =>
            {
                moduleData = dataController.DeserializeModuleData(data);
            });
            Assert.Multiple(() =>
            {
                Assert.IsNotNull(moduleData);
                Assert.AreEqual(moduleData.LEFT_SHOULDER, left_shoulder_coordinates);
                Assert.AreEqual(moduleData.RIGHT_SHOULDER, right_shoulder_coordinates);
                
            });
        }

        [Test]
        public void Test_Deserialization_Empty_Data()
        {
            string data = string.Empty;
            ModuleDataModel moduleData = new ModuleDataModel();
            Assert.DoesNotThrow(() =>
            {
                moduleData = dataController.DeserializeModuleData(data);
            });
            Assert.Multiple(() =>
            {
                Assert.IsNull(moduleData);
            });
        }
    }
}
