using System.Linq;
using UnityEditor;
using UnityEngine;

namespace YergoLabs.SceneReferences
{
    [CustomPropertyDrawer(typeof(BuildSceneReference))]
    public class BuildSceneReferencePropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty pathProperty = property.FindPropertyRelative("_Path");
            SerializedProperty buildIndexProperty = property.FindPropertyRelative("_BuildIndex");

            string[] scenePaths = EditorBuildSettings.scenes.Select(scene => scene.path).ToArray();

            buildIndexProperty.intValue = EditorGUI.Popup(position, property.displayName, buildIndexProperty.intValue, scenePaths);

            if(buildIndexProperty.intValue == -1 || buildIndexProperty.intValue >= scenePaths.Length)
            {
                pathProperty.stringValue = string.Empty;
                buildIndexProperty.intValue = -1;

                return;
            }

            pathProperty.stringValue = scenePaths[buildIndexProperty.intValue];

            property.serializedObject.ApplyModifiedProperties();
        }
    }
}