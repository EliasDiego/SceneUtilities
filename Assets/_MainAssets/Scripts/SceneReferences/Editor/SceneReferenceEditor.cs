using UnityEditor;
using UnityEngine;

namespace YergoLabs.SceneReferences
{
    [CustomPropertyDrawer(typeof(SceneReference))]
    public class SceneGroupPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty nameProperty = property.FindPropertyRelative("_Name");
            SerializedProperty pathProperty = property.FindPropertyRelative("_Path");

            SceneAsset sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(pathProperty.stringValue);

            sceneAsset = (SceneAsset)EditorGUI.ObjectField(position, property.displayName, sceneAsset, typeof(SceneAsset), false);

            if(!sceneAsset)
                return;

            nameProperty.stringValue = sceneAsset.name;
            pathProperty.stringValue = AssetDatabase.GetAssetPath(sceneAsset);

            property.serializedObject.ApplyModifiedProperties();
        }
    }
}