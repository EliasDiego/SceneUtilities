using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using YergoLabs.SceneUtilities.SceneReferences;

namespace YergoLabsEditor.Scenes
{
    [CustomPropertyDrawer(typeof(SceneAssetReference))]
    public class SceneAssetReferencePropertyDrawer : SceneReferencePropertyDrawer
    {
        protected override string GetText(SerializedProperty property)
        {
            SerializedProperty pathProperty = property.FindPropertyRelative("_Path");

            if(string.IsNullOrEmpty(pathProperty.stringValue))
                return string.Empty;

            return GetSceneName(pathProperty.stringValue);
        }

        protected override bool TryEditSceneReference(Rect position, SerializedProperty property)
        {
            SerializedProperty pathProperty = property.FindPropertyRelative("_Path");
            SceneAsset sceneAsset = null;

            if(!string.IsNullOrEmpty(pathProperty.stringValue))
                sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(pathProperty.stringValue);

            sceneAsset = (SceneAsset)EditorGUI.ObjectField(position, property.displayName, sceneAsset, typeof(SceneAsset), false);

            string scenePath = AssetDatabase.GetAssetPath(sceneAsset);

            if(string.IsNullOrEmpty(scenePath) || scenePath == pathProperty.stringValue)
                return false;

            pathProperty.stringValue = scenePath;
            
            return true;
        }
    }
}