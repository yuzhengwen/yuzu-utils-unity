using UnityEngine;

namespace YuzuValen.Utils.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class SafeAreaAnchor : MonoBehaviour
    {
        RectTransform rectTransform;
        private void Reset()
        {
            rectTransform = GetComponent<RectTransform>();
        }
        private void Awake()
        {
            if (rectTransform == null) rectTransform = GetComponent<RectTransform>();
        }
        private void Start()
        {
            SetAnchor();
        }
        private void OnRectTransformDimensionsChange()
        {
            if (rectTransform == null) rectTransform = GetComponent<RectTransform>();
            SetAnchor();
        }
        public void SetAnchor()
        {
            Rect safeArea = Screen.safeArea;
            Vector2 minAnchor = safeArea.position;
            Vector2 maxAnchor = minAnchor + safeArea.size;
            minAnchor.x /= Screen.width;
            minAnchor.y /= Screen.height;
            maxAnchor.x /= Screen.width;
            maxAnchor.y /= Screen.height;
            rectTransform.anchorMin = minAnchor;
            rectTransform.anchorMax = maxAnchor;
        }
    }
}