using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using YuzuValen.Utils.Collections;

namespace YuzuValen.Editor
{
    [CustomPropertyDrawer(typeof(SerializableKeyValuePair<,>))]
    public class SerializedKeyValuePairDrawer : PropertyDrawer
    {
        private readonly Color _warningColor = new Color(1f, 0.2f, 0.2f, 0.5f);

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var keyProperty = property.FindPropertyRelative("Key");
            var valueProperty = property.FindPropertyRelative("Value");

            if (keyProperty == null || valueProperty == null)
            {
                EditorGUI.LabelField(position, label.text, "Error: Cannot find Key or Value property");
                EditorGUI.EndProperty();
                return;
            }

            var originalColor = GUI.backgroundColor;

            // Check if key is valid (not default/empty)
            if (!IsKeyValid(keyProperty))
            {
                // Highlight invalid keys
                GUI.backgroundColor = _warningColor;
            }

            // Check if value is a complex type (has children)
            var valueIsComplex = valueProperty.hasChildren &&
                                 valueProperty.propertyType == SerializedPropertyType.Generic;
            if (valueIsComplex)
            {
                // For complex types, use full width with foldout
                float keyHeight = EditorGUI.GetPropertyHeight(keyProperty);
                Rect keyRect = new Rect(position.x, position.y, position.width, keyHeight);

                EditorGUI.PropertyField(keyRect, keyProperty, new GUIContent("Key"));

                GUI.backgroundColor = originalColor;

                // draw value below key
                float valueY = position.y + keyHeight + EditorGUIUtility.standardVerticalSpacing;
                float valueHeight = EditorGUI.GetPropertyHeight(valueProperty, true);
                Rect valueRect = new Rect(position.x, valueY, position.width, valueHeight);

                EditorGUI.PropertyField(valueRect, valueProperty, new GUIContent("Value"), true);
            }
            else
            {
                // For simple types, draw side by side
                float halfWidth = position.width / 2f - 5f;
                Rect keyRect = new Rect(position.x, position.y, halfWidth, position.height);
                Rect valueRect = new Rect(position.x + halfWidth + 10f, position.y, halfWidth, position.height);

                EditorGUI.PropertyField(keyRect, keyProperty, GUIContent.none);
                EditorGUI.PropertyField(valueRect, valueProperty, GUIContent.none);

                GUI.backgroundColor = originalColor;
            }

            EditorGUI.EndProperty();
        }

        private bool IsKeyValid(SerializedProperty keyProperty)
        {
            switch (keyProperty.propertyType)
            {
                case SerializedPropertyType.String:
                    return !string.IsNullOrEmpty(keyProperty.stringValue);
                case SerializedPropertyType.Integer:
                    return true; // All integers are valid (including 0)
                case SerializedPropertyType.Enum:
                    return true; // Enums are always valid
                case SerializedPropertyType.ObjectReference:
                    return keyProperty.objectReferenceValue != null;
                default:
                    return true;
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var keyProperty = property.FindPropertyRelative("Key");
            var valueProperty = property.FindPropertyRelative("Value");

            // as above: this returns a single line error
            if (keyProperty == null || valueProperty == null)
                return EditorGUIUtility.singleLineHeight;

            // Check if value is complex
            bool valueIsComplex = valueProperty.hasChildren &&
                                  valueProperty.propertyType == SerializedPropertyType.Generic;

            if (valueIsComplex)
            {
                // Return height for both key and value stacked vertically
                float keyHeight = EditorGUI.GetPropertyHeight(keyProperty);
                float valueHeight = EditorGUI.GetPropertyHeight(valueProperty, true);
                return keyHeight + valueHeight + EditorGUIUtility.standardVerticalSpacing;
            }
            else
            {
                // Return height for side-by-side layout
                return Mathf.Max(
                    EditorGUI.GetPropertyHeight(keyProperty),
                    EditorGUI.GetPropertyHeight(valueProperty)
                );
            }
        }
    }

    [CustomPropertyDrawer(typeof(SerializableDictionary<,>), true)]
    public class SerializableDictionaryDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var pairsProperty = property.FindPropertyRelative("pairs");

