using NUnit.Framework;
using UnityEngine;
using YuzuValen.Utils.Collections;

namespace YuzuValen.Utils.Tests.EditMode
{
    [System.Serializable]
    public class MyClass
    {
        public int x;
        public int y;

        public override string ToString() => $"MyClass(x: {x}, y: {y})";
    }

    public class SerializableDictionaryEditTests
    {
        #region Basic adding

        [Test]
        public void Dictionary_WithPrimitives_WorksCorrectly()
        {
            // Arrange
            var dict = new SerializableDictionary<string, int>();

            // Act
            dict.Add("key1", 10);
            dict.Add("key2", 20);

            // Assert
            Assert.AreEqual(2, dict.Count);
            Assert.AreEqual(10, dict["key1"]);
            Assert.AreEqual(20, dict["key2"]);
        }

        [Test]
        public void Dictionary_WithComplexType_WorksCorrectly()
        {
            // Arrange
            var dict = new SerializableDictionary<string, MyClass>();
            var myClass = new MyClass { x = 5, y = 10 };

            // Act
            dict.Add("test", myClass);

            // Assert
            Assert.AreEqual(1, dict.Count);
            Assert.AreEqual(5, dict["test"].x);
            Assert.AreEqual(10, dict["test"].y);
        }

        private enum MyEnum
        {
            First,
            Second,
            Third
        }

        [Test]
        public void Dictionary_WithEnums_WorksCorrectly()
        {
            // Arrange
            var dict = new SerializableDictionary<string, MyEnum>();

            // Act
            dict.Add("first", MyEnum.First);
            dict.Add("second", MyEnum.Second);

            // Assert
            Assert.AreEqual(2, dict.Count);
            Assert.AreEqual(MyEnum.First, dict["first"]);
            Assert.AreEqual(MyEnum.Second, dict["second"]);
        }

        #endregion

        #region Serialization Tests

        [Test]
        public void Dictionary_SerializeDeserializeEnums_PreservesData()
        {
            // Arrange
            var originalDict = new SerializableDictionary<string, MyEnum>();
            originalDict.Add("first", MyEnum.First);
            originalDict.Add("second", MyEnum.Second);

            // Act
            var json = originalDict.ToJson();
            Debug.Log(json);
            var deserializedDict = JsonUtility.FromJson<SerializableDictionary<string, MyEnum>>(json);

            // Assert
            Assert.AreEqual(2, deserializedDict.Count);
            Assert.AreEqual(MyEnum.First, deserializedDict["first"]);
            Assert.AreEqual(MyEnum.Second, deserializedDict["second"]);
        }

        [Test]
        public void Dictionary_SerializeDeserialize_PreservesData()
        {
            // Arrange
            var originalDict = new SerializableDictionary<string, MyClass>();

            // Add items explicitly instead of using collection initializer
            originalDict.Add("entity1", new MyClass { x = 1, y = 2 });
            originalDict.Add("entity2", new MyClass { x = 3, y = 4 });

            // Act
            var json = originalDict.ToJson();
            Debug.Log(json);
            var deserializedDict = JsonUtility.FromJson<SerializableDictionary<string, MyClass>>(json);

            // Assert
            Assert.AreEqual(2, deserializedDict.Count);
            Assert.IsTrue(deserializedDict.ContainsKey("entity1"));
            Assert.IsTrue(deserializedDict.ContainsKey("entity2"));
            Assert.AreEqual(1, deserializedDict["entity1"].x);
            Assert.AreEqual(2, deserializedDict["entity1"].y);
            Assert.AreEqual(3, deserializedDict["entity2"].x);
            Assert.AreEqual(4, deserializedDict["entity2"].y);
        }

