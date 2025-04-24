using UnityEngine.UIElements;

namespace YuzuValen.Utils
{
    public static class UIHelper
    {
        public static void ShowUIElement(VisualElement element, bool enable, bool animate = true)
        {
            if (enable)
            {
                if (element.style.display == DisplayStyle.Flex)
                    return;
                if (animate)
                    FadeIn(element);
                else
                    element.style.display = DisplayStyle.Flex;
            }
            else
            {
                if (element.style.display == DisplayStyle.None)
                    return;
                if (animate)
                    FadeOut(element);
                else
                    element.style.display = DisplayStyle.None;
            }
        }
        private static void FadeOut(VisualElement element)
        {
            // Tween.Custom(1, 0, .3f, (value) => element.style.opacity = value)
            //     .OnComplete(() => element.style.display = DisplayStyle.None);
        }
        private static void FadeIn(VisualElement element)
        {
            // element.style.display = DisplayStyle.Flex;
            // Tween.Custom(0, 1, .3f, (value) => element.style.opacity = value);
        }
    }
}
