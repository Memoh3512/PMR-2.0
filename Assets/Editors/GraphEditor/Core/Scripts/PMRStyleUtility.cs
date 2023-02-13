using UnityEditor;
using UnityEngine.UIElements;

namespace PMR.GraphEditor.Utilities
{
    public static class PMRStyleUtility
    {
        public static VisualElement AddStyleSheets(this VisualElement element, params string[] stylesheetNames)
        {
            foreach (string ssName in stylesheetNames)
            {
                StyleSheet SS = (StyleSheet) EditorGUIUtility.Load(ssName);
            
                element.styleSheets.Add(SS);
            }

            return element;
        }

        public static VisualElement AddClasses(this VisualElement element, params string[] classNames)
        {
            foreach (string className in classNames)
            {
                element.AddToClassList(className);
            }

            return element;
        }
    }
}
