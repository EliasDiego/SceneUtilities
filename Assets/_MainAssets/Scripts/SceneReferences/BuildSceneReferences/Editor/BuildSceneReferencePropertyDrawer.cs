using System.Linq;
using UnityEditor;
using UnityEngine;

namespace YergoLabs.SceneReferences
{
    [CustomPropertyDrawer(typeof(BuildSceneReference))]
    public class BuildSceneReferencePropertyDrawer : SceneReferencePropertyDrawer
    {
        protected override string GetText(SerializedProperty property)
        {
            SerializedProperty buildIndexProperty = property.FindPropertyRelative("_BuildIndex");
            string[] scenePaths = EditorBuildSettings.scenes.Select(scene => scene.path).ToArray();

            if(buildIndexProperty.intValue >= scenePaths.Length || buildIndexProperty.intValue < 0)
                return string.Empty;

            int buildIndex = buildIndexProperty.intValue;
            string sceneName = GetSceneName(scenePaths[buildIndexProperty.intValue]);

            return $"[{buildIndex}] {sceneName}";
        }

        // Rect position, Rect buttonRect, SerializedProperty property, SerializedProperty buildIndexProperty, string[] scenePaths
        protected override bool TryEditSceneReference(Rect position, SerializedProperty property)
        {
            SerializedProperty buildIndexProperty = property.FindPropertyRelative("_BuildIndex");
            string[] scenePaths = EditorBuildSettings.scenes.Select(scene => scene.path).ToArray();

            int originalValue = buildIndexProperty.intValue; 

            buildIndexProperty.intValue = EditorGUI.Popup(position, property.displayName, buildIndexProperty.intValue, scenePaths);

            if(originalValue == buildIndexProperty.intValue)
                return false;

            if(buildIndexProperty.intValue == -1 || buildIndexProperty.intValue >= scenePaths.Length)
            {
                buildIndexProperty.intValue = -1;

                return false;
            }

            return true;
        }
    }
}