            if (pairsProperty != null)
            {
                float listHeight = EditorGUI.GetPropertyHeight(pairsProperty, true);
                Rect listRect = new Rect(position.x, position.y, position.width, listHeight);

                // Draw the list
                EditorGUI.PropertyField(listRect, pairsProperty, label, true);

                // Count valid and invalid entries
                int validCount = 0;
                int invalidCount = 0;
                List<string> duplicateKeys = new List<string>();
                HashSet<string> seenKeys = new HashSet<string>();

                for (int i = 0; i < pairsProperty.arraySize; i++)
                {
                    var element = pairsProperty.GetArrayElementAtIndex(i);
                    var keyProperty = element.FindPropertyRelative("Key");

                    if (IsKeyValid(keyProperty))
                    {
                        string keyString = GetKeyAsString(keyProperty);
                        if (!seenKeys.Add(keyString))
                        {
                            if (!duplicateKeys.Contains(keyString))
                            {
                                duplicateKeys.Add(keyString);
                            }
                        }

                        validCount++;
                    }
                    else
                    {
                        invalidCount++;
                    }
                }

                // Show info/warning messages
                float yOffset = position.y + listHeight + 2f;

                if (invalidCount > 0)
                {
                    Rect infoRect = new Rect(position.x, yOffset, position.width,
                        EditorGUIUtility.singleLineHeight * 2);
                    EditorGUI.HelpBox(infoRect,
                        $"{invalidCount} entry(ies) with empty keys will be ignored. Fill in the key to add to dictionary.",
                        MessageType.Info);
                    yOffset += EditorGUIUtility.singleLineHeight * 2.5f;
                }

                if (duplicateKeys.Count > 0)
                {
                    Rect warningRect = new Rect(position.x, yOffset, position.width,
                        EditorGUIUtility.singleLineHeight * 2);
                    EditorGUI.HelpBox(warningRect,
                        $"Duplicate keys detected: {string.Join(", ", duplicateKeys)}. Only the first occurrence will be used.",
                        MessageType.Warning);
                }
            }
            else
            {
                EditorGUI.LabelField(position, label.text, "Unable to find pairs field");
            }

            EditorGUI.EndProperty();
        }

        private bool IsKeyValid(SerializedProperty keyProperty)
        {
            switch (keyProperty.propertyType)
            {
                case SerializedPropertyType.String:
                    return !string.IsNullOrEmpty(keyProperty.stringValue);
                case SerializedPropertyType.Integer:
                    return true;
                case SerializedPropertyType.Enum:
                    return true;
                case SerializedPropertyType.ObjectReference:
                    return keyProperty.objectReferenceValue != null;
                default:
                    return true;
            }
        }

        private string GetKeyAsString(SerializedProperty keyProperty)
        {
            switch (keyProperty.propertyType)
            {
                case SerializedPropertyType.String:
                    return keyProperty.stringValue;
                case SerializedPropertyType.Integer:
                    return keyProperty.intValue.ToString();
                case SerializedPropertyType.Enum:
                    return keyProperty.enumNames[keyProperty.enumValueIndex];
                case SerializedPropertyType.ObjectReference:
                    return keyProperty.objectReferenceValue != null
                        ? keyProperty.objectReferenceValue.GetInstanceID().ToString()
                        : "";
                default:
                    return "";
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var pairsProperty = property.FindPropertyRelative("pairs");

            if (pairsProperty != null)
            {
                float height = EditorGUI.GetPropertyHeight(pairsProperty, true);

                // Count messages
                int messageCount = 0;
                bool hasInvalid = false;
                HashSet<string> seenKeys = new HashSet<string>();
                bool hasDuplicates = false;

                for (int i = 0; i < pairsProperty.arraySize; i++)
                {
                    var element = pairsProperty.GetArrayElementAtIndex(i);
                    var keyProperty = element.FindPropertyRelative("Key");

                    if (keyProperty != null && !IsKeyValid(keyProperty))
                    {
                        hasInvalid = true;
                    }
                    else
                    {
                        string keyString = GetKeyAsString(keyProperty);
                        if (!seenKeys.Add(keyString))
                        {
                            hasDuplicates = true;
                        }
                    }
                }

                if (hasInvalid)
                {
                    height += EditorGUIUtility.singleLineHeight * 2.5f;
                }

                if (hasDuplicates)
                {
                    height += EditorGUIUtility.singleLineHeight * 2.5f;
                }

                return height;
            }

            return EditorGUIUtility.singleLineHeight;
        }
    }
}