        [Test]
        public void Dictionary_SerializeDeserialize_WithPrimitives_WorksCorrectly()
        {
            // Arrange
            var originalDict = new SerializableDictionary<string, int>();
            originalDict.Add("one", 1);
            originalDict.Add("two", 2);

            // act
            var json = originalDict.ToJson();
            var deserializedDict = JsonUtility.FromJson<SerializableDictionary<string, int>>(json);

            // Assert
            Assert.AreEqual(2, deserializedDict.Count);
            Assert.AreEqual(1, deserializedDict["one"]);
            Assert.AreEqual(2, deserializedDict["two"]);
        }

        [Test]
        public void Dictionary_SerializeDeserialize_EmptyDictionary()
        {
            // Arrange
            var dict = new SerializableDictionary<string, int>();

            // Act
            string json = JsonUtility.ToJson(dict);
            var deserializedDict = JsonUtility.FromJson<SerializableDictionary<string, int>>(json);

            // Assert
            Assert.AreEqual(0, deserializedDict.Count);
        }

        #endregion

        [Test]
        public void Dictionary_Remove_WorksCorrectly()
        {
            // Arrange
            var dict = new SerializableDictionary<string, int>();
            dict.Add("key1", 10);
            dict.Add("key2", 20);

            // Act
            bool removed = dict.Remove("key1");

            // Assert
            Assert.IsTrue(removed);
            Assert.AreEqual(1, dict.Count);
            Assert.IsFalse(dict.ContainsKey("key1"));
            Assert.IsTrue(dict.ContainsKey("key2"));
        }

        [Test]
        public void Dictionary_Clear_RemovesAllEntries()
        {
            // Arrange
            var dict = new SerializableDictionary<string, int>();
            dict.Add("key1", 10);
            dict.Add("key2", 20);

            // Act
            dict.Clear();

            // Assert
            Assert.AreEqual(0, dict.Count);
        }

        [Test]
        public void Dictionary_TryGetValue_WorksCorrectly()
        {
            // Arrange
            var dict = new SerializableDictionary<string, int>();
            dict.Add("key1", 10);

            // Act
            bool found = dict.TryGetValue("key1", out int value);
            bool notFound = dict.TryGetValue("nonexistent", out int missingValue);

            // Assert
            Assert.IsTrue(found);
            Assert.AreEqual(10, value);
            Assert.IsFalse(notFound);
            Assert.AreEqual(0, missingValue);
        }

        [Test]
        public void Dictionary_ContainsKey_WorksCorrectly()
        {
            // Arrange
            var dict = new SerializableDictionary<string, int>();
            dict.Add("key1", 10);

            // Act & Assert
            Assert.IsTrue(dict.ContainsKey("key1"));
            Assert.IsFalse(dict.ContainsKey("nonexistent"));
        }

        [Test]
        public void Dictionary_UpdateValue_WorksCorrectly()
        {
            // Arrange
            var dict = new SerializableDictionary<string, int>();
            dict.Add("key1", 10);

            // Act
            dict["key1"] = 20;

            // Assert
            Assert.AreEqual(20, dict["key1"]);
        }

        [Test]
        public void Dictionary_Enumeration_WorksCorrectly()
        {
            // Arrange
            var dict = new SerializableDictionary<string, int>();
            dict.Add("key1", 10);
            dict.Add("key2", 20);
            dict.Add("key3", 30);

            // Act
            int sum = 0;
            int count = 0;
            foreach (var kvp in dict)
            {
                sum += kvp.Value;
                count++;
            }

            // Assert
            Assert.AreEqual(3, count);
            Assert.AreEqual(60, sum);
        }

        [Test]
        public void Dictionary_LargeDataSet_WorksCorrectly()
        {
            // Arrange
            var dict = new SerializableDictionary<int, string>();

            // Act
            for (int i = 0; i < 100; i++)
            {
                dict.Add(i, $"value_{i}");
            }

            // Assert
            Assert.AreEqual(100, dict.Count);
            Assert.AreEqual("value_50", dict[50]);
            Assert.AreEqual("value_99", dict[99]);
        }
    }
}