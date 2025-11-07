using UnityEngine;
using UnityEngine.UIElements;

namespace YuzuValen.Utils.UI
{
    public class SafeAreaUIToolkit : MonoBehaviour
    {
        [SerializeField] private UIDocument uiDocument;
        [SerializeField] private string rootElementName = ""; // Optional: target specific element

        private VisualElement targetElement;
        private Rect lastSafeArea;

        private void Awake()
        {
            if (uiDocument == null)
                uiDocument = GetComponent<UIDocument>();
        }

        private void Start()
        {
            targetElement = string.IsNullOrEmpty(rootElementName)
                ? uiDocument.rootVisualElement
                : uiDocument.rootVisualElement.Q(rootElementName);

            ApplySafeArea();
        }

        private void Update()
        {
            // Check if safe area changed (e.g., device rotation)
            if (lastSafeArea != Screen.safeArea)
            {
                ApplySafeArea();
            }
        }

        private void ApplySafeArea()
        {
            if (targetElement == null) return;

            Rect safeArea = Screen.safeArea;
            lastSafeArea = safeArea;

            // Calculate padding in pixels
            float leftPadding = safeArea.x;
            float rightPadding = Screen.width - (safeArea.x + safeArea.width);
            float topPadding = Screen.height - (safeArea.y + safeArea.height);
            float bottomPadding = safeArea.y;

            // Apply padding to the element
            targetElement.style.paddingLeft = leftPadding;
            targetElement.style.paddingRight = rightPadding;
            targetElement.style.paddingTop = topPadding;
            targetElement.style.paddingBottom = bottomPadding;
        }
    }
}