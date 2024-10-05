using System.Linq;
using UnityEditor;
using UnityEngine;

namespace YergoLabs.SceneReferences
{
    public abstract class SceneReferencePropertyDrawer : PropertyDrawer
    {
        protected bool _IsEditing = false;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            string[] scenePaths = EditorBuildSettings.scenes.Select(scene => scene.path).ToArray();
            
            Rect buttonRect = new Rect(position.x + position.width - 50, position.position.y, 50, position.height);
            Rect fieldRect = position;

            fieldRect.width -= 50;

            if(_IsEditing)
            {
                _IsEditing = !GUI.Button(buttonRect, "Cancel");

                if(!_IsEditing)
                    return;

                if(TryEditSceneReference(fieldRect, property))
                {
                    property.serializedObject.ApplyModifiedProperties();

                    _IsEditing = false;
                }

                return;
            }

            _IsEditing = GUI.Button(buttonRect, "Edit");

            if(_IsEditing)
                return;

            string text = GetText(property);

            EditorGUI.LabelField(fieldRect, property.displayName, text);
        }

        protected string GetSceneName(string scenePath)
        {
            int lastDividerCount = 0;

            for(int i = 0; i < scenePath.Length; i++)
            {
                if(scenePath[i] == '/')
                    lastDividerCount = i + 1;
            }

            return new string(scenePath.TakeLast(scenePath.Length - lastDividerCount).ToArray());
        }

        protected abstract bool TryEditSceneReference(Rect position, SerializedProperty property);
        protected abstract string GetText(SerializedProperty property);
    }
}