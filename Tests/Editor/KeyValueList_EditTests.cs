using NUnit.Framework;
using UnityEngine;
using YuzuValen.Utils.Collections;

namespace YuzuValen.Utils.Tests.EditMode
{
    /// <summary>
    /// Edit mode tests for KeyValueList
    /// </summary>
    public class KeyValueListEditTests
    {
        [Test]
        public void KeyValueList_AddPairs_WorksCorrectly()
        {
            // Arrange
            var kvList = new KeyValueList<string, int>();

            // Act
            kvList.pairs.Add(new SerializableKeyValuePair<string, int>("key1", 10));
            kvList.pairs.Add(new SerializableKeyValuePair<string, int>("key2", 20));

            // Assert
            Assert.AreEqual(2, kvList.pairs.Count);
            Assert.AreEqual("key1", kvList.pairs[0].key);
            Assert.AreEqual(10, kvList.pairs[0].value);
            Assert.AreEqual("key2", kvList.pairs[1].key);
            Assert.AreEqual(20, kvList.pairs[1].value);
        }

        [Test]
        public void KeyValueList_SerializeDeserialize_PreservesData()
        {
            // Arrange
            var originalList = new KeyValueList<string, int>();
            originalList.pairs.Add(new SerializableKeyValuePair<string, int>("key1", 10));
            originalList.pairs.Add(new SerializableKeyValuePair<string, int>("key2", 20));

            // Act
            string json = JsonUtility.ToJson(originalList);
            var deserializedList = JsonUtility.FromJson<KeyValueList<string, int>>(json);

            // Assert
            Assert.AreEqual(2, deserializedList.pairs.Count);
            Assert.AreEqual("key1", deserializedList.pairs[0].key);
            Assert.AreEqual(10, deserializedList.pairs[0].value);
            Assert.AreEqual("key2", deserializedList.pairs[1].key);
            Assert.AreEqual(20, deserializedList.pairs[1].value);
        }

        [Test]
        public void KeyValueList_WithComplexType_WorksCorrectly()
        {
            // Arrange
            var kvList = new KeyValueList<string, MyClass>();
            var myClass = new MyClass { x = 5, y = 10 };

            // Act
            kvList.pairs.Add(new SerializableKeyValuePair<string, MyClass>("test", myClass));

            // Assert
            Assert.AreEqual(1, kvList.pairs.Count);
            Assert.AreEqual(5, kvList.pairs[0].value.x);
            Assert.AreEqual(10, kvList.pairs[0].value.y);
        }

        [Test]
        public void KeyValueList_Clear_RemovesAllPairs()
        {
            // Arrange
            var kvList = new KeyValueList<string, int>();
            kvList.pairs.Add(new SerializableKeyValuePair<string, int>("key1", 10));
            kvList.pairs.Add(new SerializableKeyValuePair<string, int>("key2", 20));

            // Act
            kvList.pairs.Clear();

            // Assert
            Assert.AreEqual(0, kvList.pairs.Count);
        }

        [Test]
        public void KeyValueList_RemoveAt_WorksCorrectly()
        {
            // Arrange
            var kvList = new KeyValueList<string, int>();
            kvList.pairs.Add(new SerializableKeyValuePair<string, int>("key1", 10));
            kvList.pairs.Add(new SerializableKeyValuePair<string, int>("key2", 20));
            kvList.pairs.Add(new SerializableKeyValuePair<string, int>("key3", 30));

            // Act
            kvList.pairs.RemoveAt(1);

            // Assert
            Assert.AreEqual(2, kvList.pairs.Count);
            Assert.AreEqual("key1", kvList.pairs[0].key);
            Assert.AreEqual("key3", kvList.pairs[1].key);
        }
    }
}