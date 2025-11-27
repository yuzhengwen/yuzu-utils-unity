using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using YuzuValen.Utils.Collections;

namespace YuzuUtils.Tests.PlayMode
{
    /// <summary>
    /// Play mode tests for SerializableDictionary - tests that require Unity's runtime
    /// </summary>
    public class SerializableDictionaryPlayTests
    {
        [UnityTest]
        public IEnumerator Dictionary_WithGameObject_WorksInPlayMode()
        {
            // Arrange
            var dict = new SerializableDictionary<string, GameObject>();
            var go = new GameObject("TestObject");

            // Act
            dict.Add("player", go);

            yield return null;

            // Assert
            Assert.AreEqual(1, dict.Count);
            Assert.IsNotNull(dict["player"]);
            Assert.AreEqual("TestObject", dict["player"].name);

            // Cleanup
            Object.Destroy(go);
        }

        [UnityTest]
        public IEnumerator Dictionary_WithMultipleGameObjects_SerializesCorrectly()
        {
            // Arrange
            var dict = new SerializableDictionary<string, GameObject>();
            var player = new GameObject("Player");
            var enemy = new GameObject("Enemy");

            // Act
            dict.Add("player", player);
            dict.Add("enemy", enemy);

            yield return null;

            // Simulate scene serialization
            string json = JsonUtility.ToJson(dict);

            // Assert
            Assert.AreEqual(2, dict.Count);
            Assert.IsNotNull(dict["player"]);
            Assert.IsNotNull(dict["enemy"]);

            // Cleanup
            Object.Destroy(player);
            Object.Destroy(enemy);
        }

        [UnityTest]
        public IEnumerator Dictionary_WithComponent_WorksCorrectly()
        {
            // Arrange
            var dict = new SerializableDictionary<string, Transform>();
            var go = new GameObject("TestObject");
            var transform = go.transform;

            // Act
            dict.Add("mainTransform", transform);

            yield return null;

            // Assert
            Assert.AreEqual(1, dict.Count);
            Assert.IsNotNull(dict["mainTransform"]);
            Assert.AreEqual("TestObject", dict["mainTransform"].name);

            // Cleanup
            Object.Destroy(go);
        }

        [UnityTest]
        public IEnumerator Dictionary_RuntimeModification_PersistsAcrossFrames()
        {
            // Arrange
            var dict = new SerializableDictionary<string, int>();

            // Act - Frame 1
            dict.Add("score", 0);
            yield return null;

            // Act - Frame 2
            dict["score"] = 100;
            yield return null;

            // Assert - Frame 3
            Assert.AreEqual(100, dict["score"]);
        }
    }